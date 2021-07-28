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
            base.OnAppearing();
        }
    }
}