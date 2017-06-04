using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FriendStorage.DataAccess;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.View;
using FriendStorage.UI.ViewModel;
using Prism.Events;

namespace FriendStorage.UI {
    class Bootstrapper {

        public IContainer Bootstrap() {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<FriendEditViewModel>().As<IFriendEditViewModel>();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<FileDataService>().As<IDataService>();
            builder.RegisterType<NavigationDataProvider>().As<INavigationDataProvider>();
            builder.RegisterType<FriendDataProvider>().As<IFriendDataProvider>();

            return builder.Build();
        }
    }
}