using System.Windows;

namespace FriendStorage.UI.Dialogs {
    public class MessageDialogService : IMessageDialogService {
        public MessageBoxResult ShowYesNoMessageBox(string message, string caption) {
            return MessageBox.Show(message, caption, MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes
                ? MessageBoxResult.Yes
                : MessageBoxResult.No;
        }
    }
}