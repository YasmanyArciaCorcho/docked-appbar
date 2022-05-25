using Common.Logger;
using Common.Suggestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Autocomplete.OnlineSuggestion.Google
{
    public class GoogleSearchSuggestionsService : OnlineSuggestionService
    {
        readonly GoogleSearchSuggestions _searchSuggestions;

        public GoogleSearchSuggestionsService()
        {
            _searchSuggestions = new GoogleSearchSuggestions();
        }

        public override async Task<bool> Load()
        {
            await Task.Yield();
            return true;
        }

        public override async Task<bool> Save()
        {
            await Task.Yield();
            return true;
        }

        public async override Task UpdateSuggestions(string query, int quantityDesiredItems)
        {
            List<IAutocompleteSuggestion> temporalSuggestions = new List<IAutocompleteSuggestion>();
            int amountElementInserted = 0;
            foreach (var suggestion in await _searchSuggestions.GetSearchSuggestions(query))
            {
                temporalSuggestions.Add(suggestion);
                if (amountElementInserted == quantityDesiredItems)
                    break;
                amountElementInserted++;
            }

            AutocompleteSuggestions = temporalSuggestions;
            OnPropertyChanged(nameof(AutocompleteSuggestions));
            StaticLogger.Logger.Info($"Google search suggestion service - updated suggestions.");
        }
    }
}
