using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using COMP3000Project.UserDetailsSingleton;
using COMP3000Project.ViewModel;
using COMP3000Project.LDH;

namespace COMP3000Project.Views.AccessibilitySettings
{
    class AccessibilitySettingsPageViewModel : ViewModelBase
    {
        //VARS

        //commands
        public ICommand SetVeryLargeText { get; }
        public ICommand SetLargeText { get; }
        public ICommand SetMediumText { get; }
        public ICommand Done { get; }


        //CONSTRUCTOR
        public AccessibilitySettingsPageViewModel()
        {
            //set commands
            SetVeryLargeText = new Command(() => ExecuteSetVeryLargeText());
            SetLargeText = new Command(() => ExecuteSetLargeText());
            SetMediumText = new Command(() => ExecuteSetMediumText());
            Done = new Command(() => ExecuteDone());

            UserDetails.PreferVeryLargeText = false;
            UserDetails.PreferLargeText = false;
            UserDetails.PreferMediumText = true;

            //set text size
            VeryLarge = UserDetails.GetVeryLargeTextSetting();
            Large = UserDetails.GetlargeTextSetting();
            Medium = UserDetails.GetMediumTextSetting();
            Small = UserDetails.GetSmallTextSetting();
        }

        void ExecuteSetVeryLargeText()
        {
            UserDetails.PreferVeryLargeText = true;
            UserDetails.PreferLargeText = false;
            UserDetails.PreferMediumText = false;

            LocalDataHandler.SaveUserDetails();

            VeryLarge = UserDetails.GetVeryLargeTextSetting();
            Large = UserDetails.GetlargeTextSetting();
            Medium = UserDetails.GetMediumTextSetting();
            Small = UserDetails.GetSmallTextSetting();
        }

        void ExecuteSetLargeText()
        {
            UserDetails.PreferVeryLargeText = false;
            UserDetails.PreferLargeText = true;
            UserDetails.PreferMediumText = false;

            LocalDataHandler.SaveUserDetails();

            VeryLarge = UserDetails.GetVeryLargeTextSetting();
            Large = UserDetails.GetlargeTextSetting();
            Medium = UserDetails.GetMediumTextSetting();
            Small = UserDetails.GetSmallTextSetting();
        }

        void ExecuteSetMediumText()
        {
            UserDetails.PreferVeryLargeText = false;
            UserDetails.PreferLargeText = false;
            UserDetails.PreferMediumText = true;

            LocalDataHandler.SaveUserDetails();

            VeryLarge = UserDetails.GetVeryLargeTextSetting();
            Large = UserDetails.GetlargeTextSetting();
            Medium = UserDetails.GetMediumTextSetting();
            Small = UserDetails.GetSmallTextSetting();
        }

        void ExecuteDone()
        {
            Navigation.PopAsync();
        }

    }
}
