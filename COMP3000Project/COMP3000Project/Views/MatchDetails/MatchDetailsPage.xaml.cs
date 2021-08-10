using COMP3000Project.TestObjects;
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

            base.OnAppearing();
        }
    }
}