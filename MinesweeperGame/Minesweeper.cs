using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MinesweeperGame
{
    
    public partial class Minesweeper: Form
    {
        private static string path = "C:\\Users\\super\\Source\\Repos\\MinesweeperGame\\MinesweeperGame\\statistics.txt";

        Button[,] btnArray; //array to store all the buttons
        Label lbMineCount = new Label(); //label to show the number of flags that have
        Label lbTime = new Label(); //label to show the time that was playing the level
        ComboBox comboLevels = new ComboBox(); //combobox to select the level
        Button reset = new Button(); //button to reset the game
        Label lbName = new Label(); //label to ask for the name
        TextBox textBoxName = new TextBox(); //textBox to enter the name
        Button showStatisticsButton = new Button(); //button to show the statistics

        Timer timer = new Timer(); //variable to have the time
        Random rnd; //random to generate the board

        private int timerCounter;
        private int[,] field; //array to store the position of the mines
        private Color[] colors = {Color.Blue, Color.Green, Color.Red, Color.Purple, Color.Maroon, Color.Turquoise, Color.Black, Color.Gray};

        private bool generate = true; //boolean to know if the game has already started
        private Size fieldSize = new Size(15, 15); //the size of the board, 10x10 buttons
        private int originalMineCount = 30; //the number of mines
        private int mineCount; //variable to know how many mines the user have left to locate
        private int clickCount; //variable to know the number of clicks

        private int cellSize;

        protected Rectangle container;

        public Minesweeper()
        {
            InitializeComponent();
            this.Size = new Size(716, 780);
            this.Text = "Minesweeper";
            this.Resize += OnResize;
            SetContainerLocation();
            rnd = new Random();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(Timer_Tick);
            mineCount = originalMineCount;
            clickCount = 1;
            btnArray = new Button[fieldSize.Width, fieldSize.Height];
            field = new int[fieldSize.Width, fieldSize.Height]; //if in the position i,j there is a 1, it is a mine

            createNameForm(); //we create the form to introduce the username
            createScore(); //we create the labels with the mines left, the time and a button to reset
            createBoard(); //we create the board
        }

        //function to restart the board when the user changes the level
        public void generateMinesweeper()
        {
            foreach(Control ctr in btnArray)
            {
                this.Controls.Remove(ctr);
            }

            mineCount = originalMineCount;
            clickCount = 1;
            btnArray = new Button[fieldSize.Width, fieldSize.Height];
            field = new int[fieldSize.Width, fieldSize.Height]; //if in the position i,j there is a 1, it is a mine
            createBoard(); //we create the board
        }

        private void createNameForm()
        {
            int padding = 30;

            lbName.Font = new Font("Arial", 14, FontStyle.Bold);
            lbName.BackColor = Color.FromArgb(7, 54, 66);
            lbName.ForeColor = Color.White;
            lbName.AutoSize = true;
            lbName.Text = "Introduce your username: ";
            lbName.Location = new Point(this.container.Left + padding, this.container.Bottom + padding);
            this.Controls.Add(lbName);

            //textBox to ask for the name
            textBoxName.Font = new Font("Arial", 14, FontStyle.Bold);
            textBoxName.BackColor = Color.FromArgb(7, 54, 66);
            textBoxName.ForeColor = Color.White;
            textBoxName.AutoSize = false;
            textBoxName.Size = new Size(160, 30);
            textBoxName.Location = new Point(lbName.Location.X + lbName.Size.Width + padding / 2, lbName.Location.Y - 5);
            this.Controls.Add(textBoxName);

            //button to show the statistics of the game
            showStatisticsButton.Font = new Font("Arial", 14, FontStyle.Bold);
            showStatisticsButton.BackColor = Color.FromArgb(7, 54, 66);
            showStatisticsButton.ForeColor = Color.FromArgb(147, 161, 161);
            showStatisticsButton.AutoSize = true;
            showStatisticsButton.MouseDown += new MouseEventHandler(showStatistics);
            showStatisticsButton.Text = "Show Statistics";
            showStatisticsButton.Location = new Point(this.container.Left + (this.container.Size.Width / 2) - (showStatisticsButton.Size.Width / 2) - padding - (padding / 2), this.container.Bottom + (2 * padding) + (padding / 2));
            this.Controls.Add(showStatisticsButton);
        }

        //function to create the labels with the mines left, the time and a button to reset
        private void createScore()
        {
            Font font = new Font("Arial", 22, FontStyle.Bold);

            //label to show the mine counter
            lbMineCount.Font = font;
            lbMineCount.BackColor = Color.FromArgb(7, 54, 66);
            lbMineCount.ForeColor = Color.Red;
            lbMineCount.AutoSize = true;
            this.Controls.Add(lbMineCount);

            //label to show the time
            lbTime.Font = font; 
            lbTime.BackColor = Color.FromArgb(7, 54, 66);
            lbTime.ForeColor = Color.FromArgb(147, 161, 161);
            lbTime.AutoSize = true;
            this.Controls.Add(lbTime);

            //combobox to select the level
            comboLevels.Font = new Font("Arial", 14, FontStyle.Bold);
            comboLevels.BackColor = Color.FromArgb(7, 54, 66);
            comboLevels.ForeColor = Color.FromArgb(147, 161, 161);
            comboLevels.AutoSize = true;
            comboLevels.Items.Add("Easy");
            comboLevels.Items.Add("Medium");
            comboLevels.Items.Add("Advanced");
            comboLevels.SelectedIndex = 1;
            comboLevels.SelectedIndexChanged += new EventHandler(changeLevel);
            this.Controls.Add(comboLevels);

            //button to reset the game
            reset.Font = font;
            reset.BackColor = Color.FromArgb(7, 54, 66);
            reset.ForeColor = Color.FromArgb(147, 161, 161);
            reset.AutoSize = true;
            reset.MouseDown += new MouseEventHandler(Reset);
            reset.Text = "Reset";
            //we do not show the reset button until the game starts
            reset.Enabled = false;
            reset.Visible = false;
            this.Controls.Add(reset);

            SetText();
        }

        //function to update the timer
        private void Timer_Tick(object sender, EventArgs e)
        {
            timerCounter++;
            SetText();
        }

        //function to create all the buttons, it means the board
        private void createBoard()
        {
            cellSize = this.container.Width / fieldSize.Width;
            for (int i = 0; i < fieldSize.Width; i++)
            {
                for (int j = 0; j < fieldSize.Height; j++)
                {
                    //we create new buttons with an auxiliar class
                    Button newButton = new AuxiliarButton();
                    newButton.Font = new Font("Arial", 18, FontStyle.Bold);
                    newButton.MouseDown += new MouseEventHandler(buttonBoardClick);
                    newButton.Enabled = true;
                    newButton.Location = new Point(this.container.Left + (cellSize * i), this.container.Top + (this.container.Height - (cellSize * fieldSize.Height)) + (cellSize * j));
                    newButton.Size = new Size(cellSize, cellSize);
                    newButton.FlatStyle = new FlatStyle();
                    newButton.FlatAppearance.BorderSize = 0;
                    newButton.BackColor = (i + j) % 2 == 0 ? Color.FromArgb(170, 215, 81) : Color.FromArgb(162, 209, 73);
                    newButton.Name = i.ToString() + "," + j.ToString();
                    this.Controls.Add(newButton);
                    btnArray[i, j] = newButton;
                }
            }
            generate = true;
        }

        private void SetContainerLocation()
        {
            int width = 500;
            int height = 600;
            container = new Rectangle(100, 23, width, height);
        }

        //function to change the mines location's value to 1
        private void Mines_Generate(int column, int row) 
        {
            int i = 0;
            List<string> already_mine = new List<string>();
            while (i < originalMineCount)
            {
                int column_add = rnd.Next(fieldSize.Width), row_add = rnd.Next(fieldSize.Height);
                string coords = column_add.ToString() + "," + row_add.ToString();
                bool let_generate = true;

                //esto se añade para hacer que el primero que pulsas sea un espacio en blanco
                for (int r = row - 1; r <= row + 1; r++)
                {
                    for (int c = column - 1; c <= column + 1; c++)
                    {
                        if (column_add == c && row_add == r)
                        {
                            let_generate = false;
                        }
                    }
                }

                //comprobamos que se haya creado y que en ese hueco no haya otra mina ya
                if (let_generate == true && !already_mine.Contains(coords))
                {
                    field[column_add, row_add] = 1;
                    already_mine.Add(coords);
                    i++;
                }
            }
        }

        //function to seperate the button's name and gets the row's and column's value and returns them in an array
        private int[] GetCoords(string coords) 
        {
            int[] result = new int[2];
            string[] coord = coords.Split(',');
            result[0] = int.Parse(coord[0]);
            result[1] = int.Parse(coord[1]);
            return result;
        }

        //function to disable all the buttons
        private void Ending()
        {
            foreach (Button button in btnArray)
            {
                button.Enabled = false;
            }
        }

        //function to check if the user has won
        private bool didWin()
        {
            //we check all the board looking for the mines 
            for (int i = 0; i < fieldSize.Width; i++)
            {
                for (int j = 0; j < fieldSize.Height; j++)
                {
                    //comprobamos si alguno de los que aun no se ha levantado NO es una bomba
                    if ((btnArray[i, j].BackColor == Color.FromArgb(170, 215, 81) || btnArray[i, j].BackColor == Color.FromArgb(162, 209, 73)) && field[i, j] != 1)
                    {
                        return false;
                    }
                }
            }

            //we store this game on the statistics file
            string name = "player1";
            if (textBoxName.Text != "")
                name = textBoxName.Text;
            string text = name + ";" + clickCount + ";" + timerCounter + ";w";
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(text);
            }

            return true;
        }

        //function to change the level, generate another board
        public void changeLevel(object sender, EventArgs e)
        {
            string text = (string) this.comboLevels.SelectedItem;
            int newSize = fieldSize.Width, newMines = originalMineCount; 
            switch (text)
            {
                case "Easy":
                    newSize = 10;
                    newMines = 15;
                    break;
                case "Medium":
                    newSize = 15;
                    newMines = 30;
                    break;
                case "Advanced":
                    newSize = 20;
                    newMines = 50;
                    break;
            }
            //if the size has changed, we update the board
            if(newSize != fieldSize.Width || newMines != originalMineCount)
            {
                fieldSize = new Size(newSize, newSize);
                originalMineCount = newMines;
                generateMinesweeper();
                SetText();
            }
        }

        //function to open the form to show the statistics
        public void showStatistics(object sender, MouseEventArgs e)
        {
            Form statisticsForm = new Statistics();
            statisticsForm.Show();
        }

        //function to restart the current level
        public void Reset(object sender, MouseEventArgs e)
        {
            field = new int[fieldSize.Width, fieldSize.Height];
            for (int i = 0; i < fieldSize.Width; i++)
            {
                for (int j = 0; j < fieldSize.Height; j++)
                {
                    btnArray[i, j].Text = "";
                    btnArray[i, j].BackColor = (i + j) % 2 == 0 ? Color.FromArgb(170, 215, 81) : Color.FromArgb(162, 209, 73);
                    btnArray[i, j].ForeColor = Color.Black;
                    btnArray[i, j].Enabled = true;
                }
            }

            generate = true;
            timerCounter = 0;
            timer.Enabled = false;
            mineCount = originalMineCount;
            reset.Enabled = false;
            reset.Visible = false;
            SetText();
        }

        //function to update the score
        private void SetText()
        {
            int padding = 10;
            int padding1 = 20;
            
            lbTime.Text = $"⏰: {timerCounter}";
            lbTime.Location = new Point(this.container.Right - lbTime.Size.Width - padding, this.container.Top + padding1);

            lbMineCount.Text = $"🏴: {mineCount}";
            lbMineCount.Location = new Point(this.container.Left + padding, this.container.Top + padding1);

            reset.Location = new Point(this.container.Left + (this.container.Width / 2) - (reset.Size.Width / 2), this.container.Top + padding1 + 20);

            comboLevels.Location = new Point(this.container.Left + (this.container.Width / 2) - (comboLevels.Size.Width / 2), this.container.Top);
        }

        //function to check if the coordinates are valid
        private bool isValid(int column, int row)
        {
            return (column <= fieldSize.Width - 1 && column >= 0 && row >= 0 && row <= fieldSize.Height - 1);
        }

        //function to count the mines in a 3x3 square
        private int check(int column, int row) 
        {
            int count = 0;
            for (int i = column - 1; i <= column + 1; i++)
            {
                for (int j = row - 1; j <= row + 1; j++)
                {
                    if (isValid(i, j) && field[i, j] == 1)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        //function to find mines recursively
        private void FindMines(int column, int row)
        {
            if (btnArray[column, row].Enabled)
            {
                btnArray[column, row].Enabled = false;
                btnArray[column, row].BackColor = (column + row) % 2 == 1 ? Color.FromArgb(215, 184, 153) : Color.FromArgb(229, 194, 159);
                int count = check(column, row);
                if (count == 0)
                {
                    for (int i = column - 1; i <= column + 1; i++)
                    {
                        for (int j = row - 1; j <= row + 1; j++)
                        {
                            if (isValid(i, j))
                            {
                                if (btnArray[i, j].Text == "🏴")
                                {
                                    btnArray[i, j].Text = "";
                                    mineCount++;
                                    lbMineCount.Text = $"🏴: {mineCount}";
                                }
                                FindMines(i, j);
                            }
                        }
                    }
                }
                else
                {
                    btnArray[column, row].ForeColor = colors[count - 1];
                    btnArray[column, row].Text = count.ToString();
                }
            }
        }

        //function to show all the mines when the user loses
        private void checkLeftMines()
        {
            for (int i = 0; i < fieldSize.Width; i++)
            {
                for (int j = 0; j < fieldSize.Height; j++)
                {
                    if (field[i, j] == 1 && btnArray[i, j].Text != "🏴")
                    {
                        btnArray[i, j].ForeColor = Color.Black;
                        btnArray[i, j].Text = "💣";
                    }
                    else if (field[i, j] == 0 && btnArray[i, j].Text == "🏴")
                    {
                        btnArray[i, j].Text = "❌";
                    }
                }
            }

            //we store the current game on the statistics file
            string name = "player1";
            if (textBoxName.Text != "")
                name = textBoxName.Text;
            string text = name + ";" + clickCount + ";" + timerCounter + ";d";
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(text);
            }
        }

        //function to do all the actions when you click into the board
        private void buttonBoardClick(object sender, MouseEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (e.Button == MouseButtons.Right)
            {
                if (clickedButton.Text == "🏴")
                {
                    mineCount++;
                    lbMineCount.Text = $"🏴: {mineCount}";
                    clickedButton.Text = "?";
                }
                else if (clickedButton.Text == "?")
                {
                    mineCount++;
                    lbMineCount.Text = $"🏴: {mineCount}";
                    clickedButton.Text = "";
                }
                else if (mineCount > 0)
                {
                    mineCount--;
                    lbMineCount.Text = $"🏴: {mineCount}";
                    clickedButton.Text = "🏴";
                }
                clickedButton.ForeColor = Color.Red;
            }
            else if (e.Button == MouseButtons.Left && clickedButton.Text != "🏴")
            {
                string name = clickedButton.Name;
                int column = GetCoords(name)[0];
                int row = GetCoords(name)[1];
                if (generate)
                {
                    Mines_Generate(column, row);
                    generate = false;
                    timer.Enabled = true;
                    reset.Enabled = true;
                    reset.Visible = true;
                }
                if (field[column, row] == 1)
                {
                    timer.Enabled = false;
                    //model.Mine = true;
                    checkLeftMines();
                    Ending();
                    MessageBox.Show($"       You have lost.\n\nTime elapsed: {timerCounter} second(s).\n\nTotal clicks: {clickCount} clicks.\n");
                    return;
                }
                FindMines(column, row);
                if (didWin())
                {
                    timer.Enabled = false;
                    Ending();
                    MessageBox.Show($"Congratulations, you have won!\n\nTime elapsed: {timerCounter} second(s).\n\nTotal clicks: {clickCount} clicks.\n");
                }
            }
            clickCount++;
        }

        //function to resize the window
        private void OnResize(object sender, EventArgs e)
        {
            for (int i = 0; i < fieldSize.Width; i++)
            {
                for (int j = 0; j < fieldSize.Height; j++)
                {
                    btnArray[i, j].Location = new Point
                    (
                        this.container.Left + (cellSize * i),
                        this.container.Top + (this.container.Height - (cellSize * fieldSize.Height)) + (cellSize * j)
                    );
                }
            }
            SetText();
        }
    }
}
