using COMP3000Project.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COMP3000Project.Views.EateryOptionDetails
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EateryOptionDetailsPage : ContentPage
    {
        EateryOptionDetailsPageViewModel viewModel;
        public EateryOptionDetailsPage(EateryOption eateryOption)
        {
            InitializeComponent();

            viewModel = new EateryOptionDetailsPageViewModel(eateryOption);

            BindingContext = viewModel;

        }
    }
}