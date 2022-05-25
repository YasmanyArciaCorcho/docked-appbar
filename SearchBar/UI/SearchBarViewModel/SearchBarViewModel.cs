using Common.Suggestion;
using SearchBar.UI.Base;
using Services.Autocomplete;
using System.ComponentModel;

namespace SearchBar.UI.SearchBarViewModel
{
    public class SearchBarViewModel : ViewModelBase, ISearchBarViewModel
    {
        const int AmountAutocompleteSuggestion = 7;
        private readonly object updateLook = new object();

        AsyncObservableCollection<IAutocompleteSuggestion> _itemsToSelect;
        IAutocompleteSuggestion _selectedItem;
        readonly ISearchAutocompleteService _searchAutocompleteService;

        public event ISearchBarViewModel.DoSelectItem SearchSelectedItemEvent;
        public event ISearchBarViewModel.DoSelectItem OpenDirectlySelectedItemEvent;

        public SearchBarViewModel(ISearchAutocompleteService searchAutocompleteService)
        {
            _searchAutocompleteService = searchAutocompleteService;
            _searchAutocompleteService.PropertyChanged += OnSearchAutocompleteService_PropertyChange;

            _itemsToSelect = new AsyncObservableCollection<IAutocompleteSuggestion>();
            if (_itemsToSelect.Count > 0)
                _selectedItem = _itemsToSelect[0];
        }

        public AsyncObservableCollection<IAutocompleteSuggestion> ItemsToSelect
        {
            get
            {
                return this._itemsToSelect;
            }
            set
            {
                if (value == _itemsToSelect)
                    return;
                this._itemsToSelect = value;
            }
        }

        public IAutocompleteSuggestion SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if (ReferenceEquals(value, _selectedItem))
                    return;
                _selectedItem = value;
            }
        }

        public async void SetQuery(string query)
        {
            if (!string.IsNullOrEmpty(query))
                await _searchAutocompleteService.UpdateSuggestions(query, AmountAutocompleteSuggestion);
        }

        public void SearchQuery(string selectedItem)
        {
            if (!_searchAutocompleteService.ContainsHistory(selectedItem.Trim()))
                _searchAutocompleteService.AddHistory(selectedItem);
            SearchSelectedItemEvent(selectedItem);
        }

        private void OnSearchAutocompleteService_PropertyChange(object sender, PropertyChangedEventArgs e)
        {
            lock (updateLook)
            {
                ItemsToSelect.Clear();
                int insertedSuggest = 0;
                foreach (var suggest in _searchAutocompleteService.AutocompleteSuggestions)
                {
                    ItemsToSelect.Add(suggest);
                    insertedSuggest++;

                    if (insertedSuggest == AmountAutocompleteSuggestion)
                        break;

                }
                SelectedItem = ItemsToSelect[0];
                OnPropertyChanged(nameof(ItemsToSelect));
            }
        }

        public void OpenDirectlyQuery(string selectedAutoComplete)
        {
            OpenDirectlySelectedItemEvent(selectedAutoComplete);
        }
    }
}
