using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppPressure.Core;

namespace WpfAppPressure.MVVM.ViewModel
{
    class RegViewModel : ObservableObject

    {


        private object _currentView;
        public object RegVM
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        
            

        public RegViewModel()
        {

            RegVM = this;

            // CurrentView = new Reg2ViewModel();

            //var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);


            //RegWindow window = (RegWindow)Application.Current.MainWindow;

            //Reg1ViewCommand = new RelayCommand((o =>
            //{
            //    CurrentView = new Reg2ViewModel();



            //    window.Is_Checkered(1);





            //    Console.WriteLine("переход в регистр 22222");
            //    Console.WriteLine(CurrentView);
            //}));

            //Reg2ViewCommand = new RelayCommand((o =>
            //{

            //}));

        }
    }

}
