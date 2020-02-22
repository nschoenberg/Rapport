using AutoMapper;
using Prism;
using Prism.Ioc;
using Rapport.Contracts;
using Rapport.Pexels;
using Rapport.Services;
using Rapport.Sql;
using Rapport.ViewModels;
using Rapport.Views;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
[assembly: ExportFont("Font Awesome 5 Free-Regular-400")]
[assembly: ExportFont("material.ttf")]
[assembly: ExportFont("Raleway-Black.ttf")]
[assembly: ExportFont("Raleway-Regular.ttf")]
namespace Rapport
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/" + Pages.Login);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry.RegisterSingleton<IDeviceDisplay, DeviceDisplayImplementation>();
            containerRegistry.RegisterSingleton<IPexelsRestClient, PexelsRestClient>();
            containerRegistry.RegisterSingleton<IImageService, ImageService>();
            containerRegistry.RegisterInstance(MappingDefinition.GetConfigurationProvider());
            containerRegistry.RegisterSingleton<IJiraService, JiraService>();
            containerRegistry.RegisterSingleton<IFileSystem, FileSystemImplementation>();
            containerRegistry.RegisterSingleton<IPreferences, PreferencesImplementation>();
            containerRegistry.RegisterSingleton<ISqlRepository, SqlRepository>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<BoardSelectPage, BoardPageViewModel>(Pages.BoardSelect);
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>(Pages.Login);
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>(Pages.Home);
            containerRegistry.RegisterForNavigation<OverviewPage, OverviewPageViewModel>(Pages.Overview);
            containerRegistry.RegisterForNavigation<IssueSelectPage, IssueSelectPageViewModel>(Pages.IssueSelect);
        }
    }
}
