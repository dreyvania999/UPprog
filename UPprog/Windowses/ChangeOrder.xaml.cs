using System.Linq;
using System.Windows;

namespace UPprog
{
    /// <summary>
    /// Логика взаимодействия для ChangeOrder.xaml
    /// </summary>
    public partial class ChangeOrder : Window
    {
        private readonly Order _order;

        public ChangeOrder(Order order)
        {
            InitializeComponent();

            _order = order;
            ComboStatus.ItemsSource = MainWindow.DB.Status.ToList();
            ComboStatus.SelectedValuePath = "Id_status";
            ComboStatus.DisplayMemberPath = "Status";

            TBOrder.Text = "Заказ №" + order.OrderID;
            dpDate.SelectedDate = order.OrderDeliveryDate;
            ComboStatus.SelectedValue = order.OrderStatus;
        }

        private void ButonSave_Click(object sender, RoutedEventArgs e)
        {
            _order.OrderDeliveryDate = dpDate.SelectedDate.Value;
            _order.OrderStatus = (int)ComboStatus.SelectedValue;

            try
            {
                _ = MainWindow.DB.SaveChanges();
                _ = MessageBox.Show("Заказ успешно изменён", "Изменение заказа", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();

            }
            catch
            {
                _ = MessageBox.Show("Не удалось изменить данные", "Изменение заказа", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}