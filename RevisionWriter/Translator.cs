using Nancy.Helpers;
using Nancy.Json;
using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Google.Cloud.Translation.V2;


namespace Translator
{
    public class Translator
    {

        public string TranslateText()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            TranslationClient client = TranslationClient.Create();
            var response = client.TranslateText(
                text: "Hello World.",
                targetLanguage: "ru",  // Russian
                sourceLanguage: "en");  // English
            Console.WriteLine(response.TranslatedText);
            return response.TranslatedText;
        }
    }
}
