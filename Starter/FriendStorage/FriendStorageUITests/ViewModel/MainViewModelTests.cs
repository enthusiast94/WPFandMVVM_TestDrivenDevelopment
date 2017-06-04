using System;
using FriendStorage.UI.Events;
using FriendStorage.UI.ViewModel;
using Xunit;
using Moq;
using Prism.Events;
using System.Collections.Generic;
using System.Linq;
using FriendStorage.Model;
using FriendStorage.UI.View;
using FriendStorageUITests.Util;
using FriendStorage.UI.Wrappers;

namespace FriendStorageUITests.ViewModel {
    public class MainViewModelTests {
        private List<Mock<IFriendEditViewModel>> friendEditViewModelMocks = new List<Mock<IFriendEditViewModel>>();
        private Mock<INavigationViewModel> navigationViewModelMock;
        private Mock<IEventAggregator> eventAggregatorMock;
        private MainViewModel mainViewModel;
        private OpenFriendEditViewEvent openFriendEditViewEvent;
        private OnDeleteFriendEvent onDeleteFriendEvent;

        public MainViewModelTests() {
            navigationViewModelMock = new Mock<INavigationViewModel>();
            eventAggregatorMock = new Mock<IEventAggregator>();
            eventAggregatorMock.Setup(aggregator => aggregator.GetEvent<OpenFriendEditViewEvent>())
                .Returns(new OpenFriendEditViewEvent());
            eventAggregatorMock.Setup(aggregator => aggregator.GetEvent<OnDeleteFriendEvent>())
                .Returns(new OnDeleteFriendEvent());
            openFriendEditViewEvent = eventAggregatorMock.Object.GetEvent<OpenFriendEditViewEvent>();
            onDeleteFriendEvent = eventAggregatorMock.Object.GetEvent<OnDeleteFriendEvent>();

            mainViewModel = new MainViewModel(navigationViewModelMock.Object, CreateFriendEditViewModel,
                eventAggregatorMock.Object);
        }

        [Fact]
        public void ShouldCallTheLoadMethodOfTheNavigationViewModel() {
            mainViewModel.Load();
            navigationViewModelMock.Verify(model => model.Load(), Times.Once);
        }

        [Fact]
        public void ShouldAddFriendEditViewModelAndLoadAndSelectIt() {
            openFriendEditViewEvent.Publish(1);

            Assert.Equal(1, mainViewModel.FriendEditViewModels.Count);
            IFriendEditViewModel selectedFriendEditViewModel = mainViewModel.FriendEditViewModels.First();
            Assert.Equal(selectedFriendEditViewModel, mainViewModel.SelectedFriendEditViewModel);
            friendEditViewModelMocks.First().Verify(model => model.Load(1), Times.Once);
        }

        [Fact]
        public void ShouldSelectExistingFriendEditViewModel() {
            openFriendEditViewEvent.Publish(1);
            Assert.True(mainViewModel.SelectedFriendEditViewModel.Friend.Id == 1);

            openFriendEditViewEvent.Publish(2);
            Assert.True(mainViewModel.SelectedFriendEditViewModel.Friend.Id == 2);

            openFriendEditViewEvent.Publish(1);
            Assert.True(mainViewModel.SelectedFriendEditViewModel.Friend.Id == 1);
        }

        [Fact]
        public void ShouldAddFriendEditViewModelOnlyOnce() {
            openFriendEditViewEvent.Publish(1);
            openFriendEditViewEvent.Publish(2);
            openFriendEditViewEvent.Publish(2);
            openFriendEditViewEvent.Publish(3);
            openFriendEditViewEvent.Publish(3);

            Assert.Equal(3, mainViewModel.FriendEditViewModels.Count);
        }

        [Fact]
        public void ShouldRaisePropertyChangedEventForSelectedFriendEditViewModel() {
            bool fired = mainViewModel.IsPorpertChangedFired(nameof(mainViewModel.SelectedFriendEditViewModel),
                () => {
                    Mock<IFriendEditViewModel> mock = new Mock<IFriendEditViewModel>();
                    mainViewModel.SelectedFriendEditViewModel = mock.Object;
                });

            Assert.True(fired);
        }

        [Fact]
        public void ShouldRemoveFriendEditViewModelWhenCloseTabButtonIsClicked() {
            openFriendEditViewEvent.Publish(1);
            openFriendEditViewEvent.Publish(2);

            mainViewModel.CloseFriendTabCommand.Execute(mainViewModel.FriendEditViewModels.Single(model => model.Friend.Id == 1));
            Assert.Equal(1, mainViewModel.FriendEditViewModels.Count);
            Assert.Equal(2, mainViewModel.FriendEditViewModels.First().Friend.Id);
        }

        [Fact]
        public void ShouldRemoveFriendOnReceivingDeleteEvent() {
            openFriendEditViewEvent.Publish(1);
            onDeleteFriendEvent.Publish(1);
            Assert.Equal(0, mainViewModel.FriendEditViewModels.Count);
        }

        private IFriendEditViewModel CreateFriendEditViewModel() {
            Mock<IFriendEditViewModel> mock = new Mock<IFriendEditViewModel>();
            mock.Setup(model => model.Load(It.IsAny<int>()))
                .Callback<int>(friendId => {
                    mock.Setup(model => model.Friend)
                        .Returns(new FriendWrapper(new Friend() { Id = friendId }));
                });
            friendEditViewModelMocks.Add(mock);
            return mock.Object;
        }
    }
}