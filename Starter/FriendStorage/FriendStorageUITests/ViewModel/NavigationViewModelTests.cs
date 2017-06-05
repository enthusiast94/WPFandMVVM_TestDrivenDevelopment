using System;
using System.Collections.Generic;
using System.Linq;
using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.ViewModel;
using Moq;
using Prism.Events;
using Xunit;
using FriendStorage.UI.Events;
using FriendStorageUITests.Util;

namespace FriendStorageUITests.ViewModel {
    public class NavigationViewModelTests {
        private NavigationViewModel viewModel;
        private OnDeleteFriendEvent onDeleteFriendEvent;
        private Mock<IEventAggregator> eventAggregatorMock;
        private OnFriendSavedEvent onFriendSavedEvent;

        public NavigationViewModelTests() {
            Mock<INavigationDataProvider> navigationDataProviderMock = new Mock<INavigationDataProvider>();
            navigationDataProviderMock.Setup(provider => provider.GetAllFriends())
                .Returns(() => new List<LookupItem>() {
                    new LookupItem() {Id = 1, DisplayMember = "Manas Bajaj"},
                    new LookupItem() {Id = 2, DisplayMember = "Gautam Bajaj"},
                });
            eventAggregatorMock = new Mock<IEventAggregator>();
            eventAggregatorMock.Setup(aggregator => aggregator.GetEvent<OnDeleteFriendEvent>())
                .Returns(new OnDeleteFriendEvent());
            onDeleteFriendEvent = eventAggregatorMock.Object.GetEvent<OnDeleteFriendEvent>();
            eventAggregatorMock.Setup(aggregator => aggregator.GetEvent<OnFriendSavedEvent>())
                .Returns(new OnFriendSavedEvent());
            onFriendSavedEvent = eventAggregatorMock.Object.GetEvent<OnFriendSavedEvent>();

            viewModel = new NavigationViewModel(navigationDataProviderMock.Object, eventAggregatorMock.Object);
        }

        [Fact]
        public void ShouldLoadFriends() {
            viewModel.Load();

            Assert.Equal(2, viewModel.Friends.Count);

            var friend = viewModel.Friends.SingleOrDefault(friend1 => friend1.Id == 1);
            Assert.NotNull(friend);
            Assert.Equal("Manas Bajaj", friend.DisplayMember);

            friend = viewModel.Friends.SingleOrDefault(friend1 => friend1.Id == 2);
            Assert.NotNull(friend);
            Assert.Equal("Gautam Bajaj", friend.DisplayMember);
        }

        [Fact]
        public void ShouldLoadFriendsOnlyOnce() {
            viewModel.Load();
            viewModel.Load();

            Assert.Equal(2, viewModel.Friends.Count);
        }

        [Fact]
        public void ShouldRemoveFriendOnReceivingDeleteEvent() {
            viewModel.Load();
            onDeleteFriendEvent.Publish(1);

            Assert.Equal(1, viewModel.Friends.Count);
            Assert.Equal(2, viewModel.Friends.First().Id);
        }

        [Fact]
        public void ShouldUpdateFriendsOnReceivingFriendSavedEvent() {
            viewModel.Load();
            onFriendSavedEvent.Publish(new Friend() {Id = 1, FirstName = "ManasChanged", LastName = "Bajaj"});

            Assert.Equal("ManasChanged Bajaj", viewModel.Friends.SingleOrDefault(model => model.Id == 1).DisplayMember);
        }

        [Fact]
        public void ShouldAddNavigationItemOnSavedEventIfItDoesNotAlreadyExist() {
            viewModel.Load();
            onFriendSavedEvent.Publish(new Friend() {Id = 3, FirstName = "New", LastName = "Friend"});

            Assert.Equal(3, viewModel.Friends.Count);
            Assert.Equal("New Friend", viewModel.Friends.SingleOrDefault(model => model.Id == 3).DisplayMember);
        }
    }
}