using FriendStorage.Model;
using Prism.Events;

namespace FriendStorage.UI.ViewModel {
    public class OnFriendSavedEvent : PubSubEvent<Friend> {
    }
}