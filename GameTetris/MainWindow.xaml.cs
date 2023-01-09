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

namespace GameTetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStyle = WindowStyle.None;
        }

        private void btn_RecordsClick(object sender, RoutedEventArgs e)
        {
            Records records = new Records();
            records.ShowDialog();
        }

        private void btn_RulesClick(object sender, RoutedEventArgs e)
        {
            Rules rules = new Rules();
            rules.ShowDialog();
        }

        private void btn_PlayClick(object sender, RoutedEventArgs e)
        {
            PlayerName playerName = new PlayerName();
            playerName.ShowDialog();
        }

        private void btn_ExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
