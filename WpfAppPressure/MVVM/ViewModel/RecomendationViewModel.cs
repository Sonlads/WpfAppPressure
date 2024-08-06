using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppPressure.Core;

namespace WpfAppPressure.MVVM.ViewModel
{

    internal class RecomendationViewModel : ObservableObject
    {
        private object _currentView;
        public object RecomendationVM
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public  RecomendationViewModel()
        {
            RecomendationVM = this;
        }
    }
}
