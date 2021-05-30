using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MinesweeperGame
{
    public partial class Statistics : Form
    {
        private static string path = "C:\\Users\\super\\Source\\Repos\\MinesweeperGame\\MinesweeperGame\\statistics.txt";

        private static int numberOfInformation = 5;
        private Label[] infoLabels = new Label[numberOfInformation];
        private Label[] resultsLabels = new Label[numberOfInformation];
        private Label emptyFileLabel = new Label();

        public Statistics()
        {
            InitializeComponent();
            String line;
            using (StreamReader sr = File.OpenText(path))
            {
                line = sr.ReadLine();
            }
            if(line != null)
                initializeLabels(line);
            else
                initilizeEmptyLabel(); //if the file is empty
        }

        //function to initilize all the labels
        public void initializeLabels(String line) {
            for(int i = 0; i < numberOfInformation; i++)
            {
                infoLabels[i] = new Label();
                infoLabels[i].Font = new Font("Arial", 16, FontStyle.Regular);
                //infoLabels[i].BackColor = Color.FromArgb(7, 54, 66);
                infoLabels[i].ForeColor = Color.Black;
                infoLabels[i].AutoSize = true;
                infoLabels[i].Location = new Point(20, 80 + 60 * i);
                this.Controls.Add(infoLabels[i]);

                resultsLabels[i] = new Label();
                resultsLabels[i].Font = new Font("Arial", 16, FontStyle.Bold);
                //resultsLabels[i].BackColor = Color.FromArgb(7, 54, 66);
                resultsLabels[i].ForeColor = Color.Black;
                resultsLabels[i].AutoSize = true;
                this.Controls.Add(resultsLabels[i]);
            }
            setText(line);
        }

        //function to initialize the error label
        public void initilizeEmptyLabel()
        {
            emptyFileLabel.Font = new Font("Arial", 20, FontStyle.Regular);
            //emptyFileLabel.BackColor = Color.FromArgb(7, 54, 66);
            emptyFileLabel.ForeColor = Color.Red;
            emptyFileLabel.AutoSize = true;
            emptyFileLabel.Text = "No player stats yet!";
            emptyFileLabel.Location = new Point((this.Size.Width / 2) - (emptyFileLabel.Size.Width / 2) - 80, (this.Size.Height / 2) - (emptyFileLabel.Size.Height / 2) - 50);
            this.Controls.Add(emptyFileLabel);
        }

        //function to set the text on the labels with the results
        public void setText(String line)
        {
            List<string> players = new List<string>();
            List<string> resultOfGame = new List<string>();
            int numberOfClicks = 0, timeCounter = 0;

            using (StreamReader sr = File.OpenText(path))
            {
                while (line != null)
                {
                    String[] results = line.Split(";");
                    players.Add(results[0]);
                    numberOfClicks += Convert.ToInt32(results[1]);
                    timeCounter += Convert.ToInt32(results[2]);
                    resultOfGame.Add(results[3]);
                    line = sr.ReadLine();
                }
            }
            //we make a group by not to count the same player more than one time
            var playersGroupBy = players.GroupBy(x => x);

            infoLabels[0].Text = "Number of players: ";
            infoLabels[1].Text = "Average clicks per game: ";
            infoLabels[2].Text = "Average time per game: ";
            infoLabels[3].Text = "Winning percentage: ";
            infoLabels[4].Text = "Defeat percentage: ";

            //we calculate the results
            resultsLabels[0].Text = playersGroupBy.Count().ToString() + " player(s)";
            resultsLabels[1].Text = Math.Round((((float) numberOfClicks) / players.Count()), 3).ToString() + " clicks";
            resultsLabels[2].Text = Math.Round((((float) timeCounter) / players.Count()), 3).ToString() + " seconds";
            resultsLabels[3].Text = (((float) resultOfGame.Where(x => x.Equals("w")).Count() * 100) / resultOfGame.Count()).ToString() + "%";
            resultsLabels[4].Text = (((float)resultOfGame.Where(x => x.Equals("d")).Count() * 100) / resultOfGame.Count()).ToString() + "%";
            
            //we set the location of the results
            for(int i = 0; i < numberOfInformation; i++)
            {
                resultsLabels[i].Location = new Point(infoLabels[i].Location.X + infoLabels[i].Size.Width + 5, infoLabels[i].Location.Y);
            }
        }
    }
}
