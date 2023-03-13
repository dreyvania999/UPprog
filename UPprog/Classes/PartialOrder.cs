using System.Collections.Generic;
using System.Linq;

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
        public string Summa
        {
            get
            {
                List<OrderProduct> products = MainWindow.DB.OrderProduct.Where(x => x.OrderID == OrderID).ToList();
                double summa = 0;
                foreach (OrderProduct product in products)
                {
                    summa += (double)product.Product.ProductCost * product.Product.costWithDiscount / 100 * product.Count;
                }
                return "" + summa.ToString("0.00");
            }
        }
        public string Discount
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
                return "" + procent.ToString("0.00");
            }
        }

    }
}
