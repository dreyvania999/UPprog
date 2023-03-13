using System;
using System.Windows.Media;

namespace UPprog
{
    public partial class Product
    {
        public SolidColorBrush colorBackground
        {
            get
            {
                if (ProductDiscountAmount > 15)
                {
                    SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFromString("#7fff00");
                    return color;
                }
                return Brushes.White;
            }
        }
        public double costWithDiscount => (double)(Convert.ToDouble(ProductCost) - (Convert.ToDouble(ProductCost) * ProductDiscountAmount / 100));

        public string costWithDiscountString => ProductDiscountAmount != 0 ? costWithDiscount.ToString("0.00") : "";

        public string textDecoration => ProductDiscountAmount != 0 ? "Strikethrough" : "Baseline";
    }
}
