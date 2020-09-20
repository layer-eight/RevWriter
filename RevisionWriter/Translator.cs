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
    public class Translat0r
    {

        public string TranslateText(string input)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            TranslationClient client = TranslationClient.Create();
            var response = client.TranslateText(
                text: input,
                targetLanguage: "de",  // German
                sourceLanguage: "en");  // English
            Console.WriteLine(response.TranslatedText);
            return response.TranslatedText;
        }
    }
}
