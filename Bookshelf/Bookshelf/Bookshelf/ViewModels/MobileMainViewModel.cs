using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookshelf.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bookshelf.ViewModels
{
    public partial class MobileMainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private NavigationService _navigation;

        public MobileMainViewModel(NavigationService nav)
        {
            Navigation = nav;
            Navigation.NavigateTo<LoginViewModel>();
        }
    }
}
