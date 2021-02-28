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
    public partial class TestingPage2 : ContentPage
    {
        TestingPage2ViewModel viewModel;
        public TestingPage2()
        {
            InitializeComponent();
            viewModel = new TestingPage2ViewModel();

            BindingContext = viewModel;
        }
    }
}