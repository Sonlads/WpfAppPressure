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

namespace WpfAppPressure.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для RegView.xaml
    /// </summary>
    public partial class RegView : UserControl
    {
        public RegView()
        {
            InitializeComponent();
        }

        private void ChangedMiddleName(object sender, TextChangedEventArgs e)
        {
            if (MiddleName.Text.Length == 0)
            {
                MiddleName.Background = (Brush)MiddleName.FindResource("CueBannerBrushMiddleName");
            }
            else
            {
                MiddleName.Background = new SolidColorBrush(Colors.White);
            }
        }
        private void ChangedSurname(Object sender, RoutedEventArgs args)
        {
            if (Surname.Text.Length == 0)
            {
                Surname.Background = (Brush)Surname.FindResource("CueBannerBrushSurname");
            }
            else
            {
                Surname.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void ChangedNameUser(Object sender, RoutedEventArgs args)
        {
            if (NameUser.Text.Length == 0)
            {
                NameUser.Background = (Brush)NameUser.FindResource("CueBannerBrushNameUser");
            }
            else
            {
                NameUser.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void ChangedAgeUser(Object sender, RoutedEventArgs args)
        {
            if (AgeUser.Text.Length == 0)
            {
                AgeUser.Background = (Brush)AgeUser.FindResource("CueBannerBrushAgeUser");
            }
            else
            {
                AgeUser.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void ChangedWeightUser(Object sender, RoutedEventArgs args)
        {
            if (WeightUser.Text.Length == 0)
            {
                WeightUser.Background = (Brush)WeightUser.FindResource("CueBannerBrushWeightUser");
            }
            else
            {
                WeightUser.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void ChangedHeightUser(Object sender, RoutedEventArgs args)
        {
            if (HeightUser.Text.Length == 0)
            {
                HeightUser.Background = (Brush)HeightUser.FindResource("CueBannerBrushHeightUser");
            }
            else
            {
                HeightUser.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void ChechedGenders(Object sender, RoutedEventArgs args)
        {
            if (Gender.IsChecked == true) 
            {
                Male.Foreground = new SolidColorBrush(Colors.Gray);
                Female.Foreground = new SolidColorBrush(Colors.Black);

                BorderMale.BorderBrush = new SolidColorBrush(Colors.White);
                BorderFemale.BorderBrush = new SolidColorBrush(Colors.Gray);
            }
            else
            {
                Male.Foreground = new SolidColorBrush(Colors.Black);
                Female.Foreground = new SolidColorBrush(Colors.Gray);

                BorderMale.BorderBrush = new SolidColorBrush(Colors.Gray);
                BorderFemale.BorderBrush = new SolidColorBrush(Colors.White);
            }

        }

        private void Reg2Down(object sender, RoutedEventArgs e)
        {
            Window namedWindow = new AuthWindow();

            namedWindow.Show();

            namedWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "RegWindower");

            namedWindow.Close();

        }

        private void ChangedEmailUser(object sender, TextChangedEventArgs e)
        {
            if (EmailUser.Text.Length == 0)
            {
                EmailUser.Background = (Brush)EmailUser.FindResource("CueBannerBrushEmailUser");
            }
            else
            {
                EmailUser.Background = new SolidColorBrush(Colors.White);
            }

        }


        private void ChangedPhoneUser(object sender, TextChangedEventArgs e)
        {

            if (PhoneUser.Text.Length == 0)
            {
                PhoneUser.Background = (Brush)PhoneUser.FindResource("CueBannerBrushPhoneUser");
            }
            else
            {
                PhoneUser.Background = new SolidColorBrush(Colors.White);
            }

        }

        private void ChangedPasswordUser(object sender, TextChangedEventArgs e)
        {
            if (PasswordUser.Text.Length == 0)
            {
                PasswordUser.Background = (Brush)PasswordUser.FindResource("CueBannerBrushPasswordUser");
            }
            else
            {
                PasswordUser.Background = new SolidColorBrush(Colors.White);
            }

        }
        private void DateFocus(object sender, RoutedEventArgs e)
        {
            AgeUser.Background = new SolidColorBrush(Colors.Transparent);
            Razdel1.Visibility = Visibility.Visible;
            Razdel2.Visibility = Visibility.Visible;
        }

        private void LostDateFocus(object sender, RoutedEventArgs e)
        {
            
            if(string.IsNullOrEmpty(DayTextBox.Text))
            {
                if (string.IsNullOrEmpty(MonthTextBox.Text))
                {
                    if (string.IsNullOrEmpty(YearTextBox.Text))
                    {
                        AgeUser.Background = (Brush)AgeUser.FindResource("CueBannerBrushAgeUser");
                        Razdel1.Visibility = Visibility.Hidden;
                        Razdel2.Visibility = Visibility.Hidden;
                    }
                }
            }
            
        }

        private void DayTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            

            if (DayTextBox.Text.Length == 2)
            {
                MonthTextBox.Focus();
                MonthTextBox.SelectAll();
            }
        }

        private void MonthTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            

            if (MonthTextBox.Text.Length == 2)
            {
                YearTextBox.Focus();
                YearTextBox.SelectAll();
            }
        }

        private void YearTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // You can add further logic here if needed
           
        }

        private void RegFunAddBD(object sender, RoutedEventArgs e)
        {
            //ФИО пользователя
            string UserSurname = Surname.Text;
            string UserName = NameUser.Text;
            string UserMiddleName = MiddleName.Text;
            
            
            //Дата рождения пользователя
            DateTime UserDateBirthday = new DateTime( Convert.ToInt32(YearTextBox.Text), Convert.ToInt32(MonthTextBox.Text), Convert.ToInt32(DayTextBox.Text));

            //Рост и вес пользователя
            int UserWeight = Convert.ToInt32(WeightUser.Text);
            int UserHeight = Convert.ToInt32(HeightUser.Text);


            //Пол пользователя 
            int UserGender = 0;

            int p;

            int fgff;


            if ((bool)Gender.IsChecked)
            {
                UserGender = 1;
            }
            


            Console.WriteLine("Фамилия:" + UserSurname + "   Имя:" + UserName + "   Отчество:" + UserMiddleName + "  Дата:" + UserDateBirthday );
        }
    }
}
