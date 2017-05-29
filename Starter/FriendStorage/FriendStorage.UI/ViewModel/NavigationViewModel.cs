using System;
using System.Collections.ObjectModel;
using FriendStorage.DataAccess;
using FriendStorage.Model;
using FriendStorage.UI.DataProvider;

namespace FriendStorage.UI.ViewModel {

    public interface INavigationViewModel {
        void Load();
    }

    public class NavigationViewModel : ViewModelBase, INavigationViewModel {

        public ObservableCollection<LookupItem> Friends { get; set; }
        private INavigationDataProvider navigationDataProvider;

        public NavigationViewModel(INavigationDataProvider navigationDataProvider) {
            Friends = new ObservableCollection<LookupItem>();
            this.navigationDataProvider = navigationDataProvider;
        }

        public void Load() {
            Friends.Clear();
            foreach (var friend in navigationDataProvider.GetAllFriends()) {
                Friends.Add(friend);               
            }
        }
    }
}