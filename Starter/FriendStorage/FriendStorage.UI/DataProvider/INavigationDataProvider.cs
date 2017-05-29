using FriendStorage.Model;
using System.Collections;
using System.Collections.Generic;

namespace FriendStorage.UI.DataProvider {

    public interface INavigationDataProvider {
        IEnumerable<LookupItem> GetAllFriends();
    }
}