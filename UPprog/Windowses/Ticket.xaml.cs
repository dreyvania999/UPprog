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
        private readonly double summa;
        private readonly double summaDiscount;
        private readonly int countDay;
        public Ticket(Order order, List<ProductBasket> basket, double summa, double summaDiscount, int countDay)
        {
            InitializeComponent();
            this.order = order;
            this.basket = basket;
            this.summa = summa;
            this.summaDiscount = summaDiscount;
            this.countDay = countDay;
            TBOrderNomer.Text += order.OrderNomer.ToString();
            TBDateOrder.Text += order.OrderDate.ToString("d");
            for (int i = 0; i < basket.Count; i++)
            {
                TBOrders.Text = i == basket.Count - 1
                    ? TBOrders.Text + basket[i].product.ProductName + " Количество: " + basket[i].count
                    : TBOrders.Text + basket[i].product.ProductName + " Количество: " + basket[i].count + "\n";
            }
            TBSumma.Text = TBSumma.Text + summa.ToString("0.00") + " руб.";
            TBSummaDiscount.Text = TBSummaDiscount.Text + summaDiscount.ToString("0.00") + " руб.";
            TBOrderPickupPoint.Text += order.PickupPoint.Adress;
            TBCode.Text += order.OrderCode.ToString();
            if (countDay == 3)
            {
                TBDeliveryDate.Text += "3 дня";
            }
            else
            {
                TBDeliveryDate.Text += "6 дней";
            }
        }

        private void ButonBasket_Click(object sender, RoutedEventArgs e)
        {
            PdfDocument document = new PdfDocument();
            int height = 0;
            document.Info.Title = "Талон для получения заказа";
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont fontHeader = new XFont("Arial", 18, XFontStyle.Bold);
            gfx.DrawString("Талон для получения заказа", fontHeader, XBrushes.Black,
                new XRect(10, height, page.Width, page.Height),
                XStringFormats.TopCenter);
            XFont font = new XFont("Arial", 14);
            height += 30;
            gfx.DrawString("Номер: " + order.OrderNomer, font, XBrushes.Black,
                new XRect(10, height, page.Width, page.Height),
                XStringFormats.TopLeft);
            height += 30;
            gfx.DrawString("Дата заказа: " + order.OrderDate.ToString("D"), font, XBrushes.Black,
                new XRect(10, height, page.Width, page.Height),
                XStringFormats.TopLeft);
            height += 30;
            if (countDay == 3)
            {
                gfx.DrawString("Заказ будет готов через 3 дня", font, XBrushes.Black,
                    new XRect(10, height, page.Width, page.Height),
                    XStringFormats.TopLeft);
            }
            else
            {
                gfx.DrawString("Заказ будет готов через 6 дней", font, XBrushes.Black,
                    new XRect(10, height, page.Width, page.Height),
                    XStringFormats.TopLeft);
            }
            height += 30;
            gfx.DrawString("Дата получения заказа: " + order.OrderDeliveryDate.ToString("D"), font, XBrushes.Black,
                new XRect(10, height, page.Width, page.Height),
                XStringFormats.TopLeft);
            height += 30;
            gfx.DrawString("Состав заказа: ", font, XBrushes.Black,
                new XRect(10, height, page.Width, page.Height),
                XStringFormats.TopLeft);
            for (int i = 0; i < basket.Count; i++)
            {
                height += 30;
                if (i != basket.Count - 1)
                {
                    gfx.DrawString("" + basket[i].product.ProductName + " Колличество: " + basket[i].count + ";", font, XBrushes.Black,
                        new XRect(30, height, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                else
                {
                    gfx.DrawString("" + basket[i].product.ProductName + " Колличество: " + basket[i].count + ".", font, XBrushes.Black,
                        new XRect(30, height, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
            }
            height += 30;
            gfx.DrawString("Сумма заказа: " + summa.ToString("0.00") + " руб.", font, XBrushes.Black,
                new XRect(10, height, page.Width, page.Height),
                XStringFormats.TopLeft);
            height += 30;
            gfx.DrawString("Сумма скидки: " + summaDiscount.ToString("0.00") + " руб.", font, XBrushes.Black,
                new XRect(10, height, page.Width, page.Height),
                XStringFormats.TopLeft);
            height += 30;
            gfx.DrawString("Пункт выдачи: " + order.PickupPoint.Adress, font, XBrushes.Black,
                new XRect(10, height, page.Width, page.Height),
                XStringFormats.TopLeft);
            height += 30;
            gfx.DrawString("Код для получения: " + order.OrderCode, fontHeader, XBrushes.Black,
                new XRect(10, height, page.Width, page.Height),
                XStringFormats.TopLeft);
            string filename = "TicketPDF.pdf";
            document.Save(filename);
            _ = Process.Start(filename);

        }

        private void ButonBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
