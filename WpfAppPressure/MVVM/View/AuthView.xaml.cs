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
using WpfAppPressure.DBConnect;

namespace WpfAppPressure.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для AuthView.xaml
    /// </summary>
    public partial class AuthView : UserControl
    {
        public AuthView()
        {
            InitializeComponent();

            
        }

        
       

        private DBConnector DBconn {  get; set; }

      //private void RegWindowOpen(object sender, MouseButtonEventArgs e)
      //  {
      //      Console.WriteLine("регистрация йо");

      //      RegWindow regWindow = new RegWindow();

      //      regWindow.Show();

      //      Window namedWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "AuthWindower");

      //      namedWindow.Close();

            
      //  }
        private void ChangedEmail(Object sender, RoutedEventArgs args)
        {
            if (email.Text.Length == 0)
            {
                email.Background = (Brush)email.FindResource("CueBannerBrushEmail");
            }
            else
            {
                email.Background = new SolidColorBrush(Colors.White);
            }

            

            Console.WriteLine("меняю йомейл");
        }

        private void ChangedPassword(Object sender, RoutedEventArgs args)
        {
            if (password1.Password.Length == 0)
            {
                password1.Background = (Brush)password1.FindResource("CueBannerBrushPassword");
            }
            else
            {
                password1.Background = new SolidColorBrush(Colors.White);
            }
            
           

            Console.WriteLine("меняю йопароль");
        }

        private void AuthVerifyClick(object sender, RoutedEventArgs args)
        {
            
                Console.WriteLine("жмяк авторизации");

            string AuthEmail = email.Text;
            string AuthPassword = password1.Password;


            AuthWindow authwin = (AuthWindow)Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "AuthWindower");

            DBconn = new DBConnector();

            if (DBconn.BDCommandAuth(AuthEmail,AuthPassword))
            {
                Console.WriteLine("ура правильно");

                authwin.Dostup();
            }
            else
            {
                Console.WriteLine("блин не сошлось((");
            }

        }
    }
}
