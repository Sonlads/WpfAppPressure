
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
using MySql.Data.MySqlClient;
using WpfAppPressure.DBConnect;
using System.Collections;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.VisualElements;
using System.Reflection.Emit;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.WPF;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;






namespace WpfAppPressure.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для MonitoringView.xaml
    /// </summary>
    public partial class MonitoringView : UserControl
    {



        private string query = null;
        static private string MySQLConnect = "server=127.0.0.1; uid=sonlads; pwd=SD33bn55_; database=pressure;";
        private MySqlConnection databaseConnection = new MySqlConnection(MySQLConnect);
        private MySqlDataReader reader = null;
        private MySqlCommand command = null;
        private DBConnector connector = new DBConnector();

        List<string> customAxisLabels = new List<string>() { "5" };

        List<double> pressvalues = new List<double> { 10};
        List<double> cpressvalues = new List<double> { 10};
        List<double> dpressvalues = new List<double> { 10};

        List<int> MaxPressValue = new List<int> { 10};
        List<int> MinPressValue = new List<int> { 10};
        List<int> NormalPressValue = new List<int> { 10 };

        public MonitoringView()
        {
            InitializeComponent();

            DataContext = this;

            // Создаем коллекцию серий

            // Создаем серию данных

            // Настройка пользовательских подписей для оси X

            Click_DaysDefoult();

            //Настройка подписей оси X
            //Labels = new[] { "Jan", "Feb", "Mar", "Apr" };
            //Настройка разделителей оси X
            //Separator = new LiveCharts.Wpf.Separator
            //{
            //    Step = 1,
            //    IsEnabled = false
            //};

        }

        //public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }

        public RectangularSection[] _Sections { get; set; } =
    {
        new RectangularSection
        {
            Yi = 80,
            Yj = 80,
            Stroke = new SolidColorPaint
            {
                Color = SKColors.Gray,
                StrokeThickness = 2,
                PathEffect = new DashEffect(new float[] { 6, 6 })
            }
        },
        new RectangularSection
        {
            Yi = 110,
            Yj = 110,
            Stroke = new SolidColorPaint
            {
                Color = SKColors.Gray,
                StrokeThickness = 2,
                PathEffect = new DashEffect(new float[] { 6, 6 })
            }
        }

    };

        private void CreateCharts (List<string> customAxisLabels, List<double> values)
        {
            var lineSeries = new LineSeries<double>
            {
                Values = values,
                LineSmoothness = 1,

                Fill = new SolidColorPaint()
                {
                    Color = new SKColor(100, 149, 237, 50) // Синий цвет с альфа-каналом 50 (полупрозрачный)
                },

                Stroke = new SolidColorPaint
                {
                    Color= SKColors.CornflowerBlue,
                    StrokeThickness = 4
                },
                
                GeometryStroke = new SolidColorPaint
                { 
                    Color = SKColors.CornflowerBlue, 
                    StrokeThickness = 4
                }



                //TooltipLabelFormatter = point => $"На {customAxisLabels[point.Context.Index]} значение: {point.PrimaryValue}",
                //YToolTipLabelFormatter = point => $" Значение: {point.PrimaryValue}",
                // XToolTipLabelFormatter = point => $" Дата: {customAxisLabels[point.Context.Index]} "


            };

            
    


        var xAxis = new Axis
            {
                Labels = customAxisLabels.ToArray(),
                
                Name = "Дата и время",
                NameTextSize = 16,
                TextSize = 14,
               
                

               
            };

            var yAxis = new Axis
            {
                
                Name = "Среднее артериальное давление",
                NameTextSize = 16,
                TextSize= 14,
               
                
            };


           

            // Добавляем секцию к графику
            MyCartesianChart.Sections = _Sections;

            // Привязываем данные к графику
            MyCartesianChart.Series = new ISeries[] { lineSeries };
           
            MyCartesianChart.XAxes = new Axis[] { xAxis };
            MyCartesianChart.YAxes = new Axis[] { yAxis };


            


            if (MaxPressValue.Count > 0 && MinPressValue.Count > 0 && NormalPressValue.Count >0)
            {

              MyPieChart.Series = new ISeries[]
              {
                new PieSeries<int>
                 {
                     Values = new int [] {MaxPressValue.Count },
                     Name = "Повышенное давление",
                     Fill = new SolidColorPaint(SKColors.Red) {
                         Color = new SKColor(237, 109, 100, 250)
                     },
                      InnerRadius= 60,
                      DataLabelsPaint = new SolidColorPaint(SKColors.White) 
                      ,
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                    DataLabelsFormatter = point => $"{point.PrimaryValue}"
                 },
                
                new PieSeries<int>
                {
                     Values = new int[] { MinPressValue.Count },
                     Name = "Пониженное давление",
                     Fill = new SolidColorPaint(SKColors.CornflowerBlue)
                      {
                         Color = new SKColor(100, 149, 237, 250)
                     },
                      InnerRadius= 60,
                      DataLabelsPaint = new SolidColorPaint(SKColors.White)
                       ,
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                    DataLabelsFormatter = point => $"{point.PrimaryValue}"
                 },
                 new PieSeries<int>
                 {
                      Values = new int[] { NormalPressValue.Count },
                         Name = "нормальное давление",

                        Fill = new SolidColorPaint(SKColors.LimeGreen)
                         {
                         Color = new SKColor(100, 237, 109, 250)
                     },
                       InnerRadius= 60,
                       DataLabelsPaint = new SolidColorPaint(SKColors.White)
                       ,
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                    DataLabelsFormatter = point => $"{point.PrimaryValue}"
                  }
              };
            }
            else if (MaxPressValue.Count == 0 )
            {
                MyPieChart.Series = new ISeries[]
             {
             
                
                new PieSeries<int>
                {
                     Values = new int[] { MinPressValue.Count },
                     Name = "Пониженное давление",
                     Fill = new SolidColorPaint(SKColors.CornflowerBlue)
                      {
                         Color = new SKColor(100, 149, 237, 250)
                     },
                      InnerRadius= 60,
                      DataLabelsPaint = new SolidColorPaint(SKColors.White)
                       ,
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                    DataLabelsFormatter = point => $"{point.PrimaryValue}"
                 },
                 new PieSeries<int>
                 {
                      Values = new int[] { NormalPressValue.Count },
                         Name = "Нормальное давление",
                        Fill = new SolidColorPaint(SKColors.LimeGreen) {
                         Color = new SKColor(100, 237, 149, 250)
                     },

                      InnerRadius= 60,
                      DataLabelsPaint = new SolidColorPaint(SKColors.White)
                       ,
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                    DataLabelsFormatter = point => $"{point.PrimaryValue}"
                  }
             };
            }
            else if (MinPressValue.Count == 0)
            {

                MyPieChart.Series = new ISeries[]
                {
                new PieSeries<int>
                 {
                     Values = new int [] {MaxPressValue.Count },
                     Name = "Повышенное давление",
                     Fill = new SolidColorPaint(SKColors.Red)
                      {
                         Color = new SKColor(237, 109, 100, 250)
                     },
                     InnerRadius= 60,
                     DataLabelsPaint = new SolidColorPaint(SKColors.White)
                     ,
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                    DataLabelsFormatter = point => $"{point.PrimaryValue}"
                 },
               
                 new PieSeries<int>
                 {
                      Values = new int[] { NormalPressValue.Count },
                         Name = "нормальное давление",
                        Fill = new SolidColorPaint(SKColors.LimeGreen)
                         {
                         Color = new SKColor(100, 237, 109, 250)
                     },
                       InnerRadius= 60,
                       DataLabelsPaint = new SolidColorPaint(SKColors.White)
                        ,
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                    DataLabelsFormatter = point => $"{point.PrimaryValue}"
                  }
                };
            }
            else if (NormalPressValue.Count == 0)
            {

                MyPieChart.Series = new ISeries[]
                {
                new PieSeries<int>
                 {
                     Values = new int [] {MaxPressValue.Count },
                     Name = "Повышенное давление",

                     Fill = new SolidColorPaint(SKColors.Red)
                     {
                         Color = new SKColor(237, 109, 100, 250)
                     },
                      InnerRadius= 60,
                      DataLabelsPaint = new SolidColorPaint(SKColors.White)
                      ,
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                    DataLabelsFormatter = point => $"{point.PrimaryValue}",
                   
                 },
                new PieSeries<int>
                {
                     Values = new int[] { MinPressValue.Count },
                     Name = "Пониженное давление",
                     Fill = new SolidColorPaint()
                     {
                         Color = new SKColor(100, 149, 237, 250)
                     },
                     InnerRadius= 60,
                     DataLabelsPaint = new SolidColorPaint(SKColors.White)
                      ,
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                    DataLabelsFormatter = point => $"{point.PrimaryValue}"
                 }
                };
            }


            
           


        }

        private void CreateDopInfo(List<string> date, List<double> pv, List<double> cpv, List<double> dpv)
        {
            WrapDopInfo.Children.Clear();

            

            for (int i = 0; i < pv.Count; i++)
            {
                //Button button = new Button();
                //button.Content = $"Button {i}";
                //button.Margin = new Thickness(5);
                //button.Width = 100;
                //button.Height = 30;
                //TextBlock textBlock = new TextBlock();
                //textBlock.Margin = new Thickness(10, 10, 10, 10);
                //textBlock.Width = 150;
                //textBlock.Height = 50;
                //textBlock.Name = $"TextBlockDop{i}";

                //WrapDopInfo.Children.Add(textBlock);

                //System.Windows.Controls.Label label = new System.Windows.Controls.Label();
                //label.FontSize = 14;
                //label.Content = $"Дата измерения: {pv[i]}";

                //textBlock.Name.Children.Add(label);

                // Добавляем кнопку в WrapPanel

                // Создаем StackPanel, который будет содержать TextBlock и Label
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Vertical;
                stackPanel.Margin = new Thickness(5);
                stackPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                

                Border borderDopInfo = new Border();
                borderDopInfo.Margin = new Thickness(5);
                borderDopInfo.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255,255));
                borderDopInfo.Style= (Style)MonitorView.Resources["StackPanelShadow"];
                borderDopInfo.CornerRadius = new CornerRadius(10,10,10,10);

                // Создаем Label
                System.Windows.Controls.Label label = new System.Windows.Controls.Label();
                label.Content = $"Дата и время: {date[i]}";
                label.FontSize = 14;
                label.FontWeight = FontWeights.Bold;
                stackPanel.Children.Add(label);

                label = new System.Windows.Controls.Label();
                label.Content = $"Cист: {cpv[i]}  Диаст:  {dpv[i]}";
                label.FontSize = 14;
                stackPanel.Children.Add(label);

                label = new System.Windows.Controls.Label();
                label.Content = $"Cреднее: {pv[i]}";
                label.FontSize = 14;
                stackPanel.Children.Add(label);

                label = new System.Windows.Controls.Label();
                if (pv[i] < 110 && pv[i] > 80)
                {
                    label.Content = $"Нормальное давление ";
                    label.Foreground = new SolidColorBrush(Color.FromArgb(255, 100, 200, 100));
                }
                else if (pv[i] >= 110) 
                {

                    label.Content = $"Повышенное давление ";
                    label.Foreground = new SolidColorBrush(Color.FromArgb(255, 225, 100, 100));
                }
                else if (pv[i] <= 80)
                {
                    label.Content = $"Пониженное давление ";
                    label.Foreground = new SolidColorBrush(Color.FromArgb(255, 10, 100, 225));
                }
                else
                {
                    label.Content = $"Исключительное давление";
                    label.Foreground = new SolidColorBrush(Color.FromArgb(255, 225, 225, 225));
                }
                label.FontSize = 14;
                label.FontWeight = FontWeights.Bold;
                stackPanel.Children.Add(label);

                // Добавляем StackPanel в WrapPanel
                borderDopInfo.Child = stackPanel;
                WrapDopInfo.Children.Add(borderDopInfo);

            }
        }



        private void Click_DaysDefoult()
        {
            int id_user = connector.TokenInID();

            Days7.Opacity = 0.5;
            Days30.Opacity = 0.5;
            DaysAll.Opacity = 0.5;
            DaysToday.Opacity = 1;

           

            query = "SELECT DATE_FORMAT(datetime,'%d.%m.%Y %H:%i:%s' ) AS formatted_datetime, pressvalue, cpressvalue, dpressvalue  FROM records " +
                    "WHERE id_account_fk = '" + id_user + "' AND datetime > DATE_SUB(NOW(), INTERVAL 1 DAY)";

            command = new MySqlCommand(query, databaseConnection);

            try
            {
                // Open the database
                databaseConnection.Open();

                // Execute the query
                reader = command.ExecuteReader();


                customAxisLabels = new List<string>();

                cpressvalues = new List<double>();

                dpressvalues = new List<double>();

                pressvalues = new List<double>();

                MaxPressValue = new List<int>();

                MinPressValue = new List<int>();  

                NormalPressValue = new List<int>();

                while (reader.Read())
                {
                    //customAxisLabels[index] = reader.GetString(0);

                    customAxisLabels.Add(reader.GetString(0));
                    pressvalues.Add(reader.GetDouble(1));
                    cpressvalues.Add(reader.GetDouble(2));
                    dpressvalues.Add(reader.GetDouble(3));
                    if (reader.GetInt32(1) > 110)
                    {
                        MaxPressValue.Add(reader.GetInt32(1));
                    }
                    else if (reader.GetInt32(1)<80)
                    {
                        MinPressValue.Add(reader.GetInt32(1));
                    }
                    else
                    {
                        NormalPressValue.Add(reader.GetInt32(1));
                    }

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


            CreateCharts(customAxisLabels, pressvalues);
            CreateDopInfo(customAxisLabels, pressvalues, cpressvalues, dpressvalues);
        }

        private void Click_Days7(object sender, MouseButtonEventArgs e)
        {
            int id_user = connector.TokenInID();

            Days7.Opacity = 1;
            Days30.Opacity = 0.5;
            DaysAll.Opacity = 0.5;
            DaysToday.Opacity = 0.5;    

           

            query = "SELECT DATE_FORMAT(datetime,'%d.%m.%Y %H:%i:%s' ) AS formatted_datetime, pressvalue, cpressvalue, dpressvalue  FROM records " +
                    "WHERE id_account_fk = '" + id_user + "' AND datetime >= DATE_SUB(NOW(), INTERVAL 7 DAY)";

            command = new MySqlCommand(query, databaseConnection);

            try
            {
                // Open the database
                databaseConnection.Open();

                // Execute the query
                reader = command.ExecuteReader();


                customAxisLabels = new List<string>();

                cpressvalues = new List<double>();

                dpressvalues = new List<double>();

                pressvalues = new List<double>();

                MaxPressValue = new List<int>();

                MinPressValue = new List<int>();

                NormalPressValue = new List<int>();

                while (reader.Read())
                {
                    //customAxisLabels[index] = reader.GetString(0);

                    customAxisLabels.Add(reader.GetString(0));
                    pressvalues.Add(reader.GetDouble(1));
                    cpressvalues.Add(reader.GetDouble(2));
                    dpressvalues.Add(reader.GetDouble(3));
                    if (reader.GetInt32(1) > 110)
                    {
                        MaxPressValue.Add(reader.GetInt32(1));
                    }
                    else if (reader.GetInt32(1) < 80)
                    {
                        MinPressValue.Add(reader.GetInt32(1));
                    }
                    else
                    {
                        NormalPressValue.Add(reader.GetInt32(1));
                    }

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


            CreateCharts(customAxisLabels, pressvalues);
            CreateDopInfo(customAxisLabels, pressvalues, cpressvalues, dpressvalues);
        }
        private void Click_Days30(object sender, MouseButtonEventArgs e)
        {
            int id_user = connector.TokenInID();

            Days7.Opacity = 0.5;
            Days30.Opacity = 1;
            DaysAll.Opacity = 0.5;
            DaysToday.Opacity = 0.5;

            

            query = "SELECT DATE_FORMAT(datetime,'%d.%m.%Y %H:%i:%s' ) AS formatted_datetime, pressvalue, cpressvalue, dpressvalue  FROM records " +
                    "WHERE id_account_fk = '" + id_user + "' AND datetime >= DATE_SUB(NOW(), INTERVAL 30 DAY)";

            command = new MySqlCommand(query, databaseConnection);

            try
            {
                // Open the database
                databaseConnection.Open();

                // Execute the query
                reader = command.ExecuteReader();


                customAxisLabels = new List<string>();

                cpressvalues = new List<double>();

                dpressvalues = new List<double>();

                pressvalues = new List<double>();

                MaxPressValue = new List<int>();

                MinPressValue = new List<int>();

                NormalPressValue = new List<int>();

                while (reader.Read())
                {
                    //customAxisLabels[index] = reader.GetString(0);

                    customAxisLabels.Add(reader.GetString(0));
                    pressvalues.Add(reader.GetDouble(1));
                    cpressvalues.Add(reader.GetDouble(2));
                    dpressvalues.Add(reader.GetDouble(3));
                    if (reader.GetInt32(1) > 110)
                    {
                        MaxPressValue.Add(reader.GetInt32(1));
                    }
                    else if (reader.GetInt32(1) < 80)
                    {
                        MinPressValue.Add(reader.GetInt32(1));
                    }
                    else
                    {
                        NormalPressValue.Add(reader.GetInt32(1));
                    }

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


            CreateCharts(customAxisLabels, pressvalues);
            CreateDopInfo(customAxisLabels, pressvalues, cpressvalues, dpressvalues);

        }

            private void Click_DaysAll(object sender, MouseButtonEventArgs e)
        {
            int id_user = connector.TokenInID();

            Days7.Opacity = 0.5;
            Days30.Opacity = 0.5;
            DaysAll.Opacity = 1;
            DaysToday.Opacity = 0.5;


            

            query = "SELECT DATE_FORMAT(datetime,'%d.%m.%Y %H:%i:%s' ) AS formatted_datetime, pressvalue, cpressvalue, dpressvalue  FROM records " +
                    "WHERE id_account_fk = '" + id_user + "'";

            command = new MySqlCommand(query, databaseConnection);

            try
            {
                // Open the database
                databaseConnection.Open();

                // Execute the query
                reader = command.ExecuteReader();


                customAxisLabels = new List<string>();

                cpressvalues = new List<double>();

                dpressvalues = new List<double>();

                pressvalues = new List<double>();

                MaxPressValue = new List<int>();

                MinPressValue = new List<int>();

                NormalPressValue = new List<int>();

                while (reader.Read())
                {
                    //customAxisLabels[index] = reader.GetString(0);

                    customAxisLabels.Add(reader.GetString(0));
                    pressvalues.Add(reader.GetDouble(1));
                    cpressvalues.Add(reader.GetDouble(2));
                    dpressvalues.Add(reader.GetDouble(3));
                    if (reader.GetInt32(1) > 110)
                    {
                        MaxPressValue.Add(reader.GetInt32(1));
                    }
                    else if (reader.GetInt32(1) < 80)
                    {
                        MinPressValue.Add(reader.GetInt32(1));
                    }
                    else
                    {
                        NormalPressValue.Add(reader.GetInt32(1));
                    }

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


            CreateCharts(customAxisLabels, pressvalues);
            CreateDopInfo(customAxisLabels, pressvalues, cpressvalues, dpressvalues);
        }

        private void Click_DaysToday(object sender, MouseButtonEventArgs e)
        {
            int id_user = connector.TokenInID();

            Days7.Opacity = 0.5;
            Days30.Opacity = 0.5;
            DaysAll.Opacity = 0.5;
            DaysToday.Opacity = 1;

           

            query = "SELECT DATE_FORMAT(datetime,'%d.%m.%Y %H:%i:%s' ) AS formatted_datetime, pressvalue, cpressvalue, dpressvalue  FROM records " +
                    "WHERE id_account_fk = '" + id_user + "' AND datetime > DATE_SUB(NOW(), INTERVAL 1 DAY)";

            command = new MySqlCommand(query, databaseConnection);

            try
            {
                // Open the database
                databaseConnection.Open();

                // Execute the query
                reader = command.ExecuteReader();


                customAxisLabels = new List<string>();

                cpressvalues = new List<double>();

                dpressvalues = new List<double>();

                pressvalues = new List<double>();

                MaxPressValue = new List<int>();

                MinPressValue = new List<int>();

                NormalPressValue = new List<int>();

                while (reader.Read())
                {
                    //customAxisLabels[index] = reader.GetString(0);

                    customAxisLabels.Add(reader.GetString(0));
                    pressvalues.Add(reader.GetDouble(1));
                    cpressvalues.Add(reader.GetDouble(2));
                    dpressvalues.Add(reader.GetDouble(3));
                    if (reader.GetInt32(1) > 110)
                    {
                        MaxPressValue.Add(reader.GetInt32(1));
                    }
                    else if (reader.GetInt32(1) < 80)
                    {
                        MinPressValue.Add(reader.GetInt32(1));
                    }
                    else
                    {
                        NormalPressValue.Add(reader.GetInt32(1));
                    }

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


            CreateCharts(customAxisLabels, pressvalues);
            CreateDopInfo(customAxisLabels, pressvalues,cpressvalues,dpressvalues);
        }





        //private void AllDays()
        //{
        //    int id_user = connector.TokenInID();



        //    List<string> customAxisLabels = new List<string>() { "5", "53", "45" };

        //    List<double> values = new List<double> { 10, 20, 30, 40, 50 };

        //    query = "SELECT DATE_FORMAT(datetime,'%d.%m.%Y %H:%i:%s' ) AS formatted_datetime, pressvalue  FROM records " +
        //            "WHERE id_account_fk = '" + id_user + "'";

        //    command = new MySqlCommand(query, databaseConnection);

        //    try
        //    {
        //        // Open the database
        //        databaseConnection.Open();

        //        // Execute the query
        //        reader = command.ExecuteReader();


        //        customAxisLabels = new List<string>();



        //        values = new List<double>();

        //        while (reader.Read())
        //        {
        //            //customAxisLabels[index] = reader.GetString(0);

        //            customAxisLabels.Add(reader.GetString(0));
        //            values.Add(reader.GetDouble(1));


        //            // index++;
        //        }
        //        reader.Close();


        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //    }
        //    finally
        //    {
        //        databaseConnection.Close();

        //    }


        //    CreateCharts(customAxisLabels,values);

        //var lineSeries = new LineSeries<double>
        //{
        //    Values = new List<double> { 3, 5, 7, 9, 11 },
        //    Name = "Series 1"
        //};

        // Привязываем данные к графику
        //MyCartesianChart.Series = new ISeries[] { lineSeries };

        //var customAxisLabels = new[] { "22.12.2023 11:45:20", "24.06.2024  11:45:20", "24.09.2024  11:45:20", "05.10.2024  11:45:20", "05.10.2024  12:50:25", "05.10.2024  11:45:20", "05.10.2024  11:45:20", "05.10.2024  11:45:20", "05.10.2024  11:45:20", "05.10.2024  11:45:20", "05.10.2024  11:45:20", "05.10.2024  11:45:20", "05.10.2024  11:45:20" };


        //SeriesCollection = new SeriesCollection
        //{


        //    new LineSeries
        //    {
        //        Title = "",
        //        Values = values,

        //        Stroke = new SolidColorBrush(Color.FromRgb(100,149, 237)),
        //        PointGeometry = DefaultGeometries.Circle,
        //        PointGeometrySize = 15,
        //        //LabelPoint = chartPoint => $"({chartPoint.X}, {chartPoint.Y})",
        //        //LabelPoint = chartPoint => $"({chartPoint.Y}, {chartPoint.X})",
        //        LabelPoint = point => $" Дата и время: {customAxisLabels[(int)point.X]} \n Значение измерения: {point.Y}"

        //    }




        //};


        //var axisX = new LiveCharts.Wpf.Axis
        //{
        //    Title = "Дата и время", // Заголовок оси X
        //    FontSize = 12, // Увеличиваем размер шрифта заголовка
        //    FontWeight = FontWeights.Bold,
        //    Labels = customAxisLabels, // Устанавливаем свои подписи



        //};

        //// Привязываем ось к оси X графика
        //myCartesianChart.AxisX.Add(axisX);
        //}


    }





    
}

