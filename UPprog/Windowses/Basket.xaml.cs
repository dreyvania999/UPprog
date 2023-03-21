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
        private double summa; // Сумма заказа
        private double summaDiscount; // Сумма скидок
        private readonly User user; // Пользователь под которым произведён вход
        private readonly List<ProductBasket> bascet; // Корзина
        public Basket(List<ProductBasket> bascet, User user)
        {
            InitializeComponent();
            if (user != null)
            {
                TBFIO.Text = "" + user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
            }
            this.bascet = bascet;
            this.user = user;
            lvProduct.ItemsSource = bascet;
            calculateSummaAndDiscount();
            cmbPickupPoint.ItemsSource = MainWindow.DB.PickupPoint.ToList();
            cmbPickupPoint.SelectedValuePath = "ID";
            cmbPickupPoint.DisplayMemberPath = "Adress";
            cmbPickupPoint.SelectedIndex = 0;
        }

        private void ButonBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButonBasket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Order order = new Order();
                int countDay = 0; // Колличество дней на доставку
                List<Order> orderLast = MainWindow.DB.Order.OrderBy(x => x.OrderNomer).ToList();
                order.OrderNomer = orderLast[orderLast.Count - 1].OrderNomer + 1;
                order.OrderStatus = MainWindow.DB.Status.FirstOrDefault(x => x.Title == "Новый").ID;
                order.OrderDate = DateTime.Now;
                countDay = getDeliveryTime() ? 6 : 3;
                order.OrderDeliveryDate = order.OrderDate.AddDays(countDay);
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
                        ProductArticleNumber = productBasket.product.ID,
                        Count = productBasket.count
                    };
                    _ = MainWindow.DB.OrderProduct.Add(orderProduct);
                }
                _ = MainWindow.DB.SaveChanges();
                _ = MessageBox.Show("Заказ успешно создан");
                Ticket ticket = new Ticket(order, bascet, summa, summaDiscount, countDay);
                _ = ticket.ShowDialog();
                bascet.Clear();
                Close();
            }
            catch
            {
                _ = MessageBox.Show("При создание заказа возникла ошибка!");
            }
        }

        /// <summary>
        /// Определение срока доставки
        /// </summary>
        /// <returns></returns>
        private bool getDeliveryTime()
        {
            foreach (ProductBasket productBasket in bascet)
            {
                if (productBasket.product.ProductQuantityInStock < 3 || productBasket.product.ProductQuantityInStock < productBasket.count) // Если товара на складе меньше 3 или он отсутсвует для продажи текущего колличества, то заказ будет доставляяться 6 дней
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

        private void ButonDelete_Click(object sender, RoutedEventArgs e)
        {
            Button Buton = (Button)sender;
            int index = Convert.ToInt32(Buton.Uid);
            ProductBasket productBasket = bascet.FirstOrDefault(x => x.product.ID == index);
            _ = bascet.Remove(productBasket);
            if (bascet.Count == 0)
            {
                Close();
            }
            lvProduct.Items.Refresh();
            calculateSummaAndDiscount();
        }

        /// <summary>
        /// Подсчёт суммы заказа и скидок
        /// </summary>
        private void calculateSummaAndDiscount()
        {
            summa = 0;
            summaDiscount = 0;
            foreach (ProductBasket productBasket in bascet)
            {
                summa += productBasket.count * productBasket.product.costWithDiscount;
                summaDiscount += productBasket.count * ((double)productBasket.product.ProductCost - productBasket.product.costWithDiscount);
            }
            TBSumma.Text = "Сумма заказа: " + summa.ToString("0.00") + " руб.";
            TBSummaDiscount.Text = "Сумма скидки: " + summaDiscount.ToString("0.00") + " руб.";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox TB = (TextBox)sender;
            int index = Convert.ToInt32(TB.Uid);
            ProductBasket productBasket = bascet.FirstOrDefault(x => x.product.ID == index);
            productBasket.count = TB.Text.Replace(" ", "") == "" ? 0 : Convert.ToInt32(TB.Text);
            if (productBasket.count == 0) // Если колличество 0, то продукт из корзины удаляется
            {
                _ = bascet.Remove(productBasket);
            }
            if (bascet.Count == 0) // Если в корзине нет товаров, то окно закрывается
            {
                Close();
            }
            lvProduct.Items.Refresh();
            calculateSummaAndDiscount();
        }
    }
}
