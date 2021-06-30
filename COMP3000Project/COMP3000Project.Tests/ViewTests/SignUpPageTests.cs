using System;
using NUnit.Framework;
using COMP3000Project.Views.SignUp;
using Xamarin.Forms;

namespace COMP3000Project.Tests.ViewTests
{
    [TestFixture]
    class SignUpPageTests
    {
        SignUpPageViewModel VM;

        [SetUp]
        public void Setup()
        {
            VM = new SignUpPageViewModel();
        }


        [Test]
        public void validateUsernameAndPassword_0()
        {
            string username = "abc";
            string password = "123";
            var expected = true;
            var actual = VM.UandPAreValid(username, password);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void validateUsernameAndPassword_1()
        {
            string username = "";
            string password = "";
            var expected = false;
            var actual = VM.UandPAreValid(username, password);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void validateUsernameAndPassword_2()
        {
            string username = "adsasdasdas";
            string password = "";
            var expected = false;
            var actual = VM.UandPAreValid(username, password);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void validateUsernameAndPassword_3()
        {
            string username = "";
            string password = "kidsamlkadalds";
            var expected = false;
            var actual = VM.UandPAreValid(username, password);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void validateUsernameAndPassword_4()
        {
            string username = null;
            string password = null;
            var expected = false;
            var actual = VM.UandPAreValid(username, password);
            Assert.AreEqual(expected, actual);
        }
    }
}
