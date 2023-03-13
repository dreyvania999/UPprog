namespace UPprog
{
    public class ProductBasket
    {
        public Product product { get; set; }
        public int count { get; set; }
        public string textDecoration => product.ProductDiscountAmount != 0 ? "Strikethrough" : "Baseline";
    }
}
