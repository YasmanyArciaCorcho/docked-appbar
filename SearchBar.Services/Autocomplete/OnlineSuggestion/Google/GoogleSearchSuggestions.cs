using Common;
using Common.Suggestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Services.Autocomplete.OnlineSuggestion.Google
{
    public class GoogleSearchSuggestions
    {
        /// <summary>
        /// The Google Suggest search URL.
        /// </summary>
        /// <remarks>
        /// Add gl=dk for Google Denmark. Add lr=lang_da for danish results. Add hl=da to indicate the language of the UI making the request.
        /// </remarks>
        private const string _suggestSearchUrl = "http://www.google.com/complete/search?output=toolbar&q={0}&hl=en";

        /// <summary>
        /// Gets the search suggestions from Google.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A list of <see cref="IAutocompleteSuggestion"/>s.</returns>
        public async Task<List<IAutocompleteSuggestion>> GetSearchSuggestions(string query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException("Argument cannot be null or empty!", "query");
            }

            string queryResult = await HTTPRequestHelper.DoAsyncQuery(String.Format(_suggestSearchUrl, query));

            XDocument doc = XDocument.Parse(queryResult);

            var suggestions = from suggestion in doc.Descendants("CompleteSuggestion")
                              select new GoogleSuggestion
                              {
                                  Value = suggestion.Element("suggestion").Attribute("data").Value
                              };

            List<IAutocompleteSuggestion> result = new List<IAutocompleteSuggestion>(suggestions);
            return result;
        }
    }

    /// <summary>
    /// Encapsulates a suggestion from Google.
    /// </summary>
    public class GoogleSuggestion : AutoCompleteSuggestion
    {
    }
}
