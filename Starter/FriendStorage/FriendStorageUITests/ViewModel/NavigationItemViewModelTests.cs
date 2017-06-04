using FriendStorage.UI.Events;
using FriendStorage.UI.ViewModel;
using FriendStorageUITests.Util;
using Moq;
using Prism.Events;
using Xunit;

namespace FriendStorageUITests.ViewModel {
    public class NavigationItemViewModelTests {
        private int friendId = 1;
        private Mock<OpenFriendEditViewEvent> eventMock;
        private Mock<IEventAggregator> eventAggregatorMock;
        private NavigationItemViewModel viewModel;

        public NavigationItemViewModelTests() {
            eventMock = new Mock<OpenFriendEditViewEvent>();
            eventAggregatorMock = new Mock<IEventAggregator>();
            eventAggregatorMock
                .Setup(aggregator => aggregator.GetEvent<OpenFriendEditViewEvent>())
                .Returns(eventMock.Object);
            viewModel = new NavigationItemViewModel(friendId, "Manas Bajaj", eventAggregatorMock.Object);
        }

        [Fact]
        public void ShouldPublishOpenFriendEditViewEvent() {
            viewModel.OpenFriendEditViewCommand.Execute(null);

            eventMock.Verify(openFriendEditViewEvent => openFriendEditViewEvent.Publish(friendId), Times.Once);
        }

        [Fact]
        public void ShouldFirePropertyChangedEventWhenDisplayMemberChanges() {
            bool fired = viewModel.IsPorpertChangedFired(nameof(viewModel.DisplayMember), () => {
                viewModel.DisplayMember = "Changed";
            });

            Assert.True(fired);
        }
    }
}