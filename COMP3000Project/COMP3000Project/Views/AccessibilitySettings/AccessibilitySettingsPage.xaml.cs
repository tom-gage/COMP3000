using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COMP3000Project.Views.AccessibilitySettings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccessibilitySettingsPage : ContentPage
    {
        AccessibilitySettingsPageViewModel vm;
        public AccessibilitySettingsPage()
        {
            InitializeComponent();

            vm = new AccessibilitySettingsPageViewModel();

            BindingContext = vm;
        }
    }
}