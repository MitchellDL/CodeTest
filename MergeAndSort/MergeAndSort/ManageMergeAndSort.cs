using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MergeAndSort
{
    class ManageMergeAndSort 
    {
        //xml out files
        private static string xmlFileOnePath = "C:\\output\\data1.xml";
        private static string xmlFileTwoPath = "C:\\output\\data2.xml";

        //path to xml files, sorted by child element 
        private static string xmlSortedOnePath = "C:\\output\\sortedData1.xml";
        private static string xmlSortedTwoPath = "C:\\output\\sortedData2.xml";

        //path to xml files, sorted by child element 
        private static string xmlSortedPath = "C:\\output\\sortedData.xml"; 
        
        private static string result = "";
       
        public void FormatFile(string filePath, string fileName)
        {
            foreach (string line in File.ReadLines(filePath))
            {
                //remove comma character between double quotes but keep file in csv format
                //stackoverflow.com/questions/5202005/regex-how-to-remove-comma-which-is-between-and
                result = Regex.Replace(line, @",(?=[^""]*""(?:[^""]*""[^""]*"")*[^""]*$)", String.Empty);

                //remove tab character
                if (result.Contains("\t"))
                {
                    result = result.Replace("\t", ",");
                }

                //write to new file
                using (StreamWriter file = new StreamWriter(fileName, true))
                {
                    if (!result.Contains("firstname"))
                    {
                        file.WriteLine(result);
                        Console.WriteLine(result);
                    }
                }
            }
        }

        //getcodesnippet.com/2014/05/15/how-to-convert-csv-file-to-xml-file-in-c/
        //TODO: make one method to handle csv file with different order.
        //TODO: add random id to xml file or remove id from data.xml
        public void CreateXmlFileOne(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            XElement xml = new XElement("People",
                from str in lines
                let columns = str.Split(',')
                select new XElement("person",
                    new XAttribute("id", 1),
                    new XElement("lastname", columns[0]),
                    new XElement("firstname", columns[1]),
                    new XElement("age", columns[2]),
                    new XElement("balance", columns[3])
               )
            );
            xml.Save(xmlFileOnePath);

            XDocument doc = XDocument.Load(xmlFileOnePath);
            SortElementsByNameInPlace(doc.Root);
            doc.Save(xmlSortedOnePath);
        }

        public void CreateXmlFileTwo(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            XElement xml = new XElement("People",
                from str in lines
                let columns = str.Split(',')
                select new XElement("person",
                    new XAttribute("id", 1),
                    new XElement("firstname", columns[0]),
                    new XElement("lastname", columns[1]),
                    new XElement("balance", columns[2]),
                    new XElement("age", columns[3])
               )
            );
            xml.Save(xmlFileTwoPath);

            XDocument doc = XDocument.Load(xmlFileTwoPath);
            SortElementsByNameInPlace(doc.Root);
            doc.Save(xmlSortedTwoPath);
        }

        public void SortOriginalXmlFile(string fileName)
        {
            // sort original xml file to match new xml files
            XDocument doc = XDocument.Load(fileName);
            this.SortElementsByNameInPlace(doc.Root);
            doc.Save(xmlSortedPath);
        }
        
        public void SortElementsByNameInPlace(XContainer xContainer)
        {
            //social.msdn.microsoft.com/Forums/en-US/c420b5da-4678-4f5e-8de6-8844efa8b776/how-to-sort-xdocument-content-by-node-name?forum=xmlandnetfx
            foreach (XElement element in xContainer.Descendants())
            {
                var orderedElements = (from child in element.Elements()
                                       orderby child.Name.LocalName
                                       select child).ToList();  // ToList matters, since we remove all of the child elements next

                element.Elements().Remove();
                element.Add(orderedElements);
            }
        }


        internal void MergeXmlDocuments()
        {
            //stackoverflow.com/questions/3440073/how-to-merge-two-xmldocuments-in-c-sharp
            XmlDocument originalXml = new XmlDocument();
            originalXml.Load(xmlSortedPath);
            XmlDocument xmlOne = new XmlDocument();
            xmlOne.Load(xmlSortedOnePath);
            XmlDocument xmlTwo = new XmlDocument();
            xmlTwo.Load(xmlSortedTwoPath);

            //combine original xml and first created xml
            foreach (XmlNode node in xmlOne.DocumentElement.ChildNodes)
            {
                XmlNode imported = originalXml.ImportNode(node, true);
                originalXml.DocumentElement.AppendChild(imported);
                originalXml.Save("C:\\output\\combined.xml");
            }

            //further combine and append xmlTwo 
            XmlDocument combinedXml = new XmlDocument();
            combinedXml.Load("C:\\output\\combined.xml");
            foreach (XmlNode node in xmlTwo.DocumentElement.ChildNodes)
            {
                XmlNode imported = combinedXml.ImportNode(node, true);
                combinedXml.DocumentElement.AppendChild(imported);
                combinedXml.Save("C:\\output\\totalCombined.xml");
            }
        }

        internal void SortXmlByAge()
        {
            //---- print xml according to sort order
            Console.WriteLine("You selected sorting by age");

            //-- sort by age
           //XElement data = XElement.Load("C:\\output\\combined.xml");
           //var people = data.Elements("person").OrderBy(p => (string)p.Attribute("age")).ToArray();
           //data.Descendants("people").Remove();

            //-- save sorted 
           //data.Element("person").Add(people);
           //data.Save("C:\\output\\sortByAge.xml");

            //--Open and print to screen
            //csharp.net-informations.com/xml/how-to-read-xml.htm
            //XmlDataDocument xmldoc = new XmlDataDocument(); 
            //XmlNodeList xmlnode; int i = 0; 
            //string str = null;
            //FileStream fs = new FileStream("C:\\output\\sortByAge.xml", FileMode.Open, FileAccess.Read); xmldoc.Load(fs); 
            //xmlnode = xmldoc.GetElementsByTagName("person"); 
            //for (i = 0; i <= xmlnode.Count - 1; i++) 
            //{ 
            //    xmlnode[i].ChildNodes.Item(0).InnerText.Trim(); 
            //    str = xmlnode[i].ChildNodes.Item(0).InnerText.Trim() + " " + 
            //        xmlnode[i].ChildNodes.Item(1).InnerText.Trim() + " " + 
            //        xmlnode[i].ChildNodes.Item(2).InnerText.Trim();
            //   Console.WriteLine(str); 
            //}


            
            

        }

        internal void SortXmlAlphebeticallyDesc()
        {
            //---- print xml according to sort order
            Console.WriteLine("You selected sorting by Alphebetically Desc");
            
        }

        internal void SortXmlAlphebeticallyAesc()
        {
            //---- print xml according to sort order
            Console.WriteLine("You selected sorting by Alphebetically Aesc");
        }
    }
}