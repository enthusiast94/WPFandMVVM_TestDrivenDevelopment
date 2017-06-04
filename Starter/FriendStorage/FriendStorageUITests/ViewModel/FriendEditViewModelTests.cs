using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.ViewModel;
using FriendStorageUITests.Util;
using Moq;
using Prism.Events;
using Xunit;

namespace FriendStorageUITests.ViewModel {
    public class FriendEditViewModelTests {
        private readonly int friendId = 5;
        private Mock<IFriendDataProvider> friendDataProviderMock;
        private FriendEditViewModel viewModel;
        private Mock<IEventAggregator> eventAggregatorMock;

        public FriendEditViewModelTests() {
            friendDataProviderMock = new Mock<IFriendDataProvider>();
            friendDataProviderMock.Setup(provider => provider.GetFriendById(friendId))
                .Returns(new Friend() {Id = friendId, FirstName = "Manas"});
            eventAggregatorMock = new Mock<IEventAggregator>();

            viewModel = new FriendEditViewModel(friendDataProviderMock.Object, eventAggregatorMock.Object);
        }

        [Fact]
        public void ShouldLoadFriend() {
            viewModel.Load(friendId);

            Assert.NotNull(viewModel.Friend);
            Assert.Equal(friendId, viewModel.Friend.Id);
            friendDataProviderMock.Verify(provider => provider.GetFriendById(friendId), Times.Once);
        }

        [Fact]
        public void ShouldRaisePropertyChangedEventForFriend() {
            bool fired = viewModel.IsPorpertChangedFired(nameof(viewModel.Friend), () => {
                viewModel.Load(friendId);    
            });

            Assert.True(fired);
        }
    }
}