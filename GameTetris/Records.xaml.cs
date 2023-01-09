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
    /// Interaction logic for Records.xaml
    /// </summary>
    public partial class Records : Window
    {
        public Records()
        {
            InitializeComponent();
            TextBlock[] players = new TextBlock[10] { Player1, Player2, Player3, Player4, Player5, Player6, Player7, Player8, Player9, Player10 };
            int i = 0;
            string results = "";
            string path = @"C:\Users\USer\source\repos\GameTetris";
            string path2 = "top10.txt";
            top10(path, path2);

            if (isFileEmpty(path) == false)
            {
                using (var f = new StreamReader(path2, Encoding.GetEncoding(1251)))
                {
                    while ((results = f.ReadLine()) != null && i < 10)
                    {
                        if (results != "")
                        {
                            players[i].Text = results;
                        }
                        i++;
                    }
                }
            }
        }

        private bool isFileEmpty(string path)
        {
            string results = "";
            int k = 0;
            using (var f = new StreamReader(path, Encoding.GetEncoding(1251)))
            {
                while ((results = f.ReadLine()) != null)
                {
                    k++;
                }
            }
            if (k == 0)
            {
                return true;
            }
            return false;
        }

        private void top10(string path, string path2)
        {
            string tmp = "";
            List<InfoPlayer> results = new List<InfoPlayer>();
            int flag = 0;
            string name = "", score = "";
            using (var f = new StreamReader(path, Encoding.GetEncoding(1251)))
            {
                while ((tmp = f.ReadLine()) != null)
                {
                    if (tmp != "")
                    {
                        if (flag % 2 == 0)
                        {
                            name = tmp;
                        }
                        else
                        {
                            score = tmp;
                        }
                        flag++;
                    }
                    if (flag % 2 == 0 && flag != 0)
                    {
                        results.Add(new InfoPlayer(name, score));
                    }
                }
            }

            var sortedResulsts = from res in results
                                 orderby Int32.Parse(res.score) descending
                                 select res;

            using (StreamWriter w = new StreamWriter(path2, false, Encoding.GetEncoding(1251)))
            {
                foreach(var r in sortedResulsts)
                {
                    w.WriteLine($"{r.name} -> {r.score}");
                }
            }

        }


        private void Records_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowStyle = WindowStyle.None;
        }

        private void btn_QuitRecordsClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
