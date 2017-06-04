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

namespace FriendStorageUITests.ViewModel {

    public class NavigationViewModelTests {

        private NavigationViewModel navigationViewModel;

        public NavigationViewModelTests() {
            Mock<INavigationDataProvider> navigationDataProviderMock = new Mock<INavigationDataProvider>();
            navigationDataProviderMock.Setup(provider => provider.GetAllFriends())
                .Returns(() => new List<LookupItem>() {
                    new LookupItem() {Id = 1, DisplayMember = "Manas Bajaj"},
                    new LookupItem() {Id = 2, DisplayMember = "Gautam Bajaj"},
                });
            Mock<IEventAggregator> eventAggregatorMock = new Mock<IEventAggregator>();
            eventAggregatorMock.Setup(aggregator => aggregator.GetEvent<OnDeleteFriendEvent>())
                .Returns(new OnDeleteFriendEvent());

            navigationViewModel = new NavigationViewModel(navigationDataProviderMock.Object, eventAggregatorMock.Object);
        }

        [Fact]
        public void ShouldLoadFriends() {
            navigationViewModel.Load();

            Assert.Equal(2, navigationViewModel.Friends.Count);

            var friend = navigationViewModel.Friends.SingleOrDefault(friend1 => friend1.Id == 1);
            Assert.NotNull(friend);
            Assert.Equal("Manas Bajaj", friend.DisplayMember);

            friend = navigationViewModel.Friends.SingleOrDefault(friend1 => friend1.Id == 2);
            Assert.NotNull(friend);
            Assert.Equal("Gautam Bajaj", friend.DisplayMember);
        }

        [Fact]
        public void ShouldLoadFriendsOnlyOnce() {
            navigationViewModel.Load();
            navigationViewModel.Load();

            Assert.Equal(2, navigationViewModel.Friends.Count);
        }
    }
}