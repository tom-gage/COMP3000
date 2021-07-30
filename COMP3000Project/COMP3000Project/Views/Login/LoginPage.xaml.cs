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




            viewModel = new LoginPageViewModel();

            BindingContext = viewModel;

            LocalDataHandler.SetUserDetailsFromLocalStorage();

            if (UserDetails.IsFirstStartUp)
            {
                Navigation.PushAsync(new AccessibilitySettingsPage());
                UserDetails.IsFirstStartUp = false;
                LocalDataHandler.SaveUserDetails();
            }



        }

        protected override void OnAppearing()
        {
            viewModel.VeryLarge = UserDetails.GetVeryLargeTextSetting();
            viewModel.Large = UserDetails.GetlargeTextSetting();
            viewModel.Medium = UserDetails.GetMediumTextSetting();
            viewModel.Small = UserDetails.GetSmallTextSetting();

            viewModel.hideLoginFeedbackText();
            base.OnAppearing();
        }

    }
}