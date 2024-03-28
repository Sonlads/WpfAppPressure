using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAppPressure.MVVM.ViewModel;

namespace WpfAppPressure
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }



        public void Is_Checked( int pageNumber)
        {
            if (pageNumber == 1)
            {
                PageHome.IsChecked = true;

                Contern.Content = new HomeViewModel();

                Console.WriteLine("переход в дом");     
                
            }
            else if (pageNumber == 2)
            {
                PageIzmer.IsChecked = true;

                Contern.Content = new PressureViewModel();

                Console.WriteLine("переход в давление");
        
            }
            else if (pageNumber == 3)
            {
                PageMonitor.IsChecked = true;

                Contern.Content = new MonitoringViewModel();

                Console.WriteLine("переход в мониторинг");

            }
            else if (pageNumber == 4)
            {
                PageProfil.IsChecked = true;

                Contern.Content = new ProfilViewModel();

                Console.WriteLine("переход в профиль");

            }


        }
       



        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            { 
                this.DragMove();
            }
        }

        private void MinButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.WindowState = WindowState.Minimized;
            }
        }

        private void ExitButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.Close();
            }
        }

        private void PageHome_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
               Console.WriteLine("Радио булочка дома");
                //Is_Checked(1);
            }

        }

        private void PageIzmer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                //Is_Checked(1);
                Console.WriteLine("Радио булочка давления");
            }
        }
    }

   
}
