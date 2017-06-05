using System;
using FriendStorage.DataAccess;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.View;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using FriendStorage.UI.Command;
using FriendStorage.UI.Events;

namespace FriendStorage.UI.ViewModel {
    public class MainViewModel : ViewModelBase {
        public INavigationViewModel NavigationViewModel { get; private set; }

        public IFriendEditViewModel SelectedFriendEditViewModel {
            get { return selectedFriendEditViewModel; }
            set {
                if (value != selectedFriendEditViewModel) {
                    selectedFriendEditViewModel = value;
                    OnPropertyChanged("SelectedFriendEditViewModel");
                }
            }
        }

        public ObservableCollection<IFriendEditViewModel> FriendEditViewModels { get; private set; }
        public ICommand CloseFriendTabCommand { get; private set; }
        public ICommand AddFriendCommand { get; private set; }

        private IFriendEditViewModel selectedFriendEditViewModel;
        private Func<IFriendEditViewModel> friendViewModelCreator;
        private IEventAggregator eventAggregator;

        public MainViewModel(INavigationViewModel navigationViewModel,
            Func<IFriendEditViewModel> friendEditViewModelCreator, IEventAggregator eventAggregator) {
            NavigationViewModel = navigationViewModel;
            this.friendViewModelCreator = friendEditViewModelCreator;
            this.eventAggregator = eventAggregator;
            FriendEditViewModels = new ObservableCollection<IFriendEditViewModel>();

            eventAggregator.GetEvent<OpenFriendEditViewEvent>().Subscribe(OnOpenFriendEditView);
            eventAggregator.GetEvent<OnDeleteFriendEvent>().Subscribe(OnDeleteFriend);
            CloseFriendTabCommand = new DelegateCommand(OnCloseTabExecute);
            AddFriendCommand = new DelegateCommand(OnAddFriendExecute);
        }

        private void OnAddFriendExecute(object ignored) {
            SelectedFriendEditViewModel = CreateAndLoadFriendEditViewModel(null);
        }

        private void OnDeleteFriend(int friendId) {
            FriendEditViewModels.Remove(
                FriendEditViewModels.SingleOrDefault<IFriendEditViewModel>(model => model.Friend.Id == friendId));
        }

        private void OnCloseTabExecute(object friendEditViewModel) {
            FriendEditViewModels.Remove((IFriendEditViewModel) friendEditViewModel);
        }

        private void OnOpenFriendEditView(int friendId) {
            IFriendEditViewModel selectedFriendViewModel =
                FriendEditViewModels.SingleOrDefault(model => model.Friend.Id == friendId);

            if (selectedFriendViewModel == null) {
                selectedFriendViewModel = CreateAndLoadFriendEditViewModel(friendId);
            }

            SelectedFriendEditViewModel = selectedFriendViewModel;
        }

        private IFriendEditViewModel CreateAndLoadFriendEditViewModel(int? friendId) {
            IFriendEditViewModel selectedFriendViewModel = friendViewModelCreator();
            FriendEditViewModels.Add(selectedFriendViewModel);
            selectedFriendViewModel.Load(friendId);
            return selectedFriendViewModel;
        }

        public void Load() {
            NavigationViewModel.Load();
        }
    }
}