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
            //set prefered text size
            viewModel.VeryLarge = UserDetails.GetVeryLargeTextSetting();
            viewModel.Large = UserDetails.GetlargeTextSetting();
            viewModel.Medium = UserDetails.GetMediumTextSetting();
            viewModel.Small = UserDetails.GetSmallTextSetting();

            //refresh past searches...
            WebsocketHandler.RequestGetPastSearches(UserDetails.Username, UserDetails.Password);

            //register subscriber with publisher
            WebsocketHandler.registerSubscriber(viewModel);

            base.OnAppearing();


        }

        protected override void OnDisappearing()
        {
            //remove subscriber from publisher
            WebsocketHandler.removeSubsciber(viewModel);

            base.OnDisappearing();
        }
    }
}