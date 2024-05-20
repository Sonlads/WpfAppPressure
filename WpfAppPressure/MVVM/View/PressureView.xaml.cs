using MySql.Data.MySqlClient;
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
using System.Windows.Threading;
using WpfAnimatedGif;
using WpfAppPressure.DBConnect;


namespace WpfAppPressure.MVVM.View
{
    

    /// <summary>
    /// Логика взаимодействия для PressureView.xaml
    /// </summary>
    public partial class PressureView : UserControl
    {
        private bool indicator = false;
        private DispatcherTimer _timer;




        private string query = null;
        static private string MySQLConnect = "server=127.0.0.1; uid=sonlads; pwd=SD33bn55_; database=pressure;";
        private MySqlConnection databaseConnection = new MySqlConnection(MySQLConnect);
        private MySqlDataReader reader = null;
        private MySqlCommand command = null;
        private DBConnector connector = new DBConnector();

        public PressureView()
        {
            InitializeComponent();
            
        }

        private void BeginIzmerDavlen(object sender, RoutedEventArgs e)
        {
            StartTimer();
            GifImage.Visibility = Visibility.Visible;
            LoadGif();

            WrapIzmerDavlen.Visibility = Visibility.Visible;
            IzmerDavlen.Text = "Идет измерение давления";
            IzmerDavlen.FontSize = 16;

        }

       

        private void StartTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(0); // нужный интервал времени
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop(); // Остановить таймер после первого срабатывания
            IzmerDavlenia();
            GifImage.Visibility = Visibility.Collapsed;

           
        }
        private void IzmerDavlenia()
        {
            int id_user = connector.TokenInID();

            Random random = new Random();

            double CAD = 0;
            double DAD = 0;


            //query = "SELECT DATE_FORMAT(patients.age,'%Y.%m.%d') AS formatted_date,patients.gender, patients.weight, patients.height  FROM accounts " +
            //   "JOIN patients ON accounts.id_account = patients.id_account_fk " +
            //   "WHERE accounts.id_account = '" + id_user + "'";

            //command = new MySqlCommand(query, databaseConnection);

            //try
            //{
            //    // Open the database
            //    databaseConnection.Open();

            //    // Execute the query
            //    reader = command.ExecuteReader();

            //    if (reader.Read())
            //    {
            //        int age = DateTime.Today.Year - reader.GetDateTime(0).Year;

            //        if (reader.GetDateTime(0) > DateTime.Today.AddYears(-age))
            //        {
            //            age--;
            //        }



            //        //if (reader.GetInt32(1) == 0)
            //        //{
            //        //    CAD = random.Next(70, 100) + (0.5 * age) + (0.1 * reader.GetDouble(2));
            //        //    DAD = random.Next(50, 60) + (0.1 * age) + (0.15 * reader.GetDouble(2));
            //        //}
            //        //else if (reader.GetInt32(1) == 1)
            //        //{
            //        //    CAD = random.Next(70, 100) + (0.7 * age) + (0.15 * reader.GetDouble(2));
            //        //    DAD = random.Next(50, 60) + (0.17 * age) + (0.1 * reader.GetDouble(2));
            //        //}
            //    }
            //}
            //catch (Exception ex) 
            //{
            //    Console.WriteLine(ex.ToString());
            //}
            //finally
            //{
            //    databaseConnection.Close();
            //}
            int urovnyatel = random.Next(0, 50);

            CAD = random.Next(70, 100) + urovnyatel;
            DAD = random.Next(50, 60) + urovnyatel;
            

            double CpAD = (2.0 * DAD + CAD) / 3.0;

            IzmerDavlen.Text = " Результаты измерения:" + "\n\n Cистолическое артериальное давление - " + Convert.ToInt32(CAD) + "\n\n Диастолическое артериальное давление - " + Convert.ToInt32(DAD) + "\n\n Среднее артериальное давление - " + Convert.ToInt32(CpAD);
            IzmerDavlen.FontSize = 20;

            connector.RezultIzmerDavlen( Convert.ToInt32(CAD), Convert.ToInt32(DAD), Convert.ToInt32(CpAD));

            }

        private void DopInfoOpen(object sender, MouseButtonEventArgs e)
        {
            if (sender is WrapPanel wrap)
            {
                string WrapName = wrap.Name;
                TextBlock correspondingTextBlock = null;
                Image correspondingImage = null;
                BitmapImage imageDown = new BitmapImage(new Uri("/Images/Icons/iconDown.png", UriKind.Relative));
                BitmapImage imageRight = new BitmapImage(new Uri("/Images/Icons/iconsRight.png", UriKind.Relative));


                // Определение соответствующего TextBlock для каждого Label
                switch (WrapName)
                {
                    case "Wrap1":
                        correspondingTextBlock = TextBlock1;
                        correspondingImage = Image1;
                        break;
                    case "Wrap2":
                        correspondingTextBlock = TextBlock2;
                        correspondingImage = Image2;
                        break;
                    case "Wrap3":
                        correspondingTextBlock = TextBlock3;
                        correspondingImage = Image3;
                        break;

                }

                if (correspondingTextBlock != null)
                {
                    // Переключение видимости TextBlock
                    correspondingTextBlock.Visibility =
                        correspondingTextBlock.Visibility == Visibility.Visible
                        ? Visibility.Collapsed
                        : Visibility.Visible;



                }
                if (indicator)
                {
                    correspondingImage.Source = imageRight;
                    indicator = !indicator;
                }
                else
                {
                    correspondingImage.Source = imageDown;
                    indicator = !indicator;
                }



            }




            //MyStackPanel.UpdateLayout();

            //MyScrollViewer.UpdateLayout();

            //MyScrollViewer.ScrollToEnd();
        }

        private void LoadGif()
        {
            // Укажите путь к вашему GIF-файлу
            
            var image = new BitmapImage(new Uri("/Images/Icons/iconLoading.gif", UriKind.Relative));

            // Использование библиотеки WpfAnimatedGif для установки источника изображения
            ImageBehavior.SetAnimatedSource(GifImage, image);
        }
    }
}
