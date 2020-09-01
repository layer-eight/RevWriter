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


        public void fillForm()
        {
            string newFile = @"e:\Files\Downloads\wochenbericht_finished.pdf";
            PdfReader pdfReader = new PdfReader(pdfTemplate);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create));
            AcroFields pdfFormFields = pdfStamper.AcroFields;

            pdfFormFields.SetField("Text1", "12");

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

