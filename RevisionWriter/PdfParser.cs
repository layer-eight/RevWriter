using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public void fillForm(WorkWeek scheiss, string department, int nachweisNummer, string jahr, int repeat, string date)
        {
            DateTime weekcount = Convert.ToDateTime(date);
            while (repeat >= 0)
            {
                string newFile = @"e:\Files\Downloads\wochenbericht_"+ nachweisNummer+".pdf";
                PdfReader pdfReader = new PdfReader(pdfTemplate);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create));
                AcroFields pdfFormFields = pdfStamper.AcroFields;
                //Template has Text1-Text25 Fields
                pdfFormFields.SetField("Text1", "Lukas Eittenberger");
                pdfFormFields.SetField("Text2", department);
                pdfFormFields.SetField("Text3", nachweisNummer.ToString());
                pdfFormFields.SetField("Text4", weekcount.ToString());
                weekcount = weekcount.AddDays(4);
                pdfFormFields.SetField("Text5", weekcount.ToString());
                pdfFormFields.SetField("Text6", jahr);
                //Montag
                string[] wochentag = new string[5];

                int i = 0;
                while (i < 5)
                {
                    foreach (WorkWeek.SingleTask singleTask in scheiss.WeekDayArray[i])
                    {
                        if (!singleTask.Description.Contains("Daily"))
                        {
                            wochentag[i] += "[" + singleTask.Description + "] " + singleTask.Besamung + "\n";
                        }
                    }
                    i++;
                }
                pdfFormFields.SetField("Text7", wochentag[0]);

                pdfFormFields.SetField("Text10", wochentag[1]);

                pdfFormFields.SetField("Text13", wochentag[2]);

                pdfFormFields.SetField("Text16", wochentag[3]);

                pdfFormFields.SetField("Text19", wochentag[4]);

                repeat--;
                nachweisNummer++;
                weekcount = weekcount.AddDays(3);

                // flatten the form to remove editting options, set it to false  
                // to leave the form open to subsequent manual edits  
                pdfStamper.FormFlattening = false;
                // close the pdf  
                pdfStamper.Close();
            }
        }

        #region Private Member Variables
        string pdfTemplate = @"e:\Files\Downloads\wochenbericht_template.pdf";

        #endregion
    }

}

