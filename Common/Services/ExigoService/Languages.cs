using Common;
using System.Collections.Generic;
using System.Linq;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static IEnumerable<Language> GetLanguages()
        {
            // Get a list of the available languages
            var availableLanguageIDs = GlobalSettings.Globalization.AvailableLanguages.Select(c => c.LanguageID).ToList();
            if (availableLanguageIDs.Count == 0) yield break;

            string availableLangIDs = string.Join(", ", availableLanguageIDs.Select(s => s));
            using (var context = Exigo.Sql())
            {
                string sqlProcedure = string.Format("GetLanguagesByID {0}", availableLangIDs);
                List<Language> results = context.Query<Language>(sqlProcedure).ToList();
                // Populate the available language or the one we got back from the server.
                foreach (var result in results)
                {
                    yield return result;
                }
            }

        }
        public static Language GetLanguage(int languageID)
        {
            // Try to return the first available language we have 
            var result = GlobalSettings.Globalization.AvailableLanguages.Where(c => c.LanguageID == languageID).FirstOrDefault();
            if (result != null)
            {
                return result;
            }

            // If we couldn't find it, get the languages and return it
            return GetLanguages().Where(c => c.LanguageID == languageID).FirstOrDefault();
        }
    }
}