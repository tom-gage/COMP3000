using COMP3000Project.UserDetailsSingleton;
using COMP3000Project.WS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COMP3000Project.Views.FavouriteEateries
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavouriteEateriesPage : ContentPage
    {
        FavouriteEateriesViewModel viewModel;
        public FavouriteEateriesPage()
        {
            InitializeComponent();

            viewModel = new FavouriteEateriesViewModel();

            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            //on page appearing
            //hide feedback text
            viewModel.hideFeedbackText();
            //refresh favourites list
            WebsocketHandler.RequestGetFavourites(UserDetails.Username, UserDetails.Password);

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