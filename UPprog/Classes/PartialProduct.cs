using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using UPprog.Pages;

namespace UPprog
{
    public partial class Product
    {

        public double costWithDiscount
        {
            get
            {
                return (double)(Convert.ToDouble(ProductCost) - (Convert.ToDouble(ProductCost) * ProductDiscountAmount / 100));
            }
        }

    }
}

