using SnakeGame.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for Leaderboard.xaml
    /// </summary>
    public partial class Leaderboard : Window
    {
        public Leaderboard()
        {
            InitializeComponent();
            ReadFromJson();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new();
            menu.Show();
            this.Close();
        }

        private void ReadFromJson()
        {
            string path = @"C:\Users\emilk\source\repos\SnakeGame\Data\Data.json";
            var JsonString = File.ReadAllText(path); 
            List<LeaderboardList> leaderboards = JsonSerializer.Deserialize<List<LeaderboardList>>(JsonString);

            if (leaderboards.Count() <= 0)
            {
                LeaderboardHolders.Children.Add(new Label() { Content = "There was not found any records.", Style = (Style)FindResource("BoardHolders") });
                return;
            }
            for(int i = 0; i < leaderboards.Count(); i++)
                LeaderboardHolders.Children.Add(new Label()
                {
                    Content = $"{i+1}. | {leaderboards[i].Name} | {leaderboards[i].Score}",
                    Style = (Style)FindResource("BoardHolders")
                });

            //foreach (var item in leaderboards)
        }
    }
}
