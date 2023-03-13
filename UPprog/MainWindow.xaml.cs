using System.Windows;
using System.Windows.Controls;

namespace UPprog
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Entities DB;
        public static Frame frame;
        public MainWindow()
        {
            InitializeComponent();
            DB = new Entities();
            frame = mainFrame;
            _ = frame.Navigate(new Login());
        }
    }
}
