using System;
using FriendStorage.Model;
using FriendStorage.UI.DataProvider;

namespace FriendStorage.UI.ViewModel {
    public interface IFriendEditViewModel {
        void Load(int friendId);
        Friend Friend { get; }
    }

    public class FriendEditViewModel : ViewModelBase, IFriendEditViewModel {
        private readonly IFriendDataProvider friendDataProvider;
        private Friend friend;

        public FriendEditViewModel(IFriendDataProvider friendDataProvider) {
            this.friendDataProvider = friendDataProvider;
        }

        public Friend Friend {
            get { return friend; }
            private set {
                if (friend != value) {
                    friend = value;
                    OnPropertyChanged("Friend");
                }
            }
        }

        public void Load(int friendId) {
            Friend = friendDataProvider.GetFriendById(friendId);
        }
    }
}