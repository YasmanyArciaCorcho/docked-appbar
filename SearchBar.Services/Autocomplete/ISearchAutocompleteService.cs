using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Autocomplete
{
    public interface ISearchAutocompleteService: IAutoCompleteProvider, INotifyPropertyChanged
    {
        void AddHistory(string item);

        bool ContainsHistory(string item);

        void ClearHistory();
    }
}
