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
using System.Windows.Media.Media3D;
using MySql.Data.MySqlClient;
using System.IO;
using System.Net;

namespace WpfAppPressure.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для SearchPatientView.xaml
    /// </summary>
    public partial class SearchPatientView : UserControl
    {

        private string query = null;
        static private string MySQLConnect = "server=127.0.0.1; uid=sonlads; pwd=SD33bn55_; database=pressure;";
        private MySqlConnection databaseConnection = new MySqlConnection(MySQLConnect);
        private MySqlDataReader reader = null;
        private MySqlCommand command = null;
        private DBConnector connector = new DBConnector();



        List<string> FIO = new List<string> { " " };
        List<string> phone_number = new List<string> { " " };
        List<string> email = new List<string> { " " };

        List<int> IndividualValue = new List<int> { -1 };

        public SearchPatientView()
        {
            InitializeComponent();

            SearchPatient_BD_Defoult();
        }

       

        //private void Reg2Down(object sender, RoutedEventArgs e)
        //{
        //    Window namedWindow = new AuthWindow();

        //    namedWindow.Show();

        //    namedWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "RegWindower");

        //    namedWindow.Close();

        //}

        private void SearcherPatient_Click(object sender, RoutedEventArgs e)
        {
            if(SeacherCombo.SelectedIndex == 0)
            {
                SearchPatient_BD_Individ();
            }
            else
                if(SeacherCombo.SelectedIndex == 1)
                {
                     SearchPatient_BD_FIO();
                     
                 }
            else
                if (SeacherCombo.SelectedIndex == 2)
                {
                    SearchPatient_BD_Number_Phone();
                }
        }

        private void SearchPatient_BD_Defoult( )
        {

            query = "SELECT accounts.id_account, accounts.surname, accounts.name, accounts.middle_name, accounts.phone_number, accounts.email FROM accounts "+
                "JOIN patients ON accounts.id_account = patients.id_account_fk ";

            command = new MySqlCommand(query, databaseConnection);

            try
            {
                // Open the database
                databaseConnection.Open();

                // Execute the query
                reader = command.ExecuteReader();


                FIO = new List<string>();

                phone_number = new List<string>();

                email = new List<string>();

                IndividualValue = new List<int>();

                while (reader.Read())
                {
                    //customAxisLabels[index] = reader.GetString(0);

                    IndividualValue.Add(reader.GetInt32(0));
                    FIO.Add(" "+reader.GetString(1)+" "+ reader.GetString(2) + " "+ reader.GetString(3));
                    phone_number.Add(reader.GetString(4));
                    email.Add(reader.GetString(5));
                    

                    // index++;
                }
                reader.Close();

                SearcherPatientConstruct(IndividualValue, FIO,phone_number,email);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                databaseConnection.Close();

            }
        }
        private void SearchPatient_BD_FIO()
        {
            string Search = "%"+SearcherPatientBox.Text+"%";

            query = "SELECT accounts.id_account, accounts.surname, accounts.name, accounts.middle_name, accounts.phone_number, accounts.email FROM accounts " +
                "JOIN patients ON accounts.id_account = patients.id_account_fk "+
                "WHERE CONCAT(accounts.surname,' ', accounts.name,' ', accounts.middle_name) LIKE '"+Search+"'";

            command = new MySqlCommand(query, databaseConnection);

            try
            {
                // Open the database
                databaseConnection.Open();

                // Execute the query
                reader = command.ExecuteReader();


                FIO = new List<string>();

                phone_number = new List<string>();

                email = new List<string>();

                IndividualValue = new List<int>();

                while (reader.Read())
                {
                    //customAxisLabels[index] = reader.GetString(0);

                    IndividualValue.Add(reader.GetInt32(0));
                    FIO.Add(" " + reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3));
                    phone_number.Add(reader.GetString(4));
                    email.Add(reader.GetString(5));


                    // index++;
                }
                reader.Close();

                SearcherPatientConstruct(IndividualValue, FIO, phone_number, email);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                databaseConnection.Close();

            }
        }
        private void SearchPatient_BD_Individ()
        {

            query = "SELECT accounts.id_account, accounts.surname, accounts.name, accounts.middle_name, accounts.phone_number, accounts.email FROM accounts " +
                "JOIN patients ON accounts.id_account = patients.id_account_fk "+
                 "WHERE   accounts.id_account ='" +SearcherPatientBox.Text+ "'";

            command = new MySqlCommand(query, databaseConnection);

            try
            {
                // Open the database
                databaseConnection.Open();

                // Execute the query
                reader = command.ExecuteReader();


                FIO = new List<string>();

                phone_number = new List<string>();

                email = new List<string>();
                IndividualValue = new List<int>();

                while (reader.Read())
                {
                    //customAxisLabels[index] = reader.GetString(0);

                    IndividualValue.Add(reader.GetInt32(0));
                    FIO.Add(" " + reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3));
                    phone_number.Add(reader.GetString(4));
                    email.Add(reader.GetString(5));


                    // index++;
                }
                reader.Close();

                SearcherPatientConstruct(IndividualValue, FIO, phone_number, email);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                databaseConnection.Close();

            }
        }
        private void SearchPatient_BD_Number_Phone()
        {

            query = "SELECT accounts.id_account, accounts.surname, accounts.name, accounts.middle_name, accounts.phone_number, accounts.email FROM accounts " +
               "JOIN patients ON accounts.id_account = patients.id_account_fk " +
               "WHERE accounts.phone_number LIKE '%"+SearcherPatientBox.Text+"%'";

            command = new MySqlCommand(query, databaseConnection);

            try
            {
                // Open the database
                databaseConnection.Open();

                // Execute the query
                reader = command.ExecuteReader();


                FIO = new List<string>();

                phone_number = new List<string>();

                email = new List<string>();

                IndividualValue = new List<int>();

                while (reader.Read())
                {
                    //customAxisLabels[index] = reader.GetString(0);

                    IndividualValue.Add(reader.GetInt32(0));
                    FIO.Add(" " + reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3));
                    phone_number.Add(reader.GetString(4));
                    email.Add(reader.GetString(5));


                    // index++;
                }
                reader.Close();

                SearcherPatientConstruct(IndividualValue, FIO, phone_number, email);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                databaseConnection.Close();

            }
        }
        private MainWindow window = (MainWindow)Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "MainWindower");



        private void Searcher_Click( int id_patient,int Profil_or_Monitor)
        {
            if (Profil_or_Monitor == 0)
            {
                
                window.Monitor_Patient(id_patient);
                window.HistoryViewAdd(2);
            }
            else if (Profil_or_Monitor == 1)
            {
                window.Profil_Patient(id_patient);
                window.HistoryViewAdd(1);
            }
        }

        private void SearcherPatientConstruct(List<int>individ_number, List<string> FIO, List<string> phone_number, List<string> email )
        {
            WrapInfoPatient.Children.Clear();

            string UsersProfils = "";

            
            
            for (int i = 0; i < individ_number.Count; i++)
            {
                

                Grid  grid = new Grid();
                grid.HorizontalAlignment = HorizontalAlignment.Center;
                grid.VerticalAlignment = VerticalAlignment.Center;
                grid.Width = 150; // Ширина изображения
                grid.Height = 150;

                RectangleGeometry clipGeometry = new RectangleGeometry
                {
                    Rect = new Rect(0, 0, grid.Width, grid.Height),
                    RadiusX = 10,
                    RadiusY = 10
                };
                int currentIndex = i;
                grid.Clip = clipGeometry;

                
                
                // Создаем StackPanel, который будет содержать TextBlock и Label
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Vertical;
                stackPanel.Margin = new Thickness(10);
                stackPanel.Width = 318;
                stackPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                //stackPanel.MouseDown += (sender, e) => Searcher_Monitoring_Click(sender, e, individ_number[currentIndex]); 

                Image image = new Image();
                WebClient webClient = new WebClient();
                UsersProfils = "USER_" + individ_number[i] + "/";
                Console.WriteLine(UsersProfils);
                // Загружаем данные изображения
                byte[] imageData = webClient.DownloadData("http://localhost/Pressure/uploads/"+UsersProfils+"default.png");

                // Создаем объект BitmapImage и загружаем в него данные изображения
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(imageData);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                image.Source = bitmap;
                // Настройте другие свойства изображения (если необходимо)
               
                image.Stretch = Stretch.UniformToFill;
                image.HorizontalAlignment = HorizontalAlignment.Center;
                image.VerticalAlignment = VerticalAlignment.Center;
               // Высота изображения
               // Высота изображения
                //image.Margin = new Thickness(10);

                grid.Children.Add(image);
                stackPanel.Children.Add(grid);

                Border borderInfoPatient = new Border();
                borderInfoPatient.Margin = new Thickness(10);
                borderInfoPatient.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                borderInfoPatient.Style = (Style)SearchPatientorView.Resources["StackPanelShadow"];
                borderInfoPatient.CornerRadius = new CornerRadius(10, 10, 10, 10);

                // Создаем Label
                System.Windows.Controls.Label label = new System.Windows.Controls.Label();
                label.Content = $"{FIO[i]}";
                label.FontSize = 18;
                label.FontWeight = FontWeights.Bold;
                stackPanel.Children.Add(label);

                label = new System.Windows.Controls.Label();
                label.Content = $"Индивидуальный номер:  {individ_number[i]}";
                label.FontSize = 14;
                stackPanel.Children.Add(label);

                label = new System.Windows.Controls.Label();
                label.Content = $"электронная почта:  {email[i]}";
                label.FontSize = 14;
                stackPanel.Children.Add(label);

                label = new System.Windows.Controls.Label();
                label.Content = $"Номер телефона: {phone_number[i]}";
                label.FontSize = 14;
                stackPanel.Children.Add(label);


                grid = new Grid();
                grid.Margin = new Thickness(5);

                Button button = new Button();
                button.Width = 150;
                button.Height = 30;
                button.Content = "Мониторинг пациента";
                button.Style = (Style)SearchPatientorView.Resources["RoundButtonStyle"];
                button.Margin = new Thickness(10,5,10,5);
                button.HorizontalAlignment = HorizontalAlignment.Left;
                button.Click += (sender, e) => Searcher_Click( individ_number[currentIndex], 0);
                grid.Children.Add(button);

                button = new Button();
                button.Width = 130;
                button.Height = 30;
                button.Content = "Профиль пациента";
                button.Style = (Style)SearchPatientorView.Resources["RoundButtonStyle"];
                button.Margin = new Thickness(10,5,10,5);
                button.HorizontalAlignment = HorizontalAlignment.Right;
                button.Click += (sender, e) => Searcher_Click( individ_number[currentIndex], 1);

                grid.Children.Add(button);
                stackPanel.Children.Add(grid);


                // Добавляем StackPanel в WrapPanel
                borderInfoPatient.Child = stackPanel;
                WrapInfoPatient.Children.Add(borderInfoPatient);

            
            }
        }






        // Console.WriteLine("Фамилия:" + UserSurname + "   Имя:" + UserName + "   Отчество:" + UserMiddleName + "  Дата:" + UserDateBirthday );



    }
}
