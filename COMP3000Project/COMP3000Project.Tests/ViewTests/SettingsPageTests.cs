using System;
using NUnit.Framework;
using COMP3000Project.Views.Settings;
using Xamarin.Forms;

namespace COMP3000Project.Tests.ViewTests
{
    [TestFixture]
    class SettingsPageTests
    {
        SettingsPageViewModel VM;

        [SetUp]
        public void Setup()
        {
            VM = new SettingsPageViewModel();
        }

        [Test]
        public void validateUsername_0()
        {
            string username = "abc";
            var expected = true;
            var actual = VM.UsernameIsValid(username);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void validateUsername_1()
        {
            string username = "";
            var expected = false;
            var actual = VM.UsernameIsValid(username);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void validateUsername_2()
        {
            string username = null;
            var expected = false;
            var actual = VM.UsernameIsValid(username);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void validatePassword_0()
        {
            string password = "abc";
            var expected = true;
            var actual = VM.PasswordIsValid(password);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void validatePassword_1()
        {
            string password = "";
            var expected = false;
            var actual = VM.UsernameIsValid(password);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void validatePassword_2()
        {
            string password = null;
            var expected = false;
            var actual = VM.UsernameIsValid(password);
            Assert.AreEqual(expected, actual);
        }
    }
}
