using Bookshelf.Dtos;
using Bookshelf.Services;
using Bookshelf.ViewModels;
using Moq;

namespace TestBookshelf
{
    public class NavigationServiceTests
    {
        public class DummyViewModel : ViewModelBase { }

        public class DummyAwareViewModel : ViewModelBase, INavigationAware
        {
            public object? LastParameter { get; private set; }

            public void OnNavigatedTo(object? parameter)
            {
                LastParameter = parameter;
            }
        }

        [Fact]
        public void NavigateTo_SetsCurrentViewModel()
        {
            var dummyVm = new DummyViewModel();
            var sp = new Mock<IServiceProvider>();
            sp.Setup(x => x.GetService(typeof(DummyViewModel))).Returns(dummyVm);

            var nav = new NavigationService(sp.Object);

            nav.NavigateTo<DummyViewModel>();

            Assert.Equal(dummyVm, nav.CurrentViewModel);
            Assert.False(nav.CanGoBack);
        }

        [Fact]
        public void NavigateTo_AddsToHistory()
        {
            var vm1 = new DummyViewModel();
            var vm2 = new DummyViewModel();
            var sp = new Mock<IServiceProvider>();
            sp.Setup(x => x.GetService(typeof(DummyViewModel))).Returns(vm1);

            var nav = new NavigationService(sp.Object);

            nav.NavigateTo<DummyViewModel>();
            sp.Setup(x => x.GetService(typeof(DummyViewModel))).Returns(vm2);
            nav.NavigateTo<DummyViewModel>();

            Assert.Equal(vm2, nav.CurrentViewModel);
            Assert.True(nav.CanGoBack);
        }

        [Fact]
        public void GoBack_ReturnsToPreviousViewModel()
        {
            var vm1 = new DummyViewModel();
            var vm2 = new DummyViewModel();
            var sp = new Mock<IServiceProvider>();
            sp.SetupSequence(x => x.GetService(typeof(DummyViewModel)))
              .Returns(vm1)
              .Returns(vm2);

            var nav = new NavigationService(sp.Object);

            nav.NavigateTo<DummyViewModel>();
            nav.NavigateTo<DummyViewModel>();
            nav.GoBack();

            Assert.Equal(vm1, nav.CurrentViewModel);
        }

        [Fact]
        public void NavigateTo_CallsOnNavigatedTo_WhenAware()
        {
            var awareVm = new DummyAwareViewModel();
            var sp = new Mock<IServiceProvider>();
            sp.Setup(x => x.GetService(typeof(DummyAwareViewModel))).Returns(awareVm);

            var nav = new NavigationService(sp.Object);
            var parameter = "Hello";

            nav.NavigateTo<DummyAwareViewModel>(parameter);

            Assert.Equal(parameter, awareVm.LastParameter);
        }

        [Fact]
        public void NavigationStackClear_ResetsBackState()
        {
            var vm1 = new DummyViewModel();
            var vm2 = new DummyViewModel();
            var sp = new Mock<IServiceProvider>();
            sp.SetupSequence(x => x.GetService(typeof(DummyViewModel)))
              .Returns(vm1)
              .Returns(vm2);

            var nav = new NavigationService(sp.Object);
            nav.NavigateTo<DummyViewModel>();
            nav.NavigateTo<DummyViewModel>();

            nav.NavigationStackClear();

            Assert.False(nav.CanGoBack);
        }
    }
}