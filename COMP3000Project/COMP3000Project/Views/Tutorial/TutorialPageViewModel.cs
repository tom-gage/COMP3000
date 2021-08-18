using COMP3000Project.UserDetailsSingleton;
using COMP3000Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace COMP3000Project.Views.Tutorial
{
    class TutorialPageViewModel : ViewModelBase
    {
        public ICommand Done { get; set; }

        public TutorialPageViewModel()
        {
            Done = new Command(async () => await done());

            VeryLarge = UserDetails.GetVeryLargeTextSetting();
            Large = UserDetails.GetlargeTextSetting();
            Medium = UserDetails.GetMediumTextSetting();
            Small = UserDetails.GetSmallTextSetting();
        }

        public async Task<object> done()
        {
            await Navigation.PopAsync();
            return null;
        }
    }
}
