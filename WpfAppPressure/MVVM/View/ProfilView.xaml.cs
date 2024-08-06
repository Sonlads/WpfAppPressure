using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.Win32;
using System.Collections.Specialized;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Policy;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Printing;
using MySql.Data.MySqlClient;
using System.Collections;
using WpfAppPressure.DBConnect;


namespace WpfAppPressure.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для Profil.xaml
    /// </summary>
    public partial class ProfilView : UserControl
    {

        private string token = Properties.Settings.Default.token;

        private string query = null;
        static private string MySQLConnect = "server=127.0.0.1; uid=sonlads; pwd=SD33bn55_; database=pressure;";
        private MySqlConnection databaseConnection = new MySqlConnection(MySQLConnect);
        private MySqlDataReader reader = null;
        private MySqlCommand command = null;
        private DBConnector connector = new DBConnector();
        public ProfilView()
        {
            InitializeComponent();

            int id_user = connector.TokenInID();
                
            PreLoadInfoProfile(id_user);

            string UsersProfils = "USER_"+id_user+"/";

            string imageUrl = "http://localhost/Pressure/uploads/"+UsersProfils+"default.png";

            LoadImageFromUrl(imageUrl);

            //Thickness myThickness = new Thickness();
            //myThickness.Bottom = 20;
            //myThickness.Left = 20;
            //for (int i = 0; i < 5; i++)
            //{
            //    TextBlock textBlock1 = new TextBlock
            //    {
            //        Text = "Хаю хай",
            //        Name = "TB1",
            //        Margin = myThickness,
            //        HorizontalAlignment = HorizontalAlignment.Center
            //    };

            //    Stackos.Children.Add(textBlock1);
            //}
        }

        private void PreLoadInfoProfile(int id_user)
        {
            

            

            query = "SELECT accounts.surname, accounts.name, accounts.middle_name, accounts.phone_number, accounts.email,  doctors.special, doctors.experience, doctors.education FROM accounts " +
                "JOIN doctors ON accounts.id_account = doctors.id_account_fk " +
                "WHERE accounts.id_account = '"+id_user+"'";

            command = new MySqlCommand(query, databaseConnection);

            try
            {
                // Open the database
                databaseConnection.Open();

                // Execute the query
                reader = command.ExecuteReader();

                if (reader.Read())
                {
                     UserSurname.Text = reader.GetString(0);
                     UserName.Text = reader.GetString(1);
                     UserMid_Name.Text = reader.GetString(2);
                     UserPhone.Text = reader.GetString(3);
                     UserEmail.Text = reader.GetString(4);
                     UserSpecial.Text = reader.GetString(5);
                     UserExperience.Text = reader.GetString(6);
                     UserEducation.Text = reader.GetString(7);
                   
                   
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("TocenInID:  " + ex.Message);
            }
            finally
            {
                databaseConnection.Close();
            }


            
        }

        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                UserImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(imagePath));
             

                // Загрузить изображение с новым именем
                UploadImage(imagePath);


                
            }
        }

        private void UploadImage(string imagePath)
        {
            
            using (WebClient client = new WebClient())
            {
                string url = "http://localhost/Pressure/rename_image.php";

                byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                string base64Image = Convert.ToBase64String(imageBytes);
                int id_user = connector.TokenInID();
                string directory = "USER_"+id_user+"/";
                // Формирование POST запроса
                System.Collections.Specialized.NameValueCollection formData = new System.Collections.Specialized.NameValueCollection();
                formData["image"] = base64Image;
                formData["text"] = directory;

                // Отправка запроса
                byte[] responseBytes = client.UploadValues(url, formData);
                string response = System.Text.Encoding.UTF8.GetString(responseBytes);
                
            }
           
        }

        private async void UploadCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                //MessageBox.Show("Файл успешно загружен на сервер.");
               // var responseContent = await response.Content.ReadAsStringAsync();
               // MessageBox.Show(responseContent);
                string response = Encoding.UTF8.GetString(e.Result);
        

                string imageUrl = response; // Предполагается, что сервер возвращает URL загруженного изображения
                
               Dispatcher.Invoke(() => { UserImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(imageUrl)); });
                
            }
            else
            {
                MessageBox.Show("Ошибка при загрузке изображения на сервер: " + e.Error.Message);
            }
        }

        private void LoadImageFromUrl(string imageUrl)
        {
            try
            {
                // Создаем объект WebClient для загрузки изображения
                WebClient webClient = new WebClient();

                // Загружаем данные изображения
                byte[] imageData = webClient.DownloadData(imageUrl);

                // Создаем объект BitmapImage и загружаем в него данные изображения
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(imageData);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                // Устанавливаем изображение в элемент Image
                UserImage.Source = bitmap;
                //UserImage.Stretch = Stretch.UniformToFill;
            }
            catch (Exception ex)
            {
               // MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
            }
        }

        private void ToggleButton_MouseEnter(object sender, MouseEventArgs e)
        {
            poputs.IsOpen = true;
        }

        private void ToggleButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!poputs.IsMouseOver && !toggleButton.IsMouseOver )
                poputs.IsOpen = false;
        }

      

        private void ListBox_MouseEnter(object sender, MouseEventArgs e)
        {
            poputs.StaysOpen = true;
        }

        private void ListBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!toggleButton.IsMouseOver && !poputs.IsMouseOver)
            {
                poputs.StaysOpen = false;
                poputs.IsOpen = false;
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получаем ListBox, который вызвал событие
            ListBox listBox = sender as ListBox;

            // Проверяем, выбран ли какой-либо элемент
            if (listBox.SelectedItem != null)
            {
                //// Получаем выбранный элемент
               ListBoxItem selectedItems = listBox.SelectedItem as ListBoxItem;

                //// Делаем что-то с выбранным элементом, например, выводим его содержимое
                //MessageBox.Show($"Выбран элемент: {selectedItem.Content}");

               string selectedItem = (string)selectedItems.Content;

                
                // Вызовите нужную функцию в зависимости от выбранного элемента
                switch (selectedItem)
                {
                    case "Печать":
                        
                        PrintButton_Click(sender,e);
                        break;
                    case "Элемент 2":
                       // Function2();
                        break;
                    case "Элемент 3":
                       // Function3();
                        break;
                    default:
                        // Обработка других элементов, если необходимо
                        break;
                }

                listBox.SelectedItem = null;
                // Закрываем выпадающий список
                poputs.IsOpen = false;
            }
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            // Создаем объект PrintDialog
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                // Получаем PrintTicket (настройки печати) из диалога
                // PrintDialog.PrintTicket printTicket = printDialog.PrintTicket;
              
                ProfilView profileUserControl = new ProfilView();

                profileUserControl.StackButtonSetting.Children.Clear(); 
                profileUserControl.DaleteButton.Children.Clear();
                printDialog.PrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4);

                // Создаем объект FixedDocument и добавляем в него страницу из UserControl
                FixedDocument fixedDoc = new FixedDocument();
                PageContent pageContent = new PageContent();
                FixedPage fixedPage = new FixedPage();

                fixedPage.Width = printDialog.PrintTicket.PageMediaSize.Width ?? 793.7; // Размер по умолчанию для A4
                fixedPage.Height = printDialog.PrintTicket.PageMediaSize.Height ?? 1122.52; // Размер по умолчанию для A4

                // Добавляем UserControl на страницу печати
                profileUserControl.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                profileUserControl.Arrange(new Rect(profileUserControl.DesiredSize));
                fixedPage.Children.Add(profileUserControl);

                // Устанавливаем позицию элемента на странице
                Canvas.SetLeft(profileUserControl, (fixedPage.Width - profileUserControl.ActualWidth) / 2);
                Canvas.SetTop(profileUserControl, (fixedPage.Height - profileUserControl.ActualHeight) / 2);

                ((IAddChild)pageContent).AddChild(fixedPage);
                fixedDoc.Pages.Add(pageContent);

                //fixedPage.Width = profileUserControl.ActualWidth;
                //fixedPage.Height = profileUserControl.ActualHeight;
                //fixedPage.Children.Add(profileUserControl);
                //((IAddChild)pageContent).AddChild(fixedPage);
                //fixedDoc.Pages.Add(pageContent);


                // Устанавливаем PrintTicket для документа
                fixedDoc.PrintTicket = printDialog.PrintTicket;

                // Печатаем документ
                printDialog.PrintDocument(fixedDoc.DocumentPaginator, "Печать UserControl");
            }
        }

       

        private void RedactInfo(object sender, RoutedEventArgs e)
        {

            foreach (var control in ProfileFIO.Children.OfType<TextBox>())
            {
                control.BorderThickness = new Thickness(1); // Установить новое значение BorderThickness
                control.IsReadOnly = false;
                control.Width = 200;
            }

            UserSpecial.BorderThickness = new Thickness(1); // Установить новое значение BorderThickness
            UserSpecial.IsReadOnly = false;
            UserSpecial.Width = 200;

            UserExperience.BorderThickness = new Thickness(1); // Установить новое значение BorderThickness
            UserExperience.IsReadOnly = false;
            UserExperience.Width = 200;

            UserEducation.BorderThickness = new Thickness(1); // Установить новое значение BorderThickness
            UserEducation.IsReadOnly = false;
            UserEducation.Width = 450;
            UserEducation.Height = 50;


            //UserGender.Visibility = Visibility.Collapsed;
            //UserGenderCB.Visibility = Visibility.Visible;

            ExitAccountButton.Visibility = Visibility.Collapsed;

            PrimRedacktInfo.Visibility = Visibility.Visible;
            UnPrimRedacktInfo.Visibility = Visibility.Visible;

        }

        private void Exit_Account(object sender, RoutedEventArgs e)
        {

            query = "DELETE FROM tokens WHERE tokens.token_value = '" + token + "' ";

            command = new MySqlCommand(query, databaseConnection);

            try
            {
                // Open the database
                databaseConnection.Open();

                // Execute the query
                reader = command.ExecuteReader();
                reader.Close();

                Window namedWindow = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "MainWindower");

                AuthWindow authWindow = new AuthWindow();

                authWindow.Show();
                namedWindow.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("TocenInID:  " + ex.Message);
            }
            finally
            {
                databaseConnection.Close();
            }
        }

        private void UdapteInfoProfile(object sender, RoutedEventArgs e)
        {
            foreach (var control in ProfileFIO.Children.OfType<TextBox>())
            {
                control.BorderThickness = new Thickness(0); // Установить новое значение BorderThickness
                control.IsReadOnly = true;
                control.Width = double.NaN;
            }

            UserSpecial.BorderThickness = new Thickness(0); // Установить новое значение BorderThickness
            UserSpecial.IsReadOnly = true;
            UserSpecial.Width = double.NaN;

            UserExperience.BorderThickness = new Thickness(0); // Установить новое значение BorderThickness
            UserExperience.IsReadOnly = true;
            UserExperience.Width = double.NaN;

            UserEducation.BorderThickness = new Thickness(0); // Установить новое значение BorderThickness
            UserEducation.IsReadOnly = true;
            UserEducation.Width = 450;

            //UserGender.Text = ((ComboBoxItem)UserGenderCB.SelectedItem).Content.ToString();

            //UserGender.Visibility = Visibility.Visible;
            //UserGenderCB.Visibility = Visibility.Collapsed;

            ExitAccountButton.Visibility = Visibility.Visible;

            PrimRedacktInfo.Visibility = Visibility.Collapsed;
            UnPrimRedacktInfo.Visibility = Visibility.Collapsed;


            //string date = DateTime.ParseExact(UserAge.Text, "dd.MM.yyyy", null).ToString("yyyy-MM-dd");





            int Id_user = connector.TokenInID();

            query = "UPDATE accounts" +
                    " JOIN doctors ON accounts.id_account = doctors.id_account_fk " +
                    " SET accounts.surname ='" + UserSurname.Text+ "', accounts.name='" + UserName.Text+ "', accounts.middle_name='" + UserMid_Name.Text+ "', doctors.special='" + UserSpecial.Text+ "', doctors.experience='" + UserExperience.Text+ "', doctors.education = '" + UserEducation.Text + "' " +
                    "WHERE accounts.id_account='" + connector.TokenInID() + "' ";



            command = new MySqlCommand(query, databaseConnection);

            try
            {
                // Open the database
                databaseConnection.Open();

                // Execute the query
                reader = command.ExecuteReader();
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Тутатам: "+ex.Message);
            }
            finally
            {
                databaseConnection.Close();
            }
        }

        private void UnUdapteInfoProfile(object sender, RoutedEventArgs e)
        {
            foreach (var control in ProfileFIO.Children.OfType<TextBox>())
            {
                control.BorderThickness = new Thickness(0); // Установить новое значение BorderThickness
                control.IsReadOnly = true;
                control.Width = double.NaN;
            }

            UserSpecial.BorderThickness = new Thickness(0); // Установить новое значение BorderThickness
            UserSpecial.IsReadOnly = true;
            UserSpecial.Width = double.NaN;

            UserExperience.BorderThickness = new Thickness(0); // Установить новое значение BorderThickness
            UserExperience.IsReadOnly = true;
            UserExperience.Width = double.NaN;

            UserEducation.BorderThickness = new Thickness(0); // Установить новое значение BorderThickness
            UserEducation.IsReadOnly = true;
            UserEducation.Width = 450;

            //UserGender.Visibility = Visibility.Visible;
            //UserGenderCB.Visibility = Visibility.Collapsed;


            ExitAccountButton.Visibility = Visibility.Visible;

            PrimRedacktInfo.Visibility = Visibility.Collapsed;
            UnPrimRedacktInfo.Visibility = Visibility.Collapsed;
        }

       
    }
}
