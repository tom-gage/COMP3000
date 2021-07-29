using COMP3000Project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using COMP3000Project.UserDetailsSingleton;
using COMP3000Project.Views.AccessibilitySettings;
using COMP3000Project.LDH;

namespace COMP3000Project.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginPageViewModel viewModel;
        public LoginPage()
        {
            InitializeComponent();

            LocalDataHandler.SetUserDetailsFromLocalStorage();


            viewModel = new LoginPageViewModel();

            BindingContext = viewModel;



            if (true)//UserDetails.IsFirstStartUp)
            {
                Navigation.PushAsync(new AccessibilitySettingsPage());
                UserDetails.IsFirstStartUp = false;
                LocalDataHandler.SaveUserDetails();
            }


        }

        protected override void OnAppearing()
        {


            viewModel.hideLoginFeedbackText();
            base.OnAppearing();
        }

    }
}