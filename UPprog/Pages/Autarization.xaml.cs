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

namespace UPprog.Pages
{
    /// <summary>
    /// Логика взаимодействия для Autarization.xaml
    /// </summary>
    public partial class Autarization : Page
    {
        int kolError;
        public static bool correctValue;
        int countTime; // Время для повторного получения кода
        DispatcherTimer disTimer = new DispatcherTimer();
        public Autarization()
        {
            InitializeComponent();
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


        private void DisTimer_Tick(object sender, EventArgs e)
        {
           
        }
    }
}