using System;
using System.Collections.ObjectModel;
using FriendStorage.DataAccess;
using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using Prism.Events;
using FriendStorage.UI.Events;
using System.Linq;

namespace FriendStorage.UI.ViewModel {
    public interface INavigationViewModel {
        void Load();
    }

    public class NavigationViewModel : ViewModelBase, INavigationViewModel {
        public ObservableCollection<NavigationItemViewModel> Friends { get; set; }
        private INavigationDataProvider navigationDataProvider;
        private IEventAggregator eventAggregator;

        public NavigationViewModel(INavigationDataProvider navigationDataProvider, IEventAggregator eventAggregator) {
            Friends = new ObservableCollection<NavigationItemViewModel>();
            this.navigationDataProvider = navigationDataProvider;
            this.eventAggregator = eventAggregator;

            eventAggregator.GetEvent<OnDeleteFriendEvent>().Subscribe(OnDeleteFriend);
            eventAggregator.GetEvent<OnFriendSavedEvent>().Subscribe(OnSavedFriend);
        }

        private void OnSavedFriend(Friend savedFriend) {
            NavigationItemViewModel navigationItemViewModel = Friends.SingleOrDefault(model => model.Id == savedFriend.Id);
            if (navigationItemViewModel == null) {
                Friends.Add(new NavigationItemViewModel(savedFriend.Id, GetDisplayMember(savedFriend),
                    eventAggregator));
            } else {
                navigationItemViewModel.DisplayMember = GetDisplayMember(savedFriend);
            }
        }

        private static string GetDisplayMember(Friend savedFriend) {
            return $"{savedFriend.FirstName} {savedFriend.LastName}";
        }

        private void OnDeleteFriend(int friendId) {
            Friends.Remove(Friends.SingleOrDefault<NavigationItemViewModel>(friend => friend.Id == friendId));
        }

        public void Load() {
            Friends.Clear();

            foreach (var friend in navigationDataProvider.GetAllFriends()) {
                Friends.Add(new NavigationItemViewModel(friend.Id, friend.DisplayMember, eventAggregator));
            }
        }
    }
}