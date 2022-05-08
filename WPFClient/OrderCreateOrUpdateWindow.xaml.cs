using OGAOE7_HFT_2021221.Models;
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

namespace OGAOE7_HFT_2021221.WPFClient
{
    /// <summary>
    /// Interaction logic for OrderUpdateWindow.xaml
    /// </summary>
    public partial class OrderCreateOrUpdateWindow : Window
    {
        public OrderCreateOrUpdateWindow()
        {

            InitializeComponent();
            Order = new PromoOrder();
        }

        public OrderCreateOrUpdateWindow(PromoOrder selectedOrder) : this()
        {
            Order = selectedOrder;
            sp.DataContext = Order;

        }

        public PromoOrder Order { get; internal set; }
        private void btn_send_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
