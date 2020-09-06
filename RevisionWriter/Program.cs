using iTextSharp.text.pdf;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;

namespace RevisionWriter
{
    class Program
    {
        static void Main(string[] args)
        {
            string htmlLukas = File.ReadAllText("Lukas.html");
            string htmlHannes = File.ReadAllText("Hannes.html");

            Regex reg = new Regex("(?<=class=\"    \">)(.*)(?=<\\/t)");

            WorkWeek scheiss = new WorkWeek();

            PdfParser PdfParser = new PdfParser();
            ObservableCollection<WorkWeek.SingleTask> TaskCollection = scheiss.posoFactory(reg, htmlLukas);


            
            PdfParser.ListFieldNames();
            
            PdfParser.fillForm(scheiss.SortTasksByDay(Convert.ToDateTime("25.05.2020"), TaskCollection));




            Console.WriteLine(scheiss.posoFactory(reg, htmlHannes).ToString());
            Console.WriteLine(scheiss.posoFactory(reg, htmlLukas).ToString());
            Console.ReadLine();
        }
    }
}
