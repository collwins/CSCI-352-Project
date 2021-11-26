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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Bet_Page : Page
    {
        Frame parentFrame;
        int uid;
        int balance;
        bool validBet;
        public Bet_Page(Frame parentFrame, int uid, int balance)
        {
            this.uid = uid;
            this.parentFrame = parentFrame;
            this.balance = balance;
            validBet = false;
            InitializeComponent();
            balance_label.Content = $"${balance}";
            balance_after_bet_label.Content = $"${balance}";
        }

        private void submit_btn_Click(object sender, RoutedEventArgs e)
        {
            if (validBet) //if the bet input is valid...
            {
                balance -= Convert.ToInt32(bet_input.Text);
                parentFrame.Content = new Blackjack_Page(parentFrame, uid, balance, Convert.ToInt32(bet_input.Text));
                //db stuff will be handled in the blackjack page
                //if the user loses, simply load the bet page again.
            }
        }

        private void bet_input_TextChanged(object sender, TextChangedEventArgs e)
        {
            string content = bet_input.Text;
            if (content.Any(c => !char.IsDigit(c))){ //if any of the characters in the bet input field is not a digit...
                validBet = false;
                submit_error_label.Content = "Invalid bet input. Please enter a nonnegative integer.";
            }
            else if(content == "") //if the input is empty... (avoids errors)
            {
                balance_after_bet_label.Content = $"${balance}";
                submit_error_label.Content = "Please enter a bet.";
            }
            else if (Convert.ToInt32(content) > balance) //if the bet amount is greater than the current balance...
            {
                validBet = false;
                balance_after_bet_label.Content = $"${balance - Convert.ToInt32(content)}";
                submit_error_label.Content = "Invalid bet input. Bet is too large.";
            }
            else //all good!
            {
                validBet = true;
                balance_after_bet_label.Content = $"${balance - Convert.ToInt32(content)}";
                submit_error_label.Content = "";
            }
            //note: this also implictly checks for negative values. '-' is not a digit and violates the first check.
        }
    }
}
