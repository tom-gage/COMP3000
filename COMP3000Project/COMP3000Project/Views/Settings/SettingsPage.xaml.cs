using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;



namespace COMP3000Project.Views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        SettingsPageViewModel vm;
        public SettingsPage()
        {
            InitializeComponent();

            vm = new SettingsPageViewModel();

            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            //on appearing, hide feedback text
            vm.hideFeedbackText();
            base.OnAppearing();
        }
    }
}