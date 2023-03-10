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
using static System.Net.Mime.MediaTypeNames;

namespace UPprog.Pages
{
    /// <summary>
    /// Логика взаимодействия для Autarization.xaml
    /// </summary>
    public partial class Autarization : Page
    {
        string CAPCHAtext = "";
        int kolError;
        public static bool correctValue;
        int countTime; // Время для повторного получения кода
        DispatcherTimer disTimer = new DispatcherTimer();
        public Autarization()
        {
            InitializeComponent();
            CAPTCHA();
            kolError = 0;
            correctValue = false;
            disTimer.Interval = new TimeSpan(0, 0, 1);
            disTimer.Tick += new EventHandler(DisTimer_Tick);
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            User user = MainWindow.DB.User.FirstOrDefault(x => x.UserLogin == Login.Text && x.UserPassword == Password.Text);
            if (user != null)
            {
                //передать роль 
                MainWindow.frame.Navigate(new ProductList());
            }
            else
            {
                if (kolError == 0)
                {
                    MessageBox.Show("Пользователь с таким логиным и паролем не найден!");
                    kolError++;
                }
                else
                {
                    
                }
            }
        }


        public void CAPTCHA()
        {
            Random rand = new Random();
            int kolText = 4; 
            
            for (int i = 0; i < kolText; i++)
            {
                int j = rand.Next(2); 
                if (j == 0)
                {
                    CAPCHAtext = CAPCHAtext + rand.Next(9).ToString();
                }
                else
                {
                    int l = rand.Next(2); 
                    if (l == 0)
                    {
                        CAPCHAtext = CAPCHAtext + (char)rand.Next('A', 'Z' + 1);
                    }
                    else
                    {
                        CAPCHAtext = CAPCHAtext + (char)rand.Next('a', 'z' + 1);
                    }
                }
            }
            
            int widthBegin = 0; 
            int widthEnd = 0; 
            int h = (int) Canva.ActualWidth / CAPCHAtext.Length; // Шаг разбиения
            for (int i = 0; i < CAPCHAtext.Length; i++) // Заполнение текста
            {
                if (i == 0) // Если первое разбиение
                {
                    widthEnd += h;
                }
                else
                {
                    widthBegin = widthEnd;
                    widthEnd += h;
                }
                int height = rand.Next((int)Canva.ActualHeight);
                int width = rand.Next(widthBegin, widthEnd);
                if (height > 170) // Чтобы не выходило за пределы поля (30 - это самое большая высота символа)
                {
                    height -= 30;
                }
                if (width > 590) // Чтобы не выходило за пределы поля (10 - это самое большая длина символа)
                {
                    widthEnd -= 10;
                }
                int fontSize = rand.Next(16, 33);
                TextBlock txt = new TextBlock()
                {
                    Text = CAPCHAtext[i].ToString(),
                    TextDecorations = TextDecorations.Strikethrough,
                    Padding = new Thickness(width, height, 0, 0),
                    FontSize = fontSize,
                };
                Canva.Children.Add(txt);
            }
            int kolLine = rand.Next(5, 16); // Рандомное количество линий
            for (int i = 0; i < kolLine; i++)
            {
                SolidColorBrush brush = new SolidColorBrush(Color.FromRgb((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256))); // Рандомный RGB цвет
                Line l = new Line()
                {
                    X1 = rand.Next((int)Canva.ActualWidth),
                    Y1 = rand.Next((int)Canva.ActualHeight),
                    X2 = rand.Next((int)Canva.ActualWidth),
                    Y2 = rand.Next((int)Canva.ActualHeight),
                    Stroke = brush,
                };
                Canva.Children.Add(l);
            }
        }


        private void DisTimer_Tick(object sender, EventArgs e)
        {
           
        }
    }
}