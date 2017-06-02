using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Newtonsoft.Json;
using Prism.Windows.Mvvm;
using Reddit.Model;
using Reddit.Service.Contracts;
using Reddit.Utilities;

namespace Reddit.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly ILogger _logger;
        private readonly IRedditService _service;
        private bool _isBusy;
        private MainViewModelState _restorableState;
        private Action<RedditItem> _scrollIntoViewAction;

        public MainViewModel(IRedditService service, ILogger logger)
        {
            _service = service;
            _logger = logger;
            ItemSource = new LazyItemSource<RedditItem>(Loader, HasItems);

            Application.Current.Suspending += OnSuspending;
            OnResuming();
        }

        public ObservableCollection<RedditItem> ItemSource { get; set; }

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

            List<RedditItem> result;

            if (_restorableState != null)
            {
                var count = _restorableState.ItemsCount;
                var lastId = _restorableState.LastId;
                _restorableState = null;

                result = await _service.GetPage(count, lastId);
                SetItemSelected(result.LastOrDefault());
            }
            else
            {
                var last = ItemSource.LastOrDefault();
                result = await _service.GetPage(last?.Id);
            }

            IsBusy = false;

            return result;
        }

        private void SetItemSelected(RedditItem item)
        {
            void Handler(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (!ItemSource.Contains(item))
                    return;

                ItemSource.CollectionChanged -= Handler;
                _scrollIntoViewAction(item);
            }

            ItemSource.CollectionChanged += Handler;
        }

        public async Task LaunchItemAsync(RedditItem item)
        {
            var source = item.Preview?.Images?.FirstOrDefault()?.Source?.Url ?? item.Url;
            await Launcher.LaunchUriAsync(new Uri(source));
        }

        public void SetScrollToItemAction(Action<RedditItem> action)
        {
            _scrollIntoViewAction = action;
        }

        #region State Store/Restore

        private void OnResuming()
        {
            try
            {
                var settings = ApplicationData.Current.LocalSettings;
                var serialized = settings.Values[nameof(MainViewModel)];
                if (serialized != null)
                {
                    var state = JsonConvert.DeserializeObject<MainViewModelState>(serialized.ToString());
                    if (state != null)
                    {
                        _restorableState = state;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            try
            {
                var settings = ApplicationData.Current.LocalSettings;

                var last = ItemSource.LastOrDefault();
                var state = new MainViewModelState
                {
                    ItemsCount = ItemSource.Count(),
                    LastId = last?.Id
                };

                settings.Values[nameof(MainViewModel)] = JsonConvert.SerializeObject(state);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }

            deferral.Complete();
        }

        #endregion
    }
}