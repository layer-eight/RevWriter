using iTextSharp.text.pdf;
using System;
using System.IO;
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
            
            ScheissDenWirBrauchen scheiss = new ScheissDenWirBrauchen();

            PdfParser pdfReader = new PdfParser();


            pdfReader.ListFieldNames();
            pdfReader.fillForm();




            Console.WriteLine(scheiss.posoFactory(reg, htmlHannes).ToString());
            Console.WriteLine(scheiss.posoFactory(reg, htmlLukas).ToString()); 
            Console.ReadLine();
        }
    }
}
