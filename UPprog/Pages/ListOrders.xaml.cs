using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UPprog
{
    public partial class ListOrders : Page
    {
        private readonly User user;
        public ListOrders()
        {
            InitializeComponent();
            getData();
        }

        public ListOrders(User user)
        {
            InitializeComponent();
            this.user = user;
            getData();
        }

        /// <summary>
        /// Заполнение начальных данных на странице
        /// </summary>
        private void getData()
        {
            ListVOrders.ItemsSource = MainWindow.DB.Order.ToList();
            ComboSort.SelectedIndex = 0;
            ComboFilt.SelectedIndex = 0;
        }

        private void ButonBack_Click(object sender, RoutedEventArgs e)
        {
            _ = user != null ? MainWindow.frame.Navigate(new ListProduct(user)) : MainWindow.frame.Navigate(new ListProduct());
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        /// <summary>
        /// Фильтрация и сортировка списка заказов
        /// </summary>
        private void Filter()
        {
            List<Order> orders = MainWindow.DB.Order.ToList();
            if (ComboFilt.SelectedIndex > 0)
            {
                switch (ComboFilt.SelectedIndex)
                {
                    case 1:
                        orders = orders.Where(x => x.DiscountProcent > 0 && x.DiscountProcent < 10).ToList();
                        break;
                    case 2:
                        orders = orders.Where(x => x.DiscountProcent >= 10 && x.DiscountProcent < 15).ToList();
                        break;
                    case 3:
                        orders = orders.Where(x => x.DiscountProcent >= 15).ToList();
                        break;
                }
            }
            if (ComboSort.SelectedIndex > 0)
            {
                switch (ComboSort.SelectedIndex)
                {
                    case 1:
                        orders = orders.OrderBy(x => x.Summa).ToList();
                        break;
                    case 2:
                        orders = orders.OrderByDescending(x => x.Summa).ToList();
                        break;
                }
            }
            ListVOrders.ItemsSource = orders;
            if (orders.Count == 0)
            {
                _ = MessageBox.Show("Данные не найдены");
            }
        }

        private void ButonChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int index = Convert.ToInt32(button.Uid);
            Order order = MainWindow.DB.Order.FirstOrDefault(x => x.OrderID == index);
            ChangeOrder change = new ChangeOrder(order);
            _ = change.ShowDialog();
        }


    }
}
