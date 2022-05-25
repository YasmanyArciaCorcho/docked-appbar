using Common.Suggestion;
using SearchBar.UI.Base;
using System.ComponentModel;

namespace SearchBar.UI.SearchBarViewModel
{
    public interface ISearchBarViewModel: INotifyPropertyChanged
    {
        delegate void DoSelectItem(string selectedItem);
        event DoSelectItem SearchSelectedItemEvent;
        event DoSelectItem OpenDirectlySelectedItemEvent;

        AsyncObservableCollection<IAutocompleteSuggestion> ItemsToSelect 
        { get; set; }

        IAutocompleteSuggestion SelectedItem 
        { get; set; }

        void SetQuery(string query);

        void SearchQuery(string selectedAutoComplete);

        void OpenDirectlyQuery(string selectedAutoComplete);
    }
}

