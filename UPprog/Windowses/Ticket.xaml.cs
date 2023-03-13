using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace UPprog
{
    /// <summary>
    /// Логика взаимодействия для Ticket.xaml
    /// </summary>
    public partial class Ticket : Window
    {
        private readonly Order order;
        private readonly List<ProductBasket> basket;
        public Ticket(Order order, List<ProductBasket> basket, double summa, double summaDiscount)
        {
            InitializeComponent();
            this.order = order;
            this.basket = basket;
            tbOrderNomer.Text += order.OrderNomer.ToString();
            tbDateOrder.Text += order.OrderDate.ToString("d");
            for (int i = 0; i < basket.Count; i++)
            {
                tbOrders.Text = i == basket.Count - 1
                    ? tbOrders.Text + basket[i].product.ProductName + " Количество: " + basket[i].count
                    : tbOrders.Text + basket[i].product.ProductName + " Количество: " + basket[i].count + "\n";
            }
            tbSumma.Text = tbSumma.Text + summa.ToString("0.00") + " руб.";
            tbSummaDiscount.Text = tbSummaDiscount.Text + summaDiscount.ToString("0.00") + " руб.";
            tbOrderPickupPoint.Text += order.PickupPoint.Adress;
            tbCode.Text += order.OrderCode.ToString();
        }

        private void btnBasket_Click(object sender, RoutedEventArgs e)
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Created with PDFsharp";
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont fontHeader = new XFont("Comic Sans MS", 22, XFontStyle.Bold);
            gfx.DrawString("Талон для получения заказа", fontHeader, XBrushes.Black,
                new XRect(0, 0, page.Width, page.Height),
                XStringFormats.TopCenter);
            XRect rect = new XRect(40, 100, 250, 220);
            gfx.DrawRectangle(XBrushes.SeaShell, rect);
            XFont font = new XFont("Comic Sans MS", 18);
            gfx.DrawString("Номер заказа: " + order.OrderNomer, font, XBrushes.Black,
                new XRect(0, 0, page.Width, page.Height),
                XStringFormats.BaseLineLeft);
            string filename = "TicketPDF.pdf";
            document.Save(filename);
            _ = Process.Start(filename);

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
