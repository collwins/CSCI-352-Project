﻿using System;
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
    /// Interaction logic for MainMenu_Page.xaml
    /// </summary>
    public partial class MainMenu_Page : Page
    {
        Frame parentFrame;
        int uid = -1;
        string username = "";
        int balance = -1;
        
        public MainMenu_Page(Frame frame, int userID, string username, int balance)
        {
            parentFrame = frame;
            uid = userID;
            this.username = username;
            this.balance = balance;
            welcome_msg.Content = $"Welcome, {username}.";
            balance_label.Content = $"Balance: {balance}";
            InitializeComponent();
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BJG(object sender, RoutedEventArgs e)
        {

        }

        private void Black_Jack_Button_Click(object sender, RoutedEventArgs e)
        {
            parentFrame.Content = new Blackjack_Page(parentFrame, uid, username, balance);
        }

        private void Three_Card_Poker_Game_Click(object sender, RoutedEventArgs e)
        {
            parentFrame.Content = new Threecard_page();
        }

        private void Roulette_Game_Click(object sender, RoutedEventArgs e)
        {
            parentFrame.Content = new Roulette_page();
            
        }

    }
}
