using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COMP3000Project.Views.SearchParameters
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchParametersPage : ContentPage
    {
        SearchParametersPageViewModel viewModel;

        //constructor 1, for starting a new search
        public SearchParametersPage()
        {
            InitializeComponent();

            viewModel = new SearchParametersPageViewModel();
            BindingContext = viewModel;
        }

        //constructor 2, for starting a past search
        public SearchParametersPage(string location, string time, string[] eateryTypes)
        {
            InitializeComponent();

            viewModel = new SearchParametersPageViewModel(location, time, eateryTypes);
            BindingContext = viewModel;
        }
    }
}