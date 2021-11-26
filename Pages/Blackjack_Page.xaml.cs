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
        int balance;
        int bet;
   
        public Blackjack_Page(Frame parentFrame, int uid, int balance, int bet)
        {
            this.parentFrame = parentFrame;
            this.uid = uid;
            this.balance = balance;
            this.bet = bet;
            InitializeComponent();
            balance_label.Content = $"BALANCE: ${this.balance}";
            bet_label.Content = $"BET: ${this.bet}";
        }


        private void hit_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
