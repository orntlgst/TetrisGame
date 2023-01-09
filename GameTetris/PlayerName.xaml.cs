using System;
using System.Collections.Generic;
using System.IO;
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

namespace GameTetris
{
    /// <summary>
    /// Interaction logic for PlayerName.xaml
    /// </summary>
    public partial class PlayerName : Window
    {
        public PlayerName()
        {
            InitializeComponent();
            this.WindowStyle = WindowStyle.None;
        }

        private string name = "";

        private void btn_StartClick(object sender, RoutedEventArgs e)
        {
            name = MainTextBox.Text;
            if (name == "")
            {
                name = "Guest";
            }
            string path = @"C:\Users\USer\source\repos\GameTetris";
            using (StreamWriter w = new StreamWriter(path, true, Encoding.GetEncoding(1251)))
            {
                w.WriteLine($"{name}");
            }
            Tetris tetris = new Tetris();
            tetris.ShowDialog();
            this.Close();
        }
    }
}
