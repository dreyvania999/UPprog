using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UPprog
{
    /// <summary>
    /// Логика взаимодействия для Basket.xaml
    /// </summary>
    public partial class Basket : Window
    {
        private double summa;
        private double summaDiscount;
        private readonly User user;
        private readonly List<ProductBasket> bascet;
        public Basket(List<ProductBasket> bascet, User user)
        {
            InitializeComponent();
            if (user != null)
            {
                tbFIO.Text = "" + user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
            }
            this.bascet = bascet;
            this.user = user;
            lvProduct.ItemsSource = bascet;
            calculateSummaAndDiscount();
            cmbPickupPoint.ItemsSource = MainWindow.DB.PickupPoint.ToList();
            cmbPickupPoint.SelectedValuePath = "OrderPickupPointID";
            cmbPickupPoint.DisplayMemberPath = "OrderPickupPoint";
            cmbPickupPoint.SelectedIndex = 0;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnBasket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Order order = new Order();
                List<Order> orderLast = MainWindow.DB.Order.OrderBy(x => x.OrderCode).ToList();
                order.OrderCode = orderLast[orderLast.Count - 1].OrderCode + 1;
                order.OrderStatus = MainWindow.DB.Status.FirstOrDefault(x => x.Title == "Новый").ID;
                order.OrderDate = DateTime.Now;
                order.OrderDeliveryDate = getDeliveryTime() ? order.OrderDate.AddDays(6) : order.OrderDate.AddDays(3);
                order.OrderPickupPoint = ((PickupPoint)cmbPickupPoint.SelectedItem).ID;
                if (user != null)
                {
                    order.OrderClient = user.UserID;
                }
                Random rand = new Random();
                string textCode = "";
                for (int i = 0; i < 3; i++)
                {
                    textCode += rand.Next(10).ToString();
                }
                order.OrderCode = Convert.ToInt32(textCode);
                _ = MainWindow.DB.Order.Add(order);
                _ = MainWindow.DB.SaveChanges();
                foreach (ProductBasket productBasket in bascet)
                {
                    OrderProduct orderProduct = new OrderProduct
                    {
                        OrderID = order.OrderID,
                        ID = productBasket.product.ID,
                        Count = productBasket.count
                    };
                    _ = MainWindow.DB.OrderProduct.Add(orderProduct);
                }
                _ = MainWindow.DB.SaveChanges();
                _ = MessageBox.Show("Заказ успешно создан");
                Ticket ticket = new Ticket(order, bascet, summa, summaDiscount);
                _ = ticket.ShowDialog();
                bascet.Clear();
                Close();
            }
            catch
            {
                _ = MessageBox.Show("При создание заказа возникла ошибка!");
            }
        }

        private bool getDeliveryTime()
        {
            foreach (ProductBasket productBasket in bascet)
            {
                if (productBasket.product.ProductQuantityInStock < 3 || productBasket.product.ProductQuantityInStock < productBasket.count)
                {
                    return true;
                }
            }
            return false;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int index = Convert.ToInt32(btn.Uid);
            ProductBasket productBasket = bascet.FirstOrDefault(x => x.product.ID == index);
            _ = bascet.Remove(productBasket);
            if (bascet.Count == 0)
            {
                Close();
            }
            lvProduct.Items.Refresh();
            calculateSummaAndDiscount();
        }

        private void calculateSummaAndDiscount()
        {
            summa = 0;
            summaDiscount = 0;
            foreach (ProductBasket productBasket in bascet)
            {
                summa += productBasket.count * productBasket.product.costWithDiscount;
                summaDiscount += productBasket.count * ((double)productBasket.product.ProductCost - productBasket.product.costWithDiscount);
            }
            tbSumma.Text = "Сумма заказа: " + summa.ToString("0.00") + " руб.";
            tbSummaDiscount.Text = "Сумма скидки: " + summaDiscount.ToString("0.00") + " руб.";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int index = Convert.ToInt32(tb.Uid);
            ProductBasket productBasket = bascet.FirstOrDefault(x => x.product.ID == index);
            productBasket.count = tb.Text.Replace(" ", "") == "" ? 0 : Convert.ToInt32(tb.Text);
            if (productBasket.count == 0)
            {
                _ = bascet.Remove(productBasket);
            }
            if (bascet.Count == 0)
            {
                Close();
            }
            lvProduct.Items.Refresh();
            calculateSummaAndDiscount();
        }
    }
}
