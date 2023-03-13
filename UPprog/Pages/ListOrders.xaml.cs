using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UPprog
{
    /// <summary>
    /// Логика взаимодействия для ListOrders.xaml
    /// </summary>
    public partial class ListOrders : Page
    {
        public ListOrders()
        {
            InitializeComponent();
            lvListOrders.ItemsSource = MainWindow.DB.Order.ToList();
            cbSort.SelectedIndex = 0;
            cbFilt.SelectedIndex = 0;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            _ = MainWindow.frame.Navigate(new ListProduct());
        }
    }
}
