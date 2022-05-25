using Common;
using Stores.Providers.Autocomplete;

namespace Stores.Providers.History
{
   public interface IHistoryProvider: IProvider, IAutoCompleteProvider
    {
        void Add(string item);

        bool Contains(string item);

        void Clear();
    }
}
