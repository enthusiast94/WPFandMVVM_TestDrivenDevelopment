using System;
using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using System.Windows.Input;
using FriendStorage.UI.Command;
using Prism.Events;
using FriendStorage.UI.Events;

namespace FriendStorage.UI.ViewModel {
    public interface IFriendEditViewModel {
        void Load(int friendId);
        Friend Friend { get; }
        ICommand DeleteCommand { get; }
    }

    public class FriendEditViewModel : ViewModelBase, IFriendEditViewModel {
        private readonly IFriendDataProvider friendDataProvider;
        private Friend friend;
        private IEventAggregator eventAggregator;

        public Friend Friend {
            get { return friend; }
            private set {
                if (friend != value) {
                    friend = value;
                    OnPropertyChanged("Friend");
                }
            }
        }
        public ICommand DeleteCommand { get; private set; }

        public FriendEditViewModel(IFriendDataProvider friendDataProvider, IEventAggregator eventAggregator) {
            this.friendDataProvider = friendDataProvider;
            this.eventAggregator = eventAggregator;

            DeleteCommand = new DelegateCommand(OnDeleteButtonClicked);
        }
        
        public void Load(int friendId) {
            Friend = friendDataProvider.GetFriendById(friendId);
        }

        private void OnDeleteButtonClicked(object obj) {
            friendDataProvider.DeleteFriend(Friend.Id);
            eventAggregator.GetEvent<OnDeleteFriendEvent>().Publish(Friend.Id);
        }
    }
}