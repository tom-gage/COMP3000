using COMP3000Project.WS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COMP3000Project.Views.JoinSearchParameters
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JoinSearchParametersPage : ContentPage
    {
        JoinSearchParametersPageViewModel viewModel;
        public JoinSearchParametersPage()
        {
            InitializeComponent();

            viewModel = new JoinSearchParametersPageViewModel();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            //on page appear, hide feedback text
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