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


        public void fillForm(WorkWeek scheiss)
        {
            string newFile = @"e:\Files\Downloads\wochenbericht_finished.pdf";
            PdfReader pdfReader = new PdfReader(pdfTemplate);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create));
            AcroFields pdfFormFields = pdfStamper.AcroFields;

            //Template has Text1-Text25 Fields
            pdfFormFields.SetField("Text1", "12");
            pdfFormFields.SetField("Text2", "12");
            pdfFormFields.SetField("Text3", "12");
            pdfFormFields.SetField("Text4", "12");
            pdfFormFields.SetField("Text5", "12");
            pdfFormFields.SetField("Text6", "12");
            pdfFormFields.SetField("Text6", "12");
            //Montag
            string[] wochentag = new string[5];
            int i = 0;
            while (i < 5)
            {
                foreach(WorkWeek.SingleTask singleTask in scheiss.WeekDayArray[i])
                {
                    if (!singleTask.Description.Contains("Daily"))
                    {
                        wochentag[i] +=  "["+ singleTask.Description +"] "+ singleTask.Besamung + "\n";
                    }
                }
                i++;
            }
            pdfFormFields.SetField("Text7", wochentag[0]);
            pdfFormFields.SetField("Text8", "12");
            pdfFormFields.SetField("Text9", "12");
            pdfFormFields.SetField("Text10", wochentag[1]);
            pdfFormFields.SetField("Text11", "12");
            pdfFormFields.SetField("Text12", "12");
            pdfFormFields.SetField("Text13", wochentag[2]);
            pdfFormFields.SetField("Text14", "12");
            pdfFormFields.SetField("Text15", "12");
            pdfFormFields.SetField("Text16", wochentag[3]);
            pdfFormFields.SetField("Text17", "12");
            pdfFormFields.SetField("Text18", "12");
            pdfFormFields.SetField("Text19", wochentag[4]);
            pdfFormFields.SetField("Text20", "12");
            pdfFormFields.SetField("Text21", "12");
            pdfFormFields.SetField("Text22", "12");
            pdfFormFields.SetField("Text23", "12");
            pdfFormFields.SetField("Text24", "12");
            pdfFormFields.SetField("Text25", "12");


            // flatten the form to remove editting options, set it to false  
            // to leave the form open to subsequent manual edits  
            pdfStamper.FormFlattening = false;
            // close the pdf  
            pdfStamper.Close();
        }

        #region Private Member Variables
        string pdfTemplate = @"e:\Files\Downloads\wochenbericht_template.pdf";

        #endregion
    }

}

