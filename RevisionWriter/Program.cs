using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace RevisionWriter
{
    class Program
    {
        [STAThread]
        static void  Main(string[] args)
        {
            Console.WriteLine("Choose a file to read: ");
            OpenFileDialog fbd = new OpenFileDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {   
                Console.WriteLine(Path.GetFileName(fbd.FileName));   
            }

            Console.WriteLine("Choose department you work in: e.g. Software Engineering");
            string department = Console.ReadLine();
            Console.WriteLine("Which is the current revision number: ");
            int revisionNumber = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Which year of education are you currently in: ");
            string yearOfEducation = Console.ReadLine();

            string html = File.ReadAllText(fbd.FileName);

            Regex reg = new Regex("(?<=class=\"    \">)(.*)(?=<\\/t)");
            WorkWeek scheiss = new WorkWeek();
            PdfParser PdfParser = new PdfParser();
            ObservableCollection<Task> TaskCollection = scheiss.makeTasksFromHtml(reg, html);

            PdfParser.fillForm(scheiss.MakeWorkWeeks(TaskCollection.ToList()), department, revisionNumber, yearOfEducation);

            Console.WriteLine("Program executed! Press Enter to leave."); 
            Console.ReadLine();
        }
    }
}
