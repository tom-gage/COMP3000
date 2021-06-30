using System;
using NUnit.Framework;
using COMP3000Project.Views.MainMenu;
using Xamarin.Forms;

namespace COMP3000Project.Tests.ViewTests
{
    [TestFixture]
    class MainMenuPageTests
    {
        MainMenuPageViewModel mainMenuPageVM;

        [SetUp]
        public void Setup()
        {
            mainMenuPageVM = new MainMenuPageViewModel();
        }


        [Test]
        public void validateSearchCode_0()
        {
            string searchCode = "abc";
            var expected = true;
            var actual = mainMenuPageVM.ValidateSearchCode(searchCode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void validateSearchCode_1()
        {
            string searchCode = "";
            var expected = false;
            var actual = mainMenuPageVM.ValidateSearchCode(searchCode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void validateSearchCode_2()
        {
            string searchCode = null;
            var expected = false;
            var actual = mainMenuPageVM.ValidateSearchCode(searchCode);
            Assert.AreEqual(expected, actual);
        }
    }
}
