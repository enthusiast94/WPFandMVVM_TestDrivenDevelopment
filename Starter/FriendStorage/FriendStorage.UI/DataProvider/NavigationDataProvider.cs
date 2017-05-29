using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FriendStorage.DataAccess;
using FriendStorage.Model;

namespace FriendStorage.UI.DataProvider {
    class NavigationDataProvider : INavigationDataProvider {

        private Func<IDataService> dataServiceCreator;

        public NavigationDataProvider(Func<IDataService> dataServiceCreator) {
            this.dataServiceCreator = dataServiceCreator;
        }

        public IEnumerable<LookupItem> GetAllFriends() {
            using (var dataService = dataServiceCreator()) {
                return dataService.GetAllFriends();
            }
        }
    }
}