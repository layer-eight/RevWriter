using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RevisionWriter
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Choose a file to read: ");
            OpenFileDialog fbd = new OpenFileDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                //foreach (var path in Directory.GetFiles(fbd.SelectedPath))
                
                Console.WriteLine(Path.GetFileName(fbd.FileName)); // full path
                
            }

            Console.WriteLine("Choose department you work in: e.g. Software Engineering");
            string department = Console.ReadLine();
            Console.WriteLine("Which is the current revision number: ");
            int revisionNumber = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Which year of education are you currently in: ");
            string yearOfEducation = Console.ReadLine();
            //Console.WriteLine("How many revisions: ");
            //int repeat = Convert.ToInt32(Console.ReadLine());


            string html = File.ReadAllText(fbd.FileName);



            Regex reg = new Regex("(?<=class=\"    \">)(.*)(?=<\\/t)");
            WorkWeek scheiss = new WorkWeek();
            PdfParser PdfParser = new PdfParser();
            ObservableCollection<Task> TaskCollection = scheiss.makeTasksFromHtml(reg, html);
            List<WorkWeek> Weeks = scheiss.MakeWorkWeeks(TaskCollection);
            Console.WriteLine(scheiss.GetTotalWorkDayCount(TaskCollection));

            //fill form 
            PdfParser.fillForm(Weeks, department, revisionNumber, yearOfEducation);


            Console.WriteLine("Program executed! Press Enter to leave."); 
            Console.ReadLine();
        }
    }
}
