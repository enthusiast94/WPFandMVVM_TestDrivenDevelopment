using FriendStorage.UI.Events;
using FriendStorage.UI.ViewModel;
using Moq;
using Prism.Events;
using Xunit;

namespace FriendStorageUITests.ViewModel {

    public class NavigationItemViewModelTests {

        [Fact]
        public void ShouldPublishOpenFriendEditViewEvent() {
            int friendId = 1;
            Mock<OpenFriendEditViewEvent> eventMock = new Mock<OpenFriendEditViewEvent>();
            Mock<IEventAggregator> eventAggregatorMock = new Mock<IEventAggregator>();
            eventAggregatorMock
                .Setup(aggregator => aggregator.GetEvent<OpenFriendEditViewEvent>())
                .Returns(eventMock.Object);
            NavigationItemViewModel viewModel = new NavigationItemViewModel(friendId, "Manas Bajaj", eventAggregatorMock.Object);

            viewModel.OpenFriendEditViewCommand.Execute(null);

            eventMock.Verify(openFriendEditViewEvent => openFriendEditViewEvent.Publish(friendId), Times.Once);
        }
    }
}