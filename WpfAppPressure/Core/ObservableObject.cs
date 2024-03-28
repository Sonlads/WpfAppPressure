using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfAppPressure.Core
{
    internal class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnePropertyChanged()
        {
            OnPropertyChanged();
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
