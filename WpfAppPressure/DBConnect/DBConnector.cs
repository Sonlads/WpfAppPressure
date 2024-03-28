using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace WpfAppPressure.DBConnect
{
    public class DBConnector
    {
        private string query = null;
        static private string MySQLConnect = "datasource=127.0.0.1;port=3306;username=sonlads;password=SD33bn55_;database=pressure;";
        private MySqlConnection databaseConnection = new MySqlConnection(MySQLConnect);
        private MySqlDataReader reader = null;
        private MySqlCommand command = null;

        public DBConnector() 
        {
           // DBConnectors();
            Console.WriteLine("/////////Обращение в класс взаимоджействия с БД //////////");
        }

        static string ComputeSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        //private void DBConnectors( )
        //{
        //    try
        //    {
               
        //        databaseConnection = new MySqlConnection(MySQLConnect);

        //        Console.WriteLine("Подключило к БД");

                
        //    }
        //    catch
        //    {
        //        Console.WriteLine("Не подключило к БД");

               
        //    }
            
        //}

        public bool TokenCheck(string token)
        {
            query = "SELECT * FROM tokens WHERE token_value='" + token + "' ";
            command = new MySqlCommand(query, databaseConnection);

            try
            {
                // Open the database
                databaseConnection.Open();

                // Execute the query
                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    databaseConnection.Close();
                    return true;
                }
                else
                {
                    databaseConnection.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Show any error message.
                Console.WriteLine("TokenCheck: "+ex.Message);

                return false;
            }
        }

        private void TokenCreate(int id_user, string tokeno)
        {

            tokeno = ComputeSHA256Hash(tokeno.Substring(0, tokeno.IndexOf('@')));

            

            Random res = new Random();

            // String of alphabets  
            string str = "abcdefghijklmnopqrstuvwxyz0123456789";


            // Initializing the empty string 
            string ran = "";

            for (int i = 0; i < (255 - tokeno.Length); i++)
            {

                // Selecting a index randomly 
                int x = res.Next(str.Length);

                // Appending the character at the  
                // index to the random string. 
                ran = ran + str[x];
            }

            tokeno = ran.Insert(res.Next(ran.Length), tokeno);

            


            query = "INSERT INTO tokens (id_token, id_account_fk, token_value) VALUES (NULL, '"+id_user+"', '"+tokeno+"');";

            command = new MySqlCommand(query, databaseConnection);

            try
            {
                // Open the database
                databaseConnection.Open();

                // Execute the query
                reader = command.ExecuteReader();
                
                databaseConnection.Close();

                Properties.Settings.Default.token = tokeno;
                Properties.Settings.Default.Save();

            }
            catch (Exception ex)
            {
                // Show any error message.
                Console.WriteLine("TokenCreate:  "+ex.Message);

               
            }

        }

        public bool BDCommandAuth(string email, string password )
        {
            password = ComputeSHA256Hash(password);

            query = "SELECT id_account, email, password FROM accounts WHERE email='"+email+"' AND password='"+password+"' ";

            command = new MySqlCommand(query, databaseConnection);

            try
            {
                // Open the database
                databaseConnection.Open();

                // Execute the query
                reader = command.ExecuteReader();

                

                if (reader.HasRows)
                {
                    int Id_User = -1;
                    while (reader.Read())
                    {
                        Id_User = reader.GetInt32(0);
                    }

                  //  databaseConnection.Close();


                    TokenCreate(Id_User, email);

                    

                    return true;
                }
                else
                {
                    Console.WriteLine("No rows found.");
                    //databaseConnection.Close();

                    return false;
                }

            }
            catch (Exception ex)
            {
                // Show any error message.
                Console.WriteLine("Auth:  "+ex.Message);

                return false;
            }
            finally
            {
                databaseConnection.Close();    
            }

           
        }

        public void DBCommandRegister(string UserEmail, string UserPhone, string UserPassword, string UserSurname, string UserName, string MiddleName, DateTime UserBirthday, int Weight, int Height)
        {

            try
            {
                UserPassword = ComputeSHA256Hash(UserPassword);

                query = "INSERT INTO accounts (id_account, email, password, phone_number, surname, name, middle_name ) VALUES (NULL, '" + UserEmail + "', '" + UserPassword + "' , '" + UserPhone + "', '" + UserSurname + "' );";

                command = new MySqlCommand(query, databaseConnection);
            }
            catch
            {

            }
            finally
            {
                databaseConnection.Ping();
            }
            

        }

        

    }
}
