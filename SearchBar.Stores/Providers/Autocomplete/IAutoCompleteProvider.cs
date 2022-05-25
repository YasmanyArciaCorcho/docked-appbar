using Common.Suggestion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Stores.Providers.Autocomplete
{
    public interface IAutoCompleteProvider : INotifyPropertyChanged
    {
        IList<IAutocompleteSuggestion> AutocompleteSuggestions
        { get; set; }

        Task UpdateSuggestions(string query);

        Task UpdateSuggestions(string query, int quantityDesiredItems);
    }
}
