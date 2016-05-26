using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace MergeAndSort
{
    class Program
    {
        // input files
        private static string xmlFilePath = "C:\\data.xml";
        private static string txtFileOnePath = "C:\\data1.txt";
        private static string txtFileTwoPath = "C:\\data2.txt";
        //csv output files
        private static string csvFileOneName = "C:\\output\\data1.csv";
        private static string csvFileTwoName = "C:\\output\\data2.csv";


        static void Main(string[] args)
        {
            ManageMergeAndSort manage = new ManageMergeAndSort();
            //---- format txt files 
            //NOTE place text files at the root of C
            manage.FormatFile(txtFileOnePath, csvFileOneName);
            manage.FormatFile(txtFileTwoPath, csvFileTwoName);

            //---- create xml files 
            //NOTE: the column in the csv files are in a different order that is why there are two methods, 
            //TODO: find a solution with one method, if I have time.
            manage.CreateXmlFileOne(csvFileOneName);
            manage.CreateXmlFileTwo(csvFileTwoName);
            manage.SortOriginalXmlFile(xmlFilePath);

            //---- merge xml files into one file
            manage.MergeXmlDocuments();

            //---- ask user for promts to sort xml file
            Console.WriteLine("To Sort by age press 1, To Sort by last name alphebetically (acending) press 2, .alphebetically (decending) press 3.");
             var choice =  int.Parse(Console.ReadLine());

            
             //---- print xml according to sort order
             //TODO: put in catch if user presses another key besides 1,2 or 5
             if (choice == 1)
             {
                 manage.SortXmlByAge();
             }
             else if (choice == 2)
             {
                 manage.SortXmlAlphebeticallyDesc();

             }
             else if (choice == 3)
             {
                 manage.SortXmlAlphebeticallyAesc();
             }     

            // keep console open for user
            Console.ReadKey();
        }

        
    }
}

