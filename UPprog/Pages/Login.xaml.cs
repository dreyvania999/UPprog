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
        private int countTime;
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
            User user = MainWindow.DB.User.FirstOrDefault(x => x.UserLogin == TBLogin.Text && x.UserPassword == pbPassword.Password);
            if (user != null)
            {
                if (kolError == 0)
                {
                    _ = MainWindow.frame.Navigate(new ListProduct(user));
                }
                else
                {
                    CAPTCHA captcha = new CAPTCHA();
                    _ = captcha.ShowDialog();
                    if (correctValue)
                    {
                        _ = MainWindow.frame.Navigate(new ListProduct(user));
                    }
                }
            }
            else
            {
                _ = MessageBox.Show("Пользователь с таким логиным и паролем не найден!");
                correctValue = false;
                CAPTCHA captcha = new CAPTCHA();
                _ = captcha.ShowDialog();
                kolError++;
                if (!correctValue) // Если капча не пройдена
                {
                    BtnAutorization.IsEnabled = false;
                    countTime = 10;
                    TBNewCode.Text = "Повторить попытку авторизации можно через " + countTime + " секунд";
                    TBNewCode.Visibility = Visibility.Visible;
                    disTimer.Start();
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
                TBNewCode.Visibility = Visibility.Collapsed;
            }
            else
            {
                TBNewCode.Text = "Повторно авторизоваться можно через " + countTime + " секунд";
            }
            countTime--;
        }
    }
}
