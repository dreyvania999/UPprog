using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UPprog
{
    /// <summary>
    /// Логика взаимодействия для ListProduct.xaml
    /// </summary>
    public partial class ListProduct : Page
    {

        private readonly User CurrentUser;
        /// <summary>
        /// Метод лоя начала работы страницы посте проходжения авторизации
        /// </summary>
        /// <param name="user"> авторизированный польлзователь</param>
        public ListProduct(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            ControlsEdit();
            TBFIO.Text = CurrentUser.FIO;
            if (CurrentUser.Role.RoleID == 2 || CurrentUser.Role.RoleID == 3)// отображение кнопок в зависимости от роли пользователя
            {
                ButonOrders.Visibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// Метод для начала работы если пользователь не стал проходить авторизацию
        /// </summary>
        public ListProduct()
        {
            InitializeComponent();
            ControlsEdit();
        }
        /// <summary>
        /// Метод для фильтрации, поиска и сортировки
        /// </summary>
        public void Filter()
        {
            List<Product> products = MainWindow.DB.Product.ToList();
            if (TBSearch.Text.Length > 0)
            {
                products = products.Where(x => x.ProductName.ToLower().Contains(TBSearch.Text.ToLower())).ToList();
            }
            if (ComboFilt.SelectedIndex > 0)
            {
                switch (ComboFilt.SelectedIndex)
                {
                    case 1:
                        products = products.Where(x => x.ProductDiscountAmount > 0 && x.ProductDiscountAmount < 9.99).ToList();
                        break;
                    case 2:
                        products = products.Where(x => x.ProductDiscountAmount > 10 && x.ProductDiscountAmount < 14.99).ToList();
                        break;
                    case 3:
                        products = products.Where(x => x.ProductDiscountAmount > 15).ToList();
                        break;
                }
            }
            if (ComboSort.SelectedIndex > 0)
            {
                switch (ComboSort.SelectedIndex)
                {
                    case 1:
                        products = products.OrderBy(x => x.costWithDiscount).ToList();
                        break;
                    case 2:
                        products = products.OrderByDescending(x => x.costWithDiscount).ToList();
                        break;
                }
            }
            ListVProducts.ItemsSource = products;
            if (products.Count == 0)
            {
                _ = MessageBox.Show("Данные не найдены");
            }
            TBCountProduct.Text = "" + products.Count() + " из " + MainWindow.DB.Product.ToList().Count();
        }

        private void TBSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void ButonBack_Click(object sender, RoutedEventArgs e)
        {
            _ = MainWindow.frame.Navigate(new Login());
        }
        private readonly List<ProductBasket> basket = new List<ProductBasket>();
        private void miAddBasket_Click(object sender, RoutedEventArgs e)
        {
            Product x = (Product)ListVProducts.SelectedItem;
            bool stock = false; // Наличие товара (true - товар есть; false - товара нет)
            foreach (ProductBasket productBasket in basket)
            {
                if (productBasket.product == x) // Увеличение колличества товара в корзине на +1
                {
                    productBasket.count = productBasket.count += 1;
                    stock = true;
                }
            }
            if (!stock) // Добавление нового товара в корзину
            {
                ProductBasket product = new ProductBasket
                {
                    product = x,
                    count = 1
                };
                basket.Add(product);
            }
            ButonBasket.Visibility = Visibility.Visible;
        }

        private void ButonBasket_Click(object sender, RoutedEventArgs e)
        {
            Basket basketWindow = new Basket(basket, CurrentUser);
            _ = basketWindow.ShowDialog();
            if (basket.Count == 0)
            {
                ButonBasket.Visibility = Visibility.Collapsed;
            }
        }

        private void ButonOrders_Click(object sender, RoutedEventArgs e)
        {
            _ = CurrentUser != null ? MainWindow.frame.Navigate(new ListOrders(CurrentUser)) : MainWindow.frame.Navigate(new ListOrders());
        }

        private void ButonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button Buton = (Button)sender;
                int index = Convert.ToInt32(Buton.Uid);
                Product product = MainWindow.DB.Product.FirstOrDefault(x => x.ID == index);
                List<OrderProduct> orderProducts = MainWindow.DB.OrderProduct.Where(x => x.ID == index).ToList();
                if (orderProducts.Count == 0) // Если отсутсвуют заказы с таким товаром, то товар можно удалить
                {
                    _ = MainWindow.DB.Product.Remove(product);
                    _ = MainWindow.DB.SaveChanges();
                }
                else
                {
                    _ = MessageBox.Show("Товар нельзя удалить так как он указан в заказе!");
                }

            }
            catch
            {
                _ = MessageBox.Show("При удаление товара возникла ошибка");
            }
        }

        public void ControlsEdit()
        {
            ListVProducts.ItemsSource = MainWindow.DB.Product.ToList();
            ComboFilt.SelectedIndex = 0;
            ComboSort.SelectedIndex = 0;
            TBCountProduct.Text = "" + MainWindow.DB.Product.ToList().Count() + " из " + MainWindow.DB.Product.ToList().Count();
        }
        private void ButonDelete_Loaded(object sender, RoutedEventArgs e)
        {
            if (CurrentUser == null)
            {
                return;
            }
            Button ButonDelete = sender as Button;
            ButonDelete.Visibility = CurrentUser.Role.RoleID == 2 || CurrentUser.Role.RoleID == 3 ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
