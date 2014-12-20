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
using System.Windows.Shapes;
using MinerClient.ServiceReference1;

namespace MinerClient
{
    /// <summary>
    /// Логика взаимодействия для Records.xaml
    /// </summary>
    public partial class Records : Window
    {
        public Records()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tbRes.Text = "";
            var MinerClient = new Service1Client();
            foreach (string s in MinerClient.PrintRec(1))
            {
                tbRes.Text += s + "\n";
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            tbRes.Text = "";
            var MinerClient = new Service1Client();
            foreach (string s in MinerClient.PrintRec(2))
            {
                tbRes.Text += s + "\n";
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            tbRes.Text = "";
            var MinerClient = new Service1Client();
            foreach (string s in MinerClient.PrintRec(3))
            {
                tbRes.Text += s + "\n";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbRes.Text = "";
            var MinerClient = new Service1Client();
            foreach (string s in MinerClient.PrintRec(1))
            {
                tbRes.Text += s + "\n";
            }
        }

    }
}
