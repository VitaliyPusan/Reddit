using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace Reddit.Utilities
{
    public class LazyItemSource<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        private readonly Func<bool> _hasItems;
        private readonly Func<int, Task<List<T>>> _loader;

        public LazyItemSource(Func<int, Task<List<T>>> loader, Func<bool> hasItems)
        {
            _loader = loader;
            _hasItems = hasItems;
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return TaskProvider((int) count).AsAsyncOperation();
        }

        public bool HasMoreItems => _hasItems();

        private async Task<LoadMoreItemsResult> TaskProvider(int count)
        {
            var result = new LoadMoreItemsResult();

            var items = await _loader(count);
            result.Count = (uint) items.Count;

            foreach (var item in items)
            {
                Add(item);
            }

            return result;
        }
    }
}