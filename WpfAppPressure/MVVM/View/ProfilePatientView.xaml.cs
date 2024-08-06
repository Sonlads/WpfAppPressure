using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAppPressure.DBConnect;

namespace WpfAppPressure.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для ProfilePatientView.xaml
    /// </summary>
    public partial class ProfilePatientView : UserControl
    {
        private string token = Properties.Settings.Default.token;

        private List<string> DoctorList = new List<string>();
        private List<string> DateList = new List<string>();
        private List<string> RecomentList = new List<string>();

        private string query = null;
        static private string MySQLConnect = "server=127.0.0.1; uid=sonlads; pwd=SD33bn55_; database=pressure;";
        private MySqlConnection databaseConnection = new MySqlConnection(MySQLConnect);
        private MySqlDataReader reader = null;
        private MySqlCommand command = null;
        private DBConnector connector = new DBConnector();
        private int id_user = -1;
        private int Otcuda = -1;

        public ProfilePatientView(int id_patient, int otcuda = 0)
        {
            id_user = id_patient;

            Otcuda = otcuda;

            InitializeComponent();

            //int id_user = connector.TokenInID();

            PreLoadInfoProfile(id_user);

            string UsersProfils = "USER_" + id_user + "/";

            string imageUrl = "http://localhost/Pressure/uploads/" + UsersProfils + "default.png";

            LoadImageFromUrl(imageUrl);

           
        }

        public ProfilePatientView()
        {
            InitializeComponent();

            //int id_user = connector.TokenInID();

            PreLoadInfoProfile(id_user);

            string UsersProfils = "USER_" + id_user + "/";

            string imageUrl = "http://localhost/Pressure/uploads/" + UsersProfils + "default.png";

            LoadImageFromUrl(imageUrl);

           
        }


        private void PreLoadInfoProfile(int id_user)
        {




            query = "SELECT accounts.surname, accounts.name, accounts.middle_name, accounts.phone_number, accounts.email, DATE_FORMAT(patients.age,'%d.%m.%Y') AS formatted_date, patients.weight, patients.height, patients.gender, patients.id_patient FROM accounts " +
                "JOIN patients ON accounts.id_account = patients.id_account_fk " +
                "WHERE accounts.id_account = '" + id_user + "'";

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
                    UserAge.Text = reader.GetString(5);
                    UserWidth.Text = reader.GetString(6);
                    UserHeight.Text = reader.GetString(7);

                    if (reader.GetInt32(8) == 1)
                        UserGender.Text = "Женский";
                    else if (reader.GetInt32(8) == 0)
                        UserGender.Text = "Мужчина";

                }
                reader.Close();


            }
            catch (Exception ex)
            {
                Console.WriteLine("TocenInID:  " + ex.Message);
            }
            finally
            {
                databaseConnection.Close();
            }

            query = "SELECT doctor_info, recom_text, DATE_FORMAT(recom_datetime,'%d.%m.%Y %H:%i:%s' ) AS formatted_datetime  FROM recommendations " +
                    "JOIN patients ON id_patient=id_patient_fk " +
                    "JOIN accounts ON id_account=id_account_fk " +
                    "WHERE id_account = '" + id_user + "'";

            command = new MySqlCommand(query, databaseConnection);

            try
            {
                // Open the database
                databaseConnection.Open();

                // Execute the query
                reader = command.ExecuteReader();


                DoctorList = new List<string>();

                DateList = new List<string>();

                RecomentList = new List<string>();



                while (reader.Read())
                {
                    //customAxisLabels[index] = reader.GetString(0);

                    DoctorList.Add(reader.GetString(0));
                    RecomentList.Add(reader.GetString(1));
                    DateList.Add(reader.GetString(2));


                    // index++;
                }
                reader.Close();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                databaseConnection.Close();

            }


            for (int i = DoctorList.Count-1; i >= 0; i--)
            {
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Vertical;
                stackPanel.Margin = new Thickness(10);
                stackPanel.HorizontalAlignment = HorizontalAlignment.Stretch;


                Grid grid = new Grid();
                grid.Margin = new Thickness(5);

                Border borderDopInfo = new Border();
                borderDopInfo.Margin = new Thickness(5);
                borderDopInfo.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                //borderDopInfo.Style = (Style)ProfilePatientControlos.Resources["StackPanelShadow"];
                borderDopInfo.BorderThickness = new Thickness(1);
                borderDopInfo.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                borderDopInfo.CornerRadius = new CornerRadius(10, 10, 10, 10);




                System.Windows.Controls.Label label = new System.Windows.Controls.Label();
                label = new System.Windows.Controls.Label();
                label.Content = $"{DoctorList[i]}";
                label.FontSize = 14;
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.FontWeight = FontWeights.Bold;
                grid.Children.Add(label);

                label = new System.Windows.Controls.Label();
                label.Content = $"{DateList[i]}";
                label.FontSize = 14;
                label.HorizontalAlignment = HorizontalAlignment.Right;
                label.Margin = new Thickness(50, 0, 0, 0);
                grid.Children.Add(label);



                stackPanel.Children.Add(grid);

                TextBlock textBlock = new TextBlock();
                textBlock.Text = $"{RecomentList[i]}";
                textBlock.FontSize = 14;
                textBlock.Margin = new Thickness(10, 5, 0, 10);
                textBlock.TextWrapping = TextWrapping.Wrap;
                stackPanel.Children.Add(textBlock);


                // Добавляем StackPanel в WrapPanel
                borderDopInfo.Child = stackPanel;
                Recomentor.Children.Add(borderDopInfo);
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
                string directory = "USER_" + id_user + "/";
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

        //private void ToggleButton_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    poputs.IsOpen = true;
        //}

        //private void ToggleButton_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    if (!poputs.IsMouseOver && !toggleButton.IsMouseOver)
        //        poputs.IsOpen = false;
        //}



        //private void ListBox_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    poputs.StaysOpen = true;
        //}

        //private void ListBox_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    if (!toggleButton.IsMouseOver && !poputs.IsMouseOver)
        //    {
        //        poputs.StaysOpen = false;
        //        poputs.IsOpen = false;
        //    }
        //}

        //private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    // Получаем ListBox, который вызвал событие
        //    ListBox listBox = sender as ListBox;

        //    // Проверяем, выбран ли какой-либо элемент
        //    if (listBox.SelectedItem != null)
        //    {
        //        //// Получаем выбранный элемент
        //        ListBoxItem selectedItems = listBox.SelectedItem as ListBoxItem;

        //        //// Делаем что-то с выбранным элементом, например, выводим его содержимое
        //        //MessageBox.Show($"Выбран элемент: {selectedItem.Content}");

        //        string selectedItem = (string)selectedItems.Content;


        //        // Вызовите нужную функцию в зависимости от выбранного элемента
        //        switch (selectedItem)
        //        {
        //            case "Печать":

        //                PrintButton_Click(sender, e);
        //                break;
        //            case "Элемент 2":
        //                // Function2();
        //                break;
        //            case "Элемент 3":
        //                // Function3();
        //                break;
        //            default:
        //                // Обработка других элементов, если необходимо
        //                break;
        //        }

        //        listBox.SelectedItem = null;
        //        // Закрываем выпадающий список
        //        poputs.IsOpen = false;
        //    }
        //}

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

            UserHeight.BorderThickness = new Thickness(1); // Установить новое значение BorderThickness
            UserHeight.IsReadOnly = false;
            UserHeight.Width = 50;

            UserWidth.BorderThickness = new Thickness(1); // Установить новое значение BorderThickness
            UserWidth.IsReadOnly = false;
            UserWidth.Width = 50;

            UserAge.BorderThickness = new Thickness(1); // Установить новое значение BorderThickness
            UserAge.IsReadOnly = false;
            UserAge.Width = 100;


            UserGender.Visibility = Visibility.Collapsed;
            UserGenderCB.Visibility = Visibility.Visible;

            

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

            UserHeight.BorderThickness = new Thickness(0); // Установить новое значение BorderThickness
            UserHeight.IsReadOnly = true;
            UserHeight.Width = double.NaN;

            UserWidth.BorderThickness = new Thickness(0); // Установить новое значение BorderThickness
            UserWidth.IsReadOnly = true;
            UserWidth.Width = double.NaN;

            UserAge.BorderThickness = new Thickness(0); // Установить новое значение BorderThickness
            UserAge.IsReadOnly = true;
            UserAge.Width = double.NaN;

            UserGender.Text = ((ComboBoxItem)UserGenderCB.SelectedItem).Content.ToString();

            UserGender.Visibility = Visibility.Visible;
            UserGenderCB.Visibility = Visibility.Collapsed;

           


            string date = DateTime.ParseExact(UserAge.Text, "dd.MM.yyyy", null).ToString("yyyy-MM-dd");





            int Id_user = connector.TokenInID();

            query = "UPDATE accounts" +
                    " JOIN patients ON accounts.id_account = patients.id_account_fk " +
                    " SET accounts.surname ='" + UserSurname.Text + "', accounts.name='" + UserName.Text + "', accounts.middle_name='" + UserMid_Name.Text + "',  patients.age='" + date + "', patients.weight='" + UserWidth.Text + "', patients.height='" + UserHeight.Text + "', patients.gender = '" + UserGenderCB.SelectedIndex.ToString() + "' " +
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
                Console.WriteLine(" Тутатам: " + ex.Message);
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

            UserHeight.BorderThickness = new Thickness(0); // Установить новое значение BorderThickness
            UserHeight.IsReadOnly = true;
            UserHeight.Width = double.NaN;

            UserWidth.BorderThickness = new Thickness(0); // Установить новое значение BorderThickness
            UserWidth.IsReadOnly = true;
            UserWidth.Width = double.NaN;

            UserAge.BorderThickness = new Thickness(0); // Установить новое значение BorderThickness
            UserAge.IsReadOnly = true;
            UserAge.Width = double.NaN;

            UserGender.Visibility = Visibility.Visible;
            UserGenderCB.Visibility = Visibility.Collapsed;


           
        }

        private void RecomendButton_Click(object sender, RoutedEventArgs e)
        {
            RecomendationWindow recomendationWindow = new RecomendationWindow(id_user,1);
            recomendationWindow.Show();
        }
        private MainWindow window = (MainWindow)Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "MainWindower");
        private void Back_Search(object sender, RoutedEventArgs e)
        {
            

            int id_history = window.HistoryViewRemove();

            if (id_history > 0)
            {
                
                    window.Monitor_Patient(id_user);
                
                    
            }
            else
            {
                window.Is_Checked(3);
            }

        }

        private void MonitorPatient_Click(object sender, RoutedEventArgs e)
        {
            window.Monitor_Patient(id_user);
            window.HistoryViewAdd(2);
        }
    }
}
