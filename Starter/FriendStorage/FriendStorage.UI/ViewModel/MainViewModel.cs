using System;
using FriendStorage.DataAccess;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.View;

namespace FriendStorage.UI.ViewModel {
    public class MainViewModel : ViewModelBase {
        public INavigationViewModel NavigationViewModel { get; private set; }

        public MainViewModel(INavigationViewModel navigationViewModel) {
            var navigationDataProvider = new NavigationDataProvider(() => new FileDataService());
            NavigationViewModel = navigationViewModel;
        }

        public void Load() {
            NavigationViewModel.Load();
        }
    }
}