using Xamarin.Forms;

namespace sub100DemoApp.Views
{
    public partial class DetailsPage : ContentPage
    {
        public DetailsPage()
        {
            InitializeComponent();

            lstCaracteristicas.ItemSelected += (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
            };

            lstCaracteristicasComuns.ItemSelected += (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
            };
        }
    }
}