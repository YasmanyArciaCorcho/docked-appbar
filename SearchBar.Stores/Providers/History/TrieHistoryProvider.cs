using Common;
using Common.DataStructures.Trie;
using Common.ExtensionMethods;
using Common.Logger;
using Common.Suggestion;
using Newtonsoft.Json;
using Stores.Providers.Autocomplete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Stores.Providers.History
{
    public class TrieHistoryProvider : AutoCompleteProviderBase, IHistoryProvider
    {
        private int _wordsCounter;
        private Trie<char, int> _trieStore;

        private string _historyPath;

        public TrieHistoryProvider()
        {
            _wordsCounter = 0;
        }

        public void Add(string item)
        {

            if (_wordsCounter == 50)
            {
                Clear();
            }

            if (!Contains(item))
            {
                _trieStore.Add(item, _wordsCounter);
                StaticLogger.Logger.Info($"Trie history - Added history: {item}.");
                _wordsCounter++;
                //Todo:g Improve the wait to store and clean history.
                Save();
            }
        }

        public void Clear()
        {
            _trieStore.Clear();
            Common.Logger.StaticLogger.Logger.Info("Trie history provider cleaned.");
        }

        public bool Contains(string item)
            => _trieStore.ContainsKey(item);

        public override async System.Threading.Tasks.Task<bool> Load()
        {
            _trieStore = new Trie<char, int>();
            _historyPath = DirectoryInfoHelper.GetHistoryStorePath();

            try
            {
                if (File.Exists(_historyPath))
                {
                    await Task.Yield();

                    List<string> toLoad = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(_historyPath));
                    foreach (var word in toLoad)
                        Add(word);
                    StaticLogger.Logger.Info($"Trie history - read data from disk.");
                }
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
                return false;
            }

            return true;
        }

        public override Task<bool> Save()
        {
            return Task.Run(() =>
            {
                lock (_saveLock)
                {
                    try
                    {
                        if (_trieStore != null && _trieStore.Count != 0)
                        {
                            List<string> toSerialize = new List<string>();
                            foreach (var keyValue in _trieStore.GetByPrefix(""))
                            {
                                string toAdd = keyValue.Key.CharEnumerableToString();
                                toSerialize.Add(toAdd);
                            }

                            string serializedBookmars = JsonConvert.SerializeObject(toSerialize);
                            File.WriteAllText(_historyPath, serializedBookmars);
                            StaticLogger.Logger.Info($"Trie history - saved on disk.");
                        }
                    }
                    catch (Exception e)
                    {
                        StaticLogger.Logger.Error(e);
                        return false;
                    }
                    return true;
                }
            });
        }

        public async override Task UpdateSuggestions(string query, int quantityDesiredItems)
        {
            var queryResult = _trieStore.GetByPrefix(query);
            SortedList<int, IAutocompleteSuggestion> result = new SortedList<int, IAutocompleteSuggestion>
            {
                { int.MaxValue, new AutoCompleteSuggestion() { Value = query } }
            };

            int takenValues = 0;
            foreach (var keyValue in queryResult)
            {
                await Task.Yield();

                string toAdd = keyValue.Key.CharEnumerableToString();
                result.Add(keyValue.Value, new AutoCompleteSuggestion() { Value = toAdd });

                takenValues++;
                if (takenValues == quantityDesiredItems)
                    break;
            }

            AutocompleteSuggestions = result.Values.Reverse().ToList();

            StaticLogger.Logger.Info($"Trie history - updated suggestions.");

            OnPropertyChanged(nameof(AutocompleteSuggestions));
        }
    }
}
