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
            base.OnAppearing();

            WebsocketHandler.RequestGetFavourites(UserDetails.Username, UserDetails.Password);



        }
    }
}