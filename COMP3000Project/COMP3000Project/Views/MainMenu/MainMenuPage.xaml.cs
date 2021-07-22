using COMP3000Project.UserDetailsSingleton;
using COMP3000Project.WS;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COMP3000Project.Views.MainMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuPage : ContentPage
    {
        MainMenuPageViewModel viewModel;
        public MainMenuPage()
        {
            InitializeComponent();

            viewModel = new MainMenuPageViewModel();

            BindingContext = viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            WebsocketHandler.RequestGetPastSearches(UserDetails.Username, UserDetails.Password);
        }
    }
}