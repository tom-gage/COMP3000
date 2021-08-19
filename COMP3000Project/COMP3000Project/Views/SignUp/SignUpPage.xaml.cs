using COMP3000Project.WS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COMP3000Project.Views.SignUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        SignUpPageViewModel viewModel;
        public SignUpPage()
        {
            InitializeComponent();

            viewModel = new SignUpPageViewModel();

            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            viewModel.hideLoginFeedbackText();

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