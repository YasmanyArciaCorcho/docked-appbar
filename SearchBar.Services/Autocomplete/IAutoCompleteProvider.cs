using Common.Suggestion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Autocomplete
{
    public interface IAutoCompleteProvider : INotifyPropertyChanged
    {
        IList<IAutocompleteSuggestion> AutocompleteSuggestions
        { get; set; }

        Task UpdateSuggestions(string query);

        Task UpdateSuggestions(string query, int quantityDesiredItems);
    }
}
