using FriendStorage.Model;
using FriendStorage.UI.ViewModel;
using System;

namespace FriendStorage.UI.Wrappers {
    public class FriendWrapper : ViewModelBase {
        public Friend Model { get; private set; }

        public bool IsChanged { get; set; }

        public int Id {
            get { return Model.Id; }
            set {
                if (Model.Id != value) {
                    Model.Id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FirstName {
            get { return Model.FirstName; }
            set {
                if (Model.FirstName != value) {
                    Model.FirstName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string LastName {
            get { return Model.LastName; }
            set {
                if (Model.LastName != value) {
                    Model.LastName = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime? Birthday {
            get { return Model.Birthday; }
            set {
                if (Model.Birthday != value) {
                    Model.Birthday = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsDeveloper {
            get { return Model.IsDeveloper; }
            set {
                if (Model.IsDeveloper != value) {
                    Model.IsDeveloper = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isChanged;

        public FriendWrapper(Friend model) {
            this.Model = model;
        }

        protected override void OnPropertyChanged(string propertyName = null) {
            IsChanged = true;
            base.OnPropertyChanged(propertyName);
        }
    }
}