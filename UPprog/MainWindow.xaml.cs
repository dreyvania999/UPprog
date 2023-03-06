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
using UPprog.Pages;

namespace UPprog
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Frame frame;
        public MainWindow()
        {
            InitializeComponent();
            frame = WindowFrame;
            frame.Navigate(new Autarization());
        }

        private void ClientOrBack_Click(object sender, RoutedEventArgs e)
        {
            
            if (frame.Content.ToString() == "UPprog.Pages.Autarization")
            {
                MessageBox.Show("fds");
            }
            else { MessageBox.Show(frame.Content.ToString()); }
        }
    }
}
