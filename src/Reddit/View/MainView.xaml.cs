using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Reddit.Model;
using Reddit.ViewModel;

namespace Reddit.View
{
    internal sealed partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        public MainViewModel ViewModel => DataContext as MainViewModel;

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel.SetScrollToItemAction(ScrollIntoView);
        }

        private void ScrollIntoView(RedditItem item)
        {
            ListView.SelectedItem = item;
            ListView.ScrollIntoView(item);
        }

        private async void OnItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (RedditItem) e.ClickedItem;
            await ViewModel.LaunchItemAsync(item);
        }
    }
}