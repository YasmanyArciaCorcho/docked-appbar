using Services.Autocomplete.OnlineSuggestion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Common.Logger;
using Common.Suggestion;
using Stores.Providers.Autocomplete;
using Stores.Providers.History;

namespace Services.Autocomplete
{
    public class SearchAutocompleteService : AutoCompleteProviderBase, ISearchAutocompleteService
    {
        bool _updatingProviders;
        readonly IHistoryProvider _historyProvider;
        readonly IOnlineSuggestionService _onlineSuggestionService;

        public SearchAutocompleteService(IHistoryProvider historyService, IOnlineSuggestionService onlineSuggestionService)
        {
            _historyProvider = historyService;
            _onlineSuggestionService = onlineSuggestionService;
            _historyProvider.PropertyChanged += OnProvidersPropertyChanged;
        }

        public void AddHistory(string item)
        => _historyProvider.Add(item);

        public void ClearHistory()
        => _historyProvider.Clear();

        public bool ContainsHistory(string item)
        => _historyProvider.Contains(item);

        public override async Task UpdateSuggestions(string prefix, int quantityDesiredItems)
        {
            await UpdateProviders(prefix, quantityDesiredItems);
        }

        private void OnProvidersPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!_updatingProviders &&
                ReferenceEquals(e.PropertyName, nameof(_historyProvider.AutocompleteSuggestions)))
                UpdateSuggests();
        }

        private async Task UpdateProviders(string prefix, int quantityDesiredItems)
        {
            int amoutHistory = quantityDesiredItems / 3;
            int amountOnlineSuggestion = quantityDesiredItems - amoutHistory;

            _updatingProviders = true;
            await _historyProvider.UpdateSuggestions(prefix, amoutHistory);
            await _onlineSuggestionService.UpdateSuggestions(prefix, amountOnlineSuggestion);
            _updatingProviders = false;

            UpdateSuggests();
        }

        private void UpdateSuggests()
        {
            AutocompleteSuggestions = _historyProvider.AutocompleteSuggestions;
            ((List<IAutocompleteSuggestion>)AutocompleteSuggestions).AddRange(_onlineSuggestionService.AutocompleteSuggestions);
            StaticLogger.Logger.Info($"Search Autocomplete Service - update suggests");

            OnPropertyChanged(nameof(AutocompleteSuggestions));
        }

        public override async Task<bool> Load()
        {
            if (_historyProvider != null)
            {
                await _historyProvider.Load();
                return true;
            }
            return false;
        }

        public override async Task<bool> Save()
        {
            if (_historyProvider != null)
            {
                await _historyProvider.Save();
                return true;
            }
            return false;
        }
    }
}
