using COMP3000Project.TestObjects;
using COMP3000Project.WS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COMP3000Project.Views.MatchDetails
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MatchDetailsPage : ContentPage
    {
        MatchDetailsPageViewModel viewModel;
        public MatchDetailsPage(EateryOption eateryOption)
        {
            InitializeComponent();

            viewModel = new MatchDetailsPageViewModel(eateryOption);

            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            viewModel.GetCoordinates();

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