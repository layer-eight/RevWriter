using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static RevisionWriter.Translator;


namespace RevisionWriter
{
    public class PdfParser
    {
        public string Text { get; private set; }

        /// <summary>  
        /// List all of the form fields into a textbox.  The  
        /// form fields identified can be used to map each of the  
        /// fields in a PDF.  
        /// </summary>  
        public void ListFieldNames()
        { 
            // create a new PDF reader based on the PDF template document  
            PdfReader pdfReader = new PdfReader(pdfTemplate);
            // create and populate a string builder with each of the  
            // field names available in the subject PDF  
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, iTextSharp.text.pdf.AcroFields.Item> de in pdfReader.AcroFields.Fields)
            {
                sb.Append(de.Key.ToString() + Environment.NewLine);
            }
            // Write the string builder's content to the form's textbox  
            Text = sb.ToString();
        }

        public void fillForm(List<WorkWeek> workWeeks, string department, int nachweisNummer, string jahr)
        {
            RestClient rc = new RestClient();

            string input = Console.ReadLine();
            rc.endPoint = "https://api.deepl.com/v2/translate?text=" + input + "&target_lang=EN&auth_key=f355d8ac-c493-1ae5-b4df-a3b132d1632d";
            Console.WriteLine(rc.makeRequest());
            //{"translations":[{"detected_source_language":"DE","text":"{{{ Tree }}"}]}

            int repeat = workWeeks.Count();
            string name = GetUserName(workWeeks);
            
            int weekIndex = 0;
            while (repeat > 0)
            {

                string newFile = @"e:\Files\Downloads\wochenbericht_"+ nachweisNummer+".pdf";
                PdfReader pdfReader = new PdfReader(pdfTemplate);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create));
                AcroFields pdfFormFields = pdfStamper.AcroFields;
                //Template has Text1-Text25 Fields
                pdfFormFields.SetField("Text1", name);
                pdfFormFields.SetField("Text2", department);
                pdfFormFields.SetField("Text3", nachweisNummer.ToString());                
                pdfFormFields.SetField("Text4", CalcRevisionStartDate(workWeeks[weekIndex]).ToString());
                pdfFormFields.SetField("Text5", CalcRevisionStartDate(workWeeks[weekIndex]).AddDays(4).ToString());
                pdfFormFields.SetField("Text6", jahr);
                //Montag
                string[] wochentag = new string[5];
                int dayOfWeek = 0;
                while (dayOfWeek < 5)
                {
                    foreach (Task task in workWeeks[weekIndex].WorkDayCollection[dayOfWeek])
                    {
                        if (!task.Id.Contains("Daily"))
                        {
                            wochentag[dayOfWeek] += "[" + task.Id + "] " + task.Description + "\n";
                        }
                    }
                    dayOfWeek++;
                }
                pdfFormFields.SetField("Text7", wochentag[0]);

                pdfFormFields.SetField("Text10", wochentag[1]);

                pdfFormFields.SetField("Text13", wochentag[2]);

                pdfFormFields.SetField("Text16", wochentag[3]);

                pdfFormFields.SetField("Text19", wochentag[4]);

                repeat--;
                nachweisNummer++;
                weekIndex++;

                // flatten the form to remove editting options, set it to false  
                // to leave the form open to subsequent manual edits  
                pdfStamper.FormFlattening = false;
                // close the pdf  
                pdfStamper.Close();
            }
        }
        
        private DateTime CalcRevisionStartDate(WorkWeek week)
        {
            DateTime firstDay = new DateTime();
            
            int i = 0;
            while(firstDay.Equals(new DateTime()))
            {
                if (week.WorkDayCollection[i].Count() == 0)
                    i++;
                else
                    firstDay = week.WorkDayCollection[i].First().Date;
            }

            switch (firstDay.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    break;
                case DayOfWeek.Tuesday:
                    firstDay = firstDay.AddDays(-1);
                    break;
                case DayOfWeek.Wednesday:
                    firstDay = firstDay.AddDays(-2);
                    break;
                case DayOfWeek.Thursday:
                    firstDay = firstDay.AddDays(-3);
                    break;
                case DayOfWeek.Friday:
                    firstDay = firstDay.AddDays(-4);
                    break;
            }
            return firstDay;
        }

        private string GetUserName(List<WorkWeek> workWeeks)
        {
            int i = 0;
            string name = "";
            while (string.IsNullOrEmpty(name))
            {
                if (workWeeks.First().WorkDayCollection[i].Count() == 0)
                    i++;
                else
                    name = workWeeks.First().WorkDayCollection[i].First().User;
            }
            return name;
        }

        #region Private Member Variables
        string pdfTemplate = @"e:\Files\Downloads\wochenbericht_template.pdf";

        #endregion
    }

}

