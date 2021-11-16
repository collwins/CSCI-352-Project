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
    /// Interaction logic for MainMenu_Page.xaml
    /// </summary>
    public partial class MainMenu_Page : Page
    {
        Frame parentFrame;
        
        public MainMenu_Page(Frame frame)
        {
            parentFrame = frame;
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
            parentFrame.Content = new Blackjack_Page();
        }

        private void Three_Card_Poker_Game_Click(object sender, RoutedEventArgs e)
        {
            parentFrame.Content = new Threecard_page(PageContent);
        }

        private void Roulette_Game_Click(object sender, RoutedEventArgs e)
        {
            parentFrame.Content = new Roulette_page(PageContent);
            
        }

        private void PageContent_Navigated(object sender, NavigationEventArgs e)
        {
            PageContent.Content = new Blackjack_Page();
        }
    }
}
