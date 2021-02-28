using COMP3000Project.Views.MainMenu;
using COMP3000Project.Views.Testing;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COMP3000Project
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            TestingPage testingPage = new TestingPage();
            MainPage = new NavigationPage(testingPage);
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
