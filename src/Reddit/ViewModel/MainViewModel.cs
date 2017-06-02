using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using Prism.Windows.Mvvm;
using Reddit.Model;
using Reddit.Service.Contracts;
using Reddit.Utilities;

namespace Reddit.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly IRedditService _service;
        private bool _isBusy;

        public MainViewModel(IRedditService service)
        {
            _service = service;
            ItemSource = new LazyItemSource<RedditItem>(Loader, HasItems);
        }

        public IEnumerable<RedditItem> ItemSource { get; set; }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private bool HasItems()
        {
            return true;
        }

        private async Task<List<RedditItem>> Loader(int i)
        {
            IsBusy = true;

            var last = ItemSource.LastOrDefault();
            var result = await _service.GetPage(last?.Id);

            IsBusy = false;

            return result;
        }

        public async Task LaunchItemAsync(RedditItem item)
        {
            var source = item.Preview?.Images?.FirstOrDefault()?.Source?.Url ?? item.Url;
            await Launcher.LaunchUriAsync(new Uri(source));
        }
    }
}