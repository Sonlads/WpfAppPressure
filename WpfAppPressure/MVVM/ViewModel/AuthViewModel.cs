using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfAppPressure.Core;

namespace WpfAppPressure.MVVM.ViewModel
{
     class AuthViewModel : ObservableObject
    {


        private object _currentView;
        public object AuthVM
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }



    public AuthViewModel()
         {

            AuthVM = this;
           
        }

        
    }
}
