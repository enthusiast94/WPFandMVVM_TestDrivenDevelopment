using System;
using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using System.Windows.Input;
using FriendStorage.UI.Command;
using Prism.Events;
using FriendStorage.UI.Events;
using FriendStorage.UI.Wrappers;

namespace FriendStorage.UI.ViewModel {
    public interface IFriendEditViewModel {
        void Load(int? friendId);
        FriendWrapper Friend { get; }
    }

    public class FriendEditViewModel : ViewModelBase, IFriendEditViewModel {
        private readonly IFriendDataProvider friendDataProvider;
        private FriendWrapper friend;
        private IEventAggregator eventAggregator;

        public FriendWrapper Friend {
            get { return friend; }
            private set {
                if (friend != value) {
                    friend = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand DeleteCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        public FriendEditViewModel(IFriendDataProvider friendDataProvider, IEventAggregator eventAggregator) {
            this.friendDataProvider = friendDataProvider;
            this.eventAggregator = eventAggregator;

            DeleteCommand = new DelegateCommand(OnDeleteButtonClicked, CanDelete);
            SaveCommand = new DelegateCommand(OnSaveButtonClicked, CanSave);
        }

        public void Load(int? friendId) {
            Friend = friendId.HasValue 
                ? new FriendWrapper(friendDataProvider.GetFriendById(friendId.Value)) 
                : new FriendWrapper(new Friend());

            Friend.PropertyChanged += Friend_PropertyChanged;

        }

        private void Friend_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            RefreshSaveButtonState();
        }

        private void OnDeleteButtonClicked(object obj) {
            friendDataProvider.DeleteFriend(friend.Id);
            eventAggregator.GetEvent<OnDeleteFriendEvent>().Publish(friend.Id);
        }

        private void OnSaveButtonClicked(object obj) {
            friendDataProvider.SaveFriend(friend.Model);
            friend.IsChanged = false;
            eventAggregator.GetEvent<OnFriendSavedEvent>().Publish(friend.Model);
            RefreshSaveButtonState();
            RefreshDeleteButtonState();
        }

        private bool CanSave(object arg) {
            return friend.IsChanged;
        }

        private bool CanDelete(object arg) {
            return Friend.Id > 0;
        }

        private void RefreshSaveButtonState() {
            ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
        }

        private void RefreshDeleteButtonState() {
            ((DelegateCommand) DeleteCommand).RaiseCanExecuteChanged();
        }
    }
}