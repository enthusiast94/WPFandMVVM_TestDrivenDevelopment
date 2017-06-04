using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FriendStorage.DataAccess;
using FriendStorage.Model;

namespace FriendStorage.UI.DataProvider {
    class FriendDataProvider : IFriendDataProvider {
        private readonly Func<IDataService> dataServiceCreator;

        public FriendDataProvider(Func<IDataService> dataServiceCreator) {
            this.dataServiceCreator = dataServiceCreator;
        }

        public void DeleteFriend(int id) {
            using (IDataService dataService = dataServiceCreator()) {
                dataService.DeleteFriend(id);
            }
        }

        public Friend GetFriendById(int id) {
            using (IDataService dataService = dataServiceCreator()) {
                return dataService.GetFriendById(id);
            }
        }

        public void SaveFriend(Friend friend) {
            using (IDataService dataService = dataServiceCreator()) {
                dataService.SaveFriend(friend);
            }
        }
    }
}