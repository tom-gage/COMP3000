using COMP3000Project.TestObjects;
using COMP3000Project.WS;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COMP3000Project.Views.FavouriteEateryDetails
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavouriteEateryDetailsPage : ContentPage
    {
        FavouriteEateryDetailsViewModel viewModel;
        public FavouriteEateryDetailsPage(EateryOption eateryOption)
        {
            InitializeComponent();

            viewModel = new FavouriteEateryDetailsViewModel(eateryOption);

            BindingContext = viewModel;
        }


        protected override void OnAppearing()
        {
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