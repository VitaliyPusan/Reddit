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
        }

        public MainViewModel ViewModel => DataContext as MainViewModel;

        private async void OnItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (RedditItem) e.ClickedItem;
            await ViewModel.LaunchItemAsync(item);
        }
    }
}