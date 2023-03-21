using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace UPprog
{
    public partial class Order
    {
        public string OrderList
        {
            get
            {
                List<OrderProduct> products = MainWindow.DB.OrderProduct.Where(x => x.OrderID == OrderID).ToList();
                string orderList = "";
                for (int i = 0; i < products.Count; i++)
                {
                    orderList = i == products.Count - 1
                        ? orderList + products[i].Product.ProductName + " Количество: " + products[i].Count
                        : orderList + products[i].Product.ProductName + " Количество: " + products[i].Count + "\n";
                }
                return orderList;
            }
        }
        public double Summa
        {
            get
            {
                List<OrderProduct> products = MainWindow.DB.OrderProduct.Where(x => x.OrderID == OrderID).ToList();
                double summa = 0;
                foreach (OrderProduct product in products)
                {
                    summa += (double)product.Product.ProductCost * product.Product.costWithDiscount / 100 * product.Count;
                }
                return summa;
            }
        }

        public string StrSumma => "" + Summa.ToString("0.00");

        public double DiscountProcent
        {
            get
            {
                List<OrderProduct> products = MainWindow.DB.OrderProduct.Where(x => x.OrderID == OrderID).ToList();
                double summaDiscount = 0;
                foreach (OrderProduct product in products)
                {
                    summaDiscount += product.Product.costWithDiscount * product.Count;
                }
                double summa = 0;
                foreach (OrderProduct product in products)
                {
                    summa += (double)product.Product.ProductCost * product.Count;
                }
                double procent = (summa - summaDiscount) / summa * 100;
                return procent;
            }
        }

        public string StrDiscount => "" + DiscountProcent.ToString("0.00");
        public SolidColorBrush colorBackground
        {
            get
            {
                bool b = true;
                List<OrderProduct> orderProducts = MainWindow.DB.OrderProduct.Where(x => x.OrderID == OrderID).ToList();
                foreach (OrderProduct product in orderProducts)
                {
                    if (product.Count > product.Product.ProductQuantityInStock || product.Product.ProductQuantityInStock <= 3)
                    {
                        b = false;
                        break;
                    }
                }
                if (b)
                {
                    SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFromString("#20b2aa");
                    return color;
                }
                else
                {
                    SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFromString("#ff8c00");
                    return color;
                }

            }
        }
    }
}
