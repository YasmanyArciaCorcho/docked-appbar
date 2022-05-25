using Common;
using Common.Logger;
using Common.Suggestion;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Stores.Providers.Autocomplete
{
    public abstract class AutoCompleteProviderBase : ProviderBase, IAutoCompleteProvider
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IList<IAutocompleteSuggestion> AutocompleteSuggestions
        { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }

           StaticLogger.Logger.Info($"{this.GetType().Name} - On Property Changed: {propertyName}.");
        }

        public async virtual Task UpdateSuggestions(string query)
        {
            await UpdateSuggestions(query, int.MaxValue);
        }

        public abstract Task UpdateSuggestions(string query, int quantityDesiredItems);
    }
}
