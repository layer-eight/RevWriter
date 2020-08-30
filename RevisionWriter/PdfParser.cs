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

        public string ParsePdf(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("fileName");
            using (PdfReader reader = new PdfReader(fileName))
            {
                StringBuilder sb = new StringBuilder();

                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                for (int page = 0; page < reader.NumberOfPages; page++)
                {
                    string text = PdfTextExtractor.GetTextFromPage(reader, page + 1, strategy);
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        sb.Append(Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(text))));
                    }
                }

                return sb.ToString();
            }
        }
    }

}

