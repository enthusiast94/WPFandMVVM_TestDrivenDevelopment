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
}