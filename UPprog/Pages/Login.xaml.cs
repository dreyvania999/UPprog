using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace UPprog
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private int kolError;
        public static bool correctValue;
        private int countTime; // Время для повторного получения кода
        private readonly DispatcherTimer disTimer = new DispatcherTimer();
        public Login()
        {
            InitializeComponent();
            kolError = 0;
            correctValue = false;
            disTimer.Interval = new TimeSpan(0, 0, 1);
            disTimer.Tick += new EventHandler(DisTimer_Tick);
        }

        private void BtnAutorization_Click(object sender, RoutedEventArgs e)
        {
            User user = MainWindow.DB.User.FirstOrDefault(x => x.UserLogin == tbLogin.Text && x.UserPassword == pbPassword.Password);
            if (user != null)
            {
                _ = MainWindow.frame.Navigate(new ListProduct(user));
            }
            else
            {
                if (kolError == 0)
                {
                    _ = MessageBox.Show("Пользователь с таким логиным и паролем не найден!");
                    kolError++;
                }
                else
                {
                    CAPTCHA captcha = new CAPTCHA();
                    _ = captcha.ShowDialog();
                    kolError++;
                    if (!correctValue)
                    {
                        BtnAutorization.IsEnabled = false;
                        countTime = 10;
                        tbNewCode.Text = "Получить новый код можно будет через " + countTime + " секунд";
                        tbNewCode.Visibility = Visibility.Visible;
                        disTimer.Start();
                    }
                }
            }
        }

        private void BtnGuest_Click(object sender, RoutedEventArgs e)
        {
            _ = MainWindow.frame.Navigate(new ListProduct());
        }
        private void DisTimer_Tick(object sender, EventArgs e)
        {
            if (countTime == 0) // Если 10 секунд закончились
            {
                BtnAutorization.IsEnabled = true;
                disTimer.Stop();
                tbNewCode.Visibility = Visibility.Collapsed;
            }
            else
            {
                tbNewCode.Text = "Получить новый код можно будет через " + countTime + " секунд";
            }
            countTime--;
        }
    }
}
