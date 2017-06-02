using System;
using System.Reflection;
using System.Threading.Tasks;
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

            UnhandledException += CurrentOnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnTaskSchedulerUnobservedException;
        }

        public static IUnityContainer Container { get; } = new UnityContainer();
        public static ILogger Logger => Container.Resolve<ILogger>();

        private void CurrentOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            HandleException(e.Exception);
        }

        private void OnTaskSchedulerUnobservedException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            HandleException(e.Exception);
        }

        public static void HandleException(Exception exception)
        {
            Logger.Log(exception.ToString());
        }

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
            Container.RegisterType<ILogger, Logger>(new ContainerControlledLifetimeManager());
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