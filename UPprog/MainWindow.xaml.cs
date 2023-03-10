using System.Windows;
using System.Windows.Controls;
using UPprog.Pages;

namespace UPprog
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Entities DB;
        public static Frame frame;
        public MainWindow()
        {
            InitializeComponent();
            DB = new Entities();
            frame = WindowFrame;
            frame.Navigate(new Autarization());
        }

        private void ClientOrBack_Click(object sender, RoutedEventArgs e)
        {

            if (frame.Content.ToString() == "UPprog.Pages.Autarization")
            {
                ProductList.CurrentUserRole= 0;
                frame.Navigate(new ProductList());
            }
            else
            {
                ProductList.CurrentUserRole = 0;
                frame.Navigate(new Autarization());
            }
        }
    }
}
