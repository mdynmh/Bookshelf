using System;
using System.Collections.Generic;
using Bookshelf.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace Bookshelf.Services
{
    public class NavigationService : ObservableObject
    {
        private readonly IServiceProvider _sp;
        private readonly Stack<ViewModelBase> _history = new();

        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            private set
            {
                SetProperty(ref _currentViewModel, value);
                OnPropertyChanged(nameof(CanGoBack));
            }
        }

        public bool CanGoBack => _history.Count > 0;

        public NavigationService(IServiceProvider sp) => _sp = sp;

        public void NavigateTo<TVm>(object? parameter = null)
        where TVm : ViewModelBase
        {
            if (CurrentViewModel is not null)
                _history.Push(CurrentViewModel);

            var vm = _sp.GetRequiredService<TVm>();

            if (vm is INavigationAware aware)
                aware.OnNavigatedTo(parameter);

            CurrentViewModel = vm;
        }

        public void GoBack()
        {
            if (_history.Count > 0)
                CurrentViewModel = _history.Pop();
        }

        public void NavigationStackClear()
        {
            _history.Clear();
            OnPropertyChanged(nameof(CanGoBack));
        }
    }
}
