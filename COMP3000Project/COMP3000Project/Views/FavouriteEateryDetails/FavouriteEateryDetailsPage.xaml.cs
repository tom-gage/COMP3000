using COMP3000Project.TestObjects;


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
    }
}