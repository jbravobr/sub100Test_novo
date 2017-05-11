using Prism.Unity;
using SQLite;
using System.Net.Http;
using Microsoft.Practices.Unity;
using sub100DemoApp.Views;
using DLToolkit.Forms.Controls;

namespace sub100DemoApp
{
	public class App : PrismApplication
	{
        public static SQLiteConnection AppConnection { get; set; }
        public static HttpClient AppBaseHttpClient { get; set; }

        protected override void OnInitialized()
        {
            try
            {
                FlowListView.Init();
                NavigationService.NavigateAsync("BaseNavigationPage/MainPage");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        protected override void RegisterTypes()
        {
			// Register Classe for IoC
			Container.RegisterType(typeof(IApplicationService<>), typeof(ApplicationService<>));
			Container.RegisterType(typeof(IRootObjectService<>), typeof(RootApplicationService<>));
			Container.RegisterType(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
			Container.RegisterType(typeof(IDialogsFunctions), typeof(DialogsFunctions));
			Container.RegisterType(typeof(IConnectivityFunctions), typeof(ConnectivityFunctions));

			// Register 3rd Party Classes for IoC
			Container.RegisterInstance(Acr.UserDialogs.UserDialogs.Instance);
			Container.RegisterInstance(Plugin.Connectivity.CrossConnectivity.Current);

			// Register pages for Navigation (PRISM)
			Container.RegisterTypeForNavigation<MainPage>();
			Container.RegisterTypeForNavigation<BaseNavigationPage>();
			Container.RegisterTypeForNavigation<BlankPage>();
			Container.RegisterTypeForNavigation<DetailsPage>();
			Container.RegisterTypeForNavigation<ImageGalleryPreviewPage>();
		}
    }
}