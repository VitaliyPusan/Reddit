using System;
using System.Reflection;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using Reddit.Service;
using Reddit.Service.Contracts;
using Reddit.View;

namespace Reddit
{
    public sealed partial class App
    {
        public App()
        {
            InitializeComponent();

            ConfigureContainer();
            ConfigureViewModelLocator();
        }

        public static IUnityContainer Container { get; } = new UnityContainer();

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                    rootFrame.Navigate(typeof(MainView), e.Arguments);
                Window.Current.Activate();
            }
        }

        private static void ConfigureContainer()
        {
            Container.RegisterType<IRedditService, RedditService>(new ContainerControlledLifetimeManager());
        }

        private static void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory(type => Container.Resolve(type));
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
            {
                var viewName = viewType.FullName;
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;

                var viewModel = viewName.Replace(".View.", ".ViewModel.");
                var viewModelName = $"{viewModel}Model, {viewAssemblyName}";

                return Type.GetType(viewModelName);
            });
        }
    }
}