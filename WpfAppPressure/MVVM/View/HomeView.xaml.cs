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

namespace WpfAppPressure.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private MainWindow window = (MainWindow)Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "MainWindower");

        private void SearchMonitoring_MouseDown(object sender, MouseButtonEventArgs e)
        {
            window.Is_Checked(2);
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            window.Is_Checked(3);
        }

        private void Border_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            window.Is_Checked(4);
        }
    }
}
