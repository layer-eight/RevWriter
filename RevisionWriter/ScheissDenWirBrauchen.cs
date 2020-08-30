using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace RevisionWriter
{
    /// <summary>
    /// ScheissDenWirBrauchenKlasse contains all properties needed to write a azubi revision. 
    /// Three of which are obligatory properties(Besamung, Description, Date) 
    /// and three optional (User, ProjectID, Duration).
    /// </summary>
    public class ScheissDenWirBrauchen
    {

        public ScheissDenWirBrauchen(){ }

            public string Date { get; set; }
            public string User { get; set; }
            public string Description { get; set; }
            public string Besamung { get; set; }
            public string ProjectID { get; set; }
            public string Duration { get; set; }

        /// <summary>
        /// Method to extract all ScheissDenWirBrauchenKlasse properties from odoo html file into collection of ScheissDenWirBrauchenKlasse.
        /// Takes regex and html file as parameter. Returns ObservableCollection<ScheissDenWirBrauchenKlasse>
        /// </summary>
        /// <param name="reg">Regex to get text in between html tags</param>
        /// <param name="html">Odoo html file</param>
        /// <returns>ObservableCollection<ScheissDenWirBrauchenKlasse></returns>
        public ObservableCollection<ScheissDenWirBrauchen> posoFactory(Regex reg, string html)
        {
            MatchCollection matches = reg.Matches(html);
            int ScheisseDieWirBrauchenAnzahl = matches.Count;

            ObservableCollection<ScheissDenWirBrauchen> scheißDenWirBrauchenCollection = new ObservableCollection<ScheissDenWirBrauchen>();
            for (int i = 0; i < ScheisseDieWirBrauchenAnzahl; i++)
            {
                ScheissDenWirBrauchen azubiRevision = new ScheissDenWirBrauchen();
                azubiRevision.Date = matches[i].ToString();
                azubiRevision.User = matches[i + 1].ToString();
                azubiRevision.Besamung = matches[i + 2].ToString();
                azubiRevision.ProjectID = matches[i + 3].ToString();
                azubiRevision.Description = matches[i + 4].ToString();

                i += 4;

                scheißDenWirBrauchenCollection.Add(azubiRevision);
            }
            return scheißDenWirBrauchenCollection;
        }
    }
}

