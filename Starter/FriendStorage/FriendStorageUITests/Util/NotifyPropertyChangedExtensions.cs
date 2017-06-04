using System;
using System.ComponentModel;

namespace FriendStorageUITests.Util {
    public static class NotifyPropertyChangedExtensions {
        public static bool IsPorpertChangedFired(this INotifyPropertyChanged notifyPropertyChanged, string propertyName, Action actiion) {
            bool fired = false;

            notifyPropertyChanged.PropertyChanged += (sender, args) => {
                if (args.PropertyName == propertyName) {
                    fired = true;
                }
            };

            actiion();

            return fired;
        }
    }
}