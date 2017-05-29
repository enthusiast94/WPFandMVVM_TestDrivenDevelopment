using System;
using System.Collections.Generic;
using System.Linq;
using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.ViewModel;
using Xunit;

namespace FriendStorageUITests.ViewModel {

    public class NavigationViewModelTests {

        [Fact]
        public void ShouldLoadFriends() {
            NavigationViewModel navigationViewModel = new NavigationViewModel(new NavigationDataProviderMock());
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
            NavigationViewModel navigationViewModel = new NavigationViewModel(new NavigationDataProviderMock());
            navigationViewModel.Load();
            navigationViewModel.Load();

            Assert.Equal(2, navigationViewModel.Friends.Count);
        }
    }

    public class NavigationDataProviderMock : INavigationDataProvider {
        public IEnumerable<LookupItem> GetAllFriends() {
            yield return new LookupItem() {Id = 1, DisplayMember = "Manas Bajaj"};
            yield return new LookupItem() {Id = 2, DisplayMember = "Gautam Bajaj"};
        }
    }
}