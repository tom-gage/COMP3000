using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using COMP3000App.Views.Testing;

namespace COMP3000App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new TestingPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
