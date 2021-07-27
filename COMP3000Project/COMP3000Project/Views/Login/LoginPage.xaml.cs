using COMP3000Project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COMP3000Project.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginPageViewModel viewModel;
        public LoginPage()
        {
            InitializeComponent();

            viewModel = new LoginPageViewModel();

            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {


            viewModel.hideLoginFeedbackText();
            base.OnAppearing();
        }

    }
}