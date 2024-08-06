using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfAppPressure.Core;

namespace WpfAppPressure.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand PressureViewCommand { get; set; }
        public RelayCommand MonitoringViewCommand { get; set; }
        public RelayCommand ProfilViewCommand { get; set; }

        private MainWindow window = (MainWindow)Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "MainWindower");

        public HomeViewModel HomeVM {get; set;}


        private object _currentView;
        
        public object CurrentView
        {
            get { return _currentView; }
            set
            { _currentView = value;
                OnPropertyChanged();
            }
        }

        

        public MainViewModel() 
        {
            HomeVM = new HomeViewModel();

            

            

            HomeViewCommand = new RelayCommand((o => 
            {
                
                window.Is_Checked(1);
            }));

            PressureViewCommand = new RelayCommand((o =>
            {
              
                window.Is_Checked(2);            
            }));

            MonitoringViewCommand = new RelayCommand((o =>
            {
               
                window.Is_Checked(3);
            }));

            ProfilViewCommand = new RelayCommand((o =>
            {
                
                window.Is_Checked(4);
            }));
        }

    }
}
