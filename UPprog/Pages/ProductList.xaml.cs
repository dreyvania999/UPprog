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

namespace UPprog.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductList.xaml
    /// </summary>
    public partial class ProductList : Page
    {
        public static int CurrentUserRole=0;
        List<Basket> basket = new List<Basket>();
       
        public ProductList()
        {
            InitializeComponent();
            CreatingFields();
        }

        public void CreatingFields()
        {
            lvListProducts.ItemsSource =  MainWindow.DB.Product.ToList();
            cbFilt.SelectedIndex = 0;
            cbSort.SelectedIndex = 0;
            countProduct.Text = "" +  MainWindow.DB.Product.ToList().Count() + " из " +  MainWindow.DB.Product.ToList().Count();
        }

        public void Filter()
        {
            List<Product> products =  MainWindow.DB.Product.ToList();
            if (tbSearch.Text.Length > 0)
            {
                products = products.Where(x => x.ProductName.ToLower().Contains(tbSearch.Text.ToLower())).ToList();
            }
            if (cbFilt.SelectedIndex > 0)
            {
                switch (cbFilt.SelectedIndex)
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
            if (cbSort.SelectedIndex > 0)
            {
                switch (cbSort.SelectedIndex)
                {
                    case 1:
                        products = products.OrderBy(x => x.costWithDiscount).ToList();
                        break;
                    case 2:
                        products = products.OrderByDescending(x => x.costWithDiscount).ToList();
                        break;
                }
            }
            lvListProducts.ItemsSource = products;
            if (products.Count == 0)
            {
                MessageBox.Show("Данные не найдены");
            }
            countProduct.Text = "" + products.Count() + " из " +  MainWindow.DB.Product.ToList().Count();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void miAddBasket_Click(object sender, RoutedEventArgs e)
        {
            Product x = (Product)lvListProducts.SelectedItem;
            bool stock = false;
            foreach (var productBasket in basket)
            {
                if (productBasket.ProductInBasket == x)
                {
                    productBasket.count = productBasket.count++;
                    stock = true;
                }
            }
            if (!stock)
            {
                Basket product = new Basket();
                product.ProductInBasket = x;
                product.count = product.count++;
                basket.Add(product);
            }
            btnBasket.Visibility = Visibility.Visible;
        }

        private void btnBasket_Click(object sender, RoutedEventArgs e)
        {
            //Переход на карзину
        }
    }
}
