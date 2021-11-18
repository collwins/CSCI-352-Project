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

namespace Main_Menu
{
    /// <summary>
    /// Interaction logic for Blackjack_Page.xaml
    /// </summary>
    public partial class Blackjack_Page : Page
    {
        Frame parentFrame;
        int uid;
        string username;
        int balance;
        public Blackjack_Page()
        {
            parentFrame = null;
            uid = -1;
            username = "";
            balance = -1;
            hit_btn.Opacity = 0;
            hit_btn.IsEnabled = false;
            balance_label.Content = $"BALANCE: {balance}";
            InitializeComponent();
        }
        public Blackjack_Page(Frame parentFrame, int uid, string username, int balance)
        {
            this.parentFrame = parentFrame;
            this.uid = uid;
            this.username = username;
            this.balance = balance;
            balance_label.Content = $"BALANCE: ${this.balance}";
            InitializeComponent();
        }


        private void hit_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
