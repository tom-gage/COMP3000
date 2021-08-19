using COMP3000Project.LDH;
using COMP3000Project.UserDetailsSingleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COMP3000Project.Views.Tutorial
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TutorialPage : ContentPage
    {
        TutorialPageViewModel viewModel;
        public TutorialPage()
        {
            InitializeComponent();

            viewModel = new TutorialPageViewModel();
            BindingContext = viewModel;
            
        }

        protected override void OnAppearing()
        {
            UserDetails.SearchPageTutorialShown = true;
            LocalDataHandler.SaveUserDetails();

            base.OnAppearing();
        }
    }
}