using System;
using System.Windows;

namespace FriendStorage.UI.Dialogs {
    public interface IMessageDialogService {
        MessageBoxResult ShowYesNoMessageBox(string message, string caption);
    }

    public enum MessageBoxResult {
        Yes,
        No
    }

    public class MessageBoxService : IMessageDialogService {
        public MessageBoxResult ShowYesNoMessageBox(string message, string caption) {
            return MessageBox.Show(message, caption, MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes
                ? MessageBoxResult.Yes
                : MessageBoxResult.No;
        }
    }
}