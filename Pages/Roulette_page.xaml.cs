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
    /// Interaction logic for Roulette_page.xaml
    /// </summary>
    public partial class Roulette_page : Page
    {
        Frame parentFrame;
        public Roulette_page()
        {
            InitializeComponent();
        }
        public Roulette_page(Frame parentFrame)
        {
            InitializeComponent();
            this.parentFrame = parentFrame;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Num_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
    }
}
