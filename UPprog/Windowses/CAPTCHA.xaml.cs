using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UPprog
{
    /// <summary>
    /// Логика взаимодействия для CAPTCHA.xaml
    /// </summary>
    public partial class CAPTCHA : Window
    {
        public string text;
        public CAPTCHA()
        {
            InitializeComponent();
            Random rand = new Random();
            int kolText = 4; // Количество символов в строке
            text = "";
            for (int i = 0; i < kolText; i++)
            {
                int j = rand.Next(2); // Выбор 0 - число; 1 - буква
                if (j == 0)
                {
                    text += rand.Next(9).ToString();
                }
                else
                {
                    int l = rand.Next(2); // Выбор 0 - заглавная; 1 - маленькая буква
                    if (l == 0)
                    {
                        text += (char)rand.Next('A', 'Z' + 1);
                    }
                    else
                    {
                        text += (char)rand.Next('a', 'z' + 1);
                    }
                }
            }
            // Переменные для того, чтобы символы шли по порядку
            int widthBegin = 0; // Начало отрезка
            int widthEnd = 0; // Конец отрезка
            int h = (int)CvField.Width / text.Length; // Шаг разбиения
            for (int i = 0; i < text.Length; i++) // Заполнение текста
            {
                if (i == 0) // Если первое разбиение
                {
                    widthEnd += h;
                }
                else
                {
                    widthBegin = widthEnd;
                    widthEnd += h;
                }
                int height = rand.Next((int)CvField.Height);
                int width = rand.Next(widthBegin, widthEnd);
                if (height > 170) // Чтобы не выходило за пределы поля (30 - это самое большая высота символа)
                {
                    height -= 30;
                }
                if (width > 590) // Чтобы не выходило за пределы поля (10 - это самое большая длина символа)
                {
                    widthEnd -= 10;
                }
                int fontSize = rand.Next(16, 33);
                TextBlock txt = new TextBlock()
                {
                    Text = text[i].ToString(),
                    TextDecorations = TextDecorations.Strikethrough,
                    Padding = new Thickness(width, height, 0, 0),
                    FontSize = fontSize,
                };
                _ = CvField.Children.Add(txt);
            }
            int kolLine = rand.Next(5, 16); // Рандомное количество линий
            for (int i = 0; i < kolLine; i++)
            {
                SolidColorBrush brush = new SolidColorBrush(Color.FromRgb((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256))); // Рандомный RGB цвет
                Line l = new Line()
                {
                    X1 = rand.Next((int)CvField.Width),
                    Y1 = rand.Next((int)CvField.Height),
                    X2 = rand.Next((int)CvField.Width),
                    Y2 = rand.Next((int)CvField.Height),
                    Stroke = brush,
                };
                _ = CvField.Children.Add(l);
            }
        }
        private void BtnGo_Click(object sender, RoutedEventArgs e)
        {
            if (TBInputField.Text == text)
            {
                Login.correctValue = true;
                Close();
            }
            else
            {
                Close();
            }
        }

    }
}
