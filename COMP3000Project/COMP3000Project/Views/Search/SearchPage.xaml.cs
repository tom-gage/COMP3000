using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COMP3000Project.Views.Search
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        SearchPageViewModel viewModel;
        public SearchPage()
        {
            InitializeComponent();

            viewModel = new SearchPageViewModel();
            BindingContext = viewModel;
        }
    }
}