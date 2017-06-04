using System;
using System.Collections.ObjectModel;
using System.Windows;
using FriendStorage.UI.ViewModel;
using Prism.Events;

namespace FriendStorage.UI.View {
    public partial class MainWindow : Window {
        private MainViewModel mainViewModel;

        public MainWindow(MainViewModel mainViewModel) {
            InitializeComponent();

            this.mainViewModel = mainViewModel;

            DataContext = mainViewModel;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs) {
            mainViewModel.Load();
        }
    }
}