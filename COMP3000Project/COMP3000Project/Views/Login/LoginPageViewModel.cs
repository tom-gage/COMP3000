using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using COMP3000Project.ViewModel;
using COMP3000Project.WS;

namespace COMP3000Project.Views.Login
{
    class LoginPageViewModel : ViewModelBase
    {
        //VARIABLES
        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                if(_username != value)
                {
                    SetProperty(ref _username, value);//informs view of change
                }
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    SetProperty(ref _password, value);//informs view of change
                }
            }
        }

        //COMMANDS
        public ICommand RegisterNewUser { get; }

        //CONSTRUCTOR
        public LoginPageViewModel()
        {
            Username = "martin";
            Password = "slime man";

            RegisterNewUser = new Command(async () => await ExecuteRegisterUser());
        }

        //FUNCTIONS
        async Task<object> ExecuteRegisterUser()
        {
            WebsocketHandler.RequestRegisterNewUser(Username, Password);

            return null;
        }

    }
}
