using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FriendStorage.UI.Command;
using FriendStorage.UI.Events;
using Prism.Events;

namespace FriendStorage.UI.ViewModel {

    public class NavigationItemViewModel {

        public int Id { get; private set; }
        public string DisplayMember { get; private set; }
        public ICommand OpenFriendEditViewCommand { get; private set; }

        private IEventAggregator eventAggregator;

        public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator) {
            Id = id;
            DisplayMember = displayMember;
            OpenFriendEditViewCommand = new DelegateCommand(OnFriendEditViewExecute);
            this.eventAggregator = eventAggregator;
        }

        private void OnFriendEditViewExecute(object obj) {
            eventAggregator.GetEvent<OpenFriendEditViewEvent>().Publish(Id);
        }
    }
}
