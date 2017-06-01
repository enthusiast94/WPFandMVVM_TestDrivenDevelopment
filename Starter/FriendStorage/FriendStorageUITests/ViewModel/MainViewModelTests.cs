using System;
using FriendStorage.UI.ViewModel;
using Xunit;
using Moq;

namespace FriendStorageUITests.ViewModel {

    public class MainViewModelTests {

        [Fact]
        public void ShouldCallTheLoadMethodOfTheNavigationViewModel() {
            Mock<INavigationViewModel> navigationViewModelMock = new Mock<INavigationViewModel>();
            MainViewModel mainViewModel = new MainViewModel(navigationViewModelMock.Object);
            mainViewModel.Load();

            navigationViewModelMock.Verify(model => model.Load(), Times.Once);            
        }
    }
}