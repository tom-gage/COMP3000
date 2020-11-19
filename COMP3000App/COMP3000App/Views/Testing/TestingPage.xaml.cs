using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COMP3000App.Views.Testing
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestingPage : ContentPage
    {

        TestingPageViewModel ViewModel;
        public TestingPage()
        {
            InitializeComponent();

            ViewModel = new TestingPageViewModel();

            BindingContext = ViewModel;
        }
    }
}