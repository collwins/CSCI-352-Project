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
        public Blackjack_Page()
        {
            InitializeComponent();
            hit_btn.Opacity = 0;
            hit_btn.IsEnabled = false;
        }
        public Blackjack_Page(Frame parentFrame)
        {
            InitializeComponent();
            this.parentFrame = parentFrame;
        }

        private void PageContent_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void hit_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
