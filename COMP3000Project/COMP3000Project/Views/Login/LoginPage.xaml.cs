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
using COMP3000Project.WS;

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



            //get user details from local storage
            LocalDataHandler.SetUserDetailsFromLocalStorage();

            if (UserDetails.IsFirstStartUp)//if this is first start up, prompt user with text accessibility page
            {
                Navigation.PushAsync(new AccessibilitySettingsPage());

                //no longer the first start up, set flag to false
                UserDetails.IsFirstStartUp = false;

                UserDetails.SearchPageTutorialShown = false;


                LocalDataHandler.SaveUserDetails();
            }



        }

        protected override void OnAppearing()
        {
            //on appearing
            //set text size
            viewModel.VeryLarge = UserDetails.GetVeryLargeTextSetting();
            viewModel.Large = UserDetails.GetlargeTextSetting();
            viewModel.Medium = UserDetails.GetMediumTextSetting();
            viewModel.Small = UserDetails.GetSmallTextSetting();

            //set username/password
            viewModel.Username = UserDetails.Username;
            viewModel.Password = UserDetails.Password;

            //hide feedback text
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