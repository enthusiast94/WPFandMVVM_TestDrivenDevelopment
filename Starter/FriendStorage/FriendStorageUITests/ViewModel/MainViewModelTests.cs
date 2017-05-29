using System;
using FriendStorage.UI.ViewModel;
using Xunit;

namespace FriendStorageUITests.ViewModel {

    public class MainViewModelTests {

        [Fact]
        public void ShouldCallTheLoadMethodOfTheNavigationViewModel() {
            var navigationViewModelMock = new NavigationViewModelMock();
            var mainViewModel = new MainViewModel(navigationViewModelMock);
            mainViewModel.Load();

            Assert.True(navigationViewModelMock.HasLoadBeenCalled);            
        }
    }

    public class NavigationViewModelMock : INavigationViewModel {

        public bool HasLoadBeenCalled { get; private set; }

        public void Load() {
            HasLoadBeenCalled = true;
        }
    }
}