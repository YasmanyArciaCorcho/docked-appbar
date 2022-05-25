using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SearchBar.UI.Base
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase()
        {
            Initialize();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        public void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                throw new Exception("Invalid property name: " + propertyName);
            }
        }

        protected virtual void Initialize()
        {
        }
    }
}
