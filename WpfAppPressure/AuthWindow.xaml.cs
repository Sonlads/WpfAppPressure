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
using WpfAppPressure.DBConnect;

namespace WpfAppPressure
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {

        
        public AuthWindow()
        {
            string tokeno = Properties.Settings.Default.token;

            DBConnector dBConnect = new DBConnector();

            if (dBConnect.TokenCheck(tokeno))
            {
                Dostup();
                this.Close();
            }

            else
            {
                InitializeComponent();

               // ConternAuth.Content = new AuthViewModel();
            }

        }

        public AuthWindow authwind {  get; set; }
       

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

        public void Dostup( )
        {

                

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();  
           
                this.Close();
            
        }
    }
}

