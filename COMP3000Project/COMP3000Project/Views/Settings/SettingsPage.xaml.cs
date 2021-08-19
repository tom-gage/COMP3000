using COMP3000Project.WS;
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
        SettingsPageViewModel viewModel;
        public SettingsPage()
        {
            InitializeComponent();

            viewModel = new SettingsPageViewModel();

            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            //on appearing, hide feedback text
            viewModel.hideFeedbackText();

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