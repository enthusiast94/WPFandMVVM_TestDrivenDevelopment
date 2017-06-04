using System;
using System.Collections.ObjectModel;
using FriendStorage.DataAccess;
using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using Prism.Events;

namespace FriendStorage.UI.ViewModel {
    public interface INavigationViewModel {
        void Load();
    }

    public class NavigationViewModel : ViewModelBase, INavigationViewModel {

        public ObservableCollection<NavigationItemVIewModel> Friends { get; set; }
        private INavigationDataProvider navigationDataProvider;
        private IEventAggregator eventAggregator;

        public NavigationViewModel(INavigationDataProvider navigationDataProvider, IEventAggregator eventAggregator) {
            Friends = new ObservableCollection<NavigationItemVIewModel>();
            this.navigationDataProvider = navigationDataProvider;
            this.eventAggregator = eventAggregator;
        }

        public void Load() {
            Friends.Clear();

            foreach (var friend in navigationDataProvider.GetAllFriends()) {
                Friends.Add(new NavigationItemVIewModel(friend.Id, friend.DisplayMember, eventAggregator));               
            }
        }
    }
}