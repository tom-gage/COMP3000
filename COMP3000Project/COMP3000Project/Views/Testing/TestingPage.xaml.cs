using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COMP3000Project.Views.Testing
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestingPage : ContentPage
    {
        TestingPageViewModel viewModel;


        public TestingPage()
        {
            InitializeComponent();

            viewModel = new TestingPageViewModel();

            BindingContext = viewModel;
        }


    }
}