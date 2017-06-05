using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Dialogs;
using FriendStorage.UI.Events;
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
        private Mock<OnFriendSavedEvent> onFriendSavedEventMock;
        private Mock<IMessageDialogService> messageDialogServiceMock;

        public FriendEditViewModelTests() {
            friendDataProviderMock = new Mock<IFriendDataProvider>();
            onFriendSavedEventMock = new Mock<OnFriendSavedEvent>();
            friendDataProviderMock.Setup(provider => provider.GetFriendById(friendId))
                .Returns(new Friend() {Id = friendId, FirstName = "Manas", LastName = "Bajaj"});
            eventAggregatorMock = new Mock<IEventAggregator>();
            eventAggregatorMock.Setup(aggregator => aggregator.GetEvent<OnDeleteFriendEvent>())
                .Returns(new OnDeleteFriendEvent());
            eventAggregatorMock.Setup(aggregator => aggregator.GetEvent<OnFriendSavedEvent>())
                .Returns(onFriendSavedEventMock.Object);
            messageDialogServiceMock = new Mock<IMessageDialogService>();

            viewModel = new FriendEditViewModel(friendDataProviderMock.Object, eventAggregatorMock.Object,
                messageDialogServiceMock.Object);
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
            bool fired = viewModel.IsPorpertChangedFired(nameof(viewModel.Friend), () => { viewModel.Load(friendId); });

            Assert.True(fired);
        }

        [Theory]
        [InlineData(MessageBoxResult.Yes, 1)]
        [InlineData(MessageBoxResult.No, 0)]
        public void ShouldDeleteFriendAndFireDeleteEventWhenDeleteButtonClickedAndDeleteConfirmed(
            MessageBoxResult messageBoxResult, int expectedNumTimes) {
            viewModel.Load(friendId);
            messageDialogServiceMock
                .Setup(service => service.ShowYesNoMessageBox(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(messageBoxResult);
            viewModel.DeleteCommand.Execute(null);

            friendDataProviderMock.Verify(provider => provider.DeleteFriend(friendId), Times.Exactly(expectedNumTimes));
            eventAggregatorMock.Verify(aggregator => aggregator.GetEvent<OnDeleteFriendEvent>(), Times.Exactly(expectedNumTimes));
        }

        [Fact]
        public void ShouldDisableSaveButtonOnLoad() {
            viewModel.Load(friendId);

            Assert.False(viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldEnableSaveButtonOnChange() {
            viewModel.Load(friendId);
            viewModel.Friend.FirstName = "Changed";

            Assert.True(viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldRaiseCanExecueChangedEventForSaveCommandOnChange() {
            viewModel.Load(friendId);
            bool fired = false;
            viewModel.SaveCommand.CanExecuteChanged += (sender, args) => fired = true;
            viewModel.Friend.FirstName = "Changed";

            Assert.True(fired);
        }

        [Fact]
        public void ShouldSaveFriendUsingDataProviderAndPublishSavedEventOnSaveButtonClick() {
            viewModel.Load(friendId);
            viewModel.Friend.FirstName = "Changed";
            viewModel.SaveCommand.Execute(null);

            friendDataProviderMock.Verify(provider => provider.SaveFriend(viewModel.Friend.Model), Times.Once);
            onFriendSavedEventMock.Verify(onSavedEvent => onSavedEvent.Publish(viewModel.Friend.Model), Times.Once);
        }

        [Fact]
        public void ShouldCreateNewFriendWhenNullIsPassedToLoadMethod() {
            viewModel.Load(null);

            Assert.NotNull(viewModel.Friend);
            Assert.Equal(0, viewModel.Friend.Id);
            Assert.Null(viewModel.Friend.FirstName);
            Assert.Null(viewModel.Friend.LastName);
            Assert.False(viewModel.Friend.IsDeveloper);
            Assert.Null(viewModel.Friend.Birthday);

            friendDataProviderMock.Verify(provider => provider.GetFriendById(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void ShouldDisableDeleteButtonWhenFriendDoesNotExist() {
            viewModel.Load(null);

            Assert.False(viewModel.DeleteCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldEnableDeleteButtonWhenFriendExists() {
            viewModel.Load(friendId);

            Assert.True(viewModel.DeleteCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldRaiseCanExecuteChangedOnDeleteCommandWhenFriendIsSaved() {
            viewModel.Load(friendId);
            bool fired = false;
            viewModel.SaveCommand.CanExecuteChanged += (sender, args) => fired = true;
            viewModel.SaveCommand.Execute(null);

            Assert.True(fired);
        }

        [Fact]
        public void ShouldShowCorrectDeleteDialogMessage() {
            viewModel.Load(friendId);
            viewModel.DeleteCommand.Execute(null);

            messageDialogServiceMock.Verify(service => service.ShowYesNoMessageBox($"Are you sure you want to delete the friend 'Manas Bajaj'", "Delete friend"), Times.Once);
        }
    }
}