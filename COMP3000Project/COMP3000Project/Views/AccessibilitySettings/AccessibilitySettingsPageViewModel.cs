using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using COMP3000Project.UserDetailsSingleton;
using COMP3000Project.ViewModel;

namespace COMP3000Project.Views.AccessibilitySettings
{
    class AccessibilitySettingsPageViewModel : ViewModelBase
    {
        //VARS
        public ICommand SetVeryLargeText { get; }
        public ICommand SetLargeText { get; }
        public ICommand SetMediumText { get; }

        //CONSTRUCTOR
        public AccessibilitySettingsPageViewModel()
        {
            //set commands
            SetVeryLargeText = new Command(() => ExecuteSetVeryLargeText());
            SetLargeText = new Command(() => ExecuteSetLargeText());
            SetMediumText = new Command(() => ExecuteSetMediumText());

            //set text size
            VeryLargeText = UserDetails.GetVeryLargeTextSetting();
            LargeText = UserDetails.GetlargeTextSetting();
            MediumText = UserDetails.GetMediumTextSetting();
        }

        void ExecuteSetVeryLargeText()
        {
            UserDetails.PreferVeryLargeText = true;
            UserDetails.PreferMediumText = false;
            UserDetails.PreferMediumText = false;

            VeryLargeText = UserDetails.GetVeryLargeTextSetting();
            LargeText = UserDetails.GetlargeTextSetting();
            MediumText = UserDetails.GetMediumTextSetting();
        }

        void ExecuteSetLargeText()
        {
            UserDetails.PreferVeryLargeText = false;
            UserDetails.PreferMediumText = true;
            UserDetails.PreferMediumText = false;

            VeryLargeText = UserDetails.GetVeryLargeTextSetting();
            LargeText = UserDetails.GetlargeTextSetting();
            MediumText = UserDetails.GetMediumTextSetting();
        }

        void ExecuteSetMediumText()
        {
            UserDetails.PreferVeryLargeText = false;
            UserDetails.PreferMediumText = false;
            UserDetails.PreferMediumText = true;

            VeryLargeText = UserDetails.GetVeryLargeTextSetting();
            LargeText = UserDetails.GetlargeTextSetting();
            MediumText = UserDetails.GetMediumTextSetting();
        }
    }
}
