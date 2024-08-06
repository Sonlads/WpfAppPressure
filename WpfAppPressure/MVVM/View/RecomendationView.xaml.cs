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
using WpfAppPressure.DBConnect;

namespace WpfAppPressure.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для RecomendationView.xaml
    /// </summary>
    public partial class RecomendationView : UserControl
    {
        private string query = null;
        static private string MySQLConnect = "server=127.0.0.1; uid=sonlads; pwd=SD33bn55_; database=pressure;";
        private MySqlConnection databaseConnection = new MySqlConnection(MySQLConnect);
        private MySqlDataReader reader = null;
        private MySqlCommand command = null;
        private DBConnector connector = new DBConnector();
        private int Id_patient = -1;

        private int otcuda = 0;
        public RecomendationView(int id_patient ,int otuda)
        {
            otcuda = otuda;

            Id_patient = id_patient;
            InitializeComponent();

            
        }
        public RecomendationView()
        {
            InitializeComponent();
        }


        private MainWindow mainWindow = (MainWindow)Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "MainWindower");

        private RecomendationWindow window = (RecomendationWindow)Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "RecomendationWindower");

        private void Enter_Recom(object sender, RoutedEventArgs e)
        {
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            int id_user = connector.TokenInID();

            int id_patient = -1;

            string RecomText = RecomBox.Text;

            string Doctor ="";

            query = "SELECT id_patient FROM patients " +
                    
                    "WHERE id_account_fk = '" + Id_patient + "'";


            command = new MySqlCommand(query, databaseConnection);

            try
            {
                // Open the database
                databaseConnection.Open();

                // Execute the query
                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    id_patient = reader.GetInt32(0);
                }

                    reader.Close();

                query = "SELECT accounts.surname, accounts.name, accounts.middle_name, doctors.special FROM accounts " +
                "JOIN doctors ON accounts.id_account = doctors.id_account_fk " +
                "WHERE accounts.id_account = '" + id_user + "'";
              

                command = new MySqlCommand(query, databaseConnection);

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                   Doctor = reader.GetString(3) +" "+ reader.GetString(0)+" " + reader.GetString(1)+" "+ reader.GetString(2);
                }

                reader.Close();
     

                query = "INSERT INTO recommendations (id_recommendation, id_patient_fk, recom_text, doctor_info, recom_datetime) VALUES (NULL, '" + id_patient + "', '" + RecomText+ "', '" + Doctor+ "', '" + currentTime + "');";

                command = new MySqlCommand(query, databaseConnection);

                reader = command.ExecuteReader();
                reader.Close();
                if (otcuda == 1)
                {
                    mainWindow.Profil_Patient(Id_patient);
                }
                window.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                databaseConnection.Close();
            }

        }

        private void Exit_Recom(object sender, RoutedEventArgs e)
        {
            window.Close();
        }
    }
}
