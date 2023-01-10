using System;
using System.Drawing;
using System.Windows.Forms;

namespace Saper
{
    partial class Form1
    {
        private const int mapSize = 10;
        private const int blockSize = 80;

        private int[,] map = new int[mapSize, mapSize];

        private Button[,] buttons = new Button[mapSize, mapSize];

        private Image bomb;
        private Image flag;
        private Image closed;
        private Image empty;
        private Image one;
        private Image two;
        private Image three;
        private Image four;
        private Image five;
        private Image six;
        private Image seven;
        private Image eight;

        Point startPos;

        private bool isStart = false;

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Text = "Saper";

            LoadImageFromStorage();
            SetFormSize();
            RefreshMap();
            SetButtonsSettings();
        }

        private void SetFormSize()
        {
            this.Width = mapSize * blockSize + 20;
            this.Height = mapSize * blockSize + 40;
        }

        private void RefreshMap()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    map[i, j] = 0;
                }
            }
        }

        private void LoadImageFromStorage()
        {
            bomb = Image.FromFile("Pictures\\bomb.png");
            flag = Image.FromFile("Pictures\\flag.png");
            closed = Image.FromFile("Pictures\\lock.png");
            empty = Image.FromFile("Pictures\\unlock.png");
            one = Image.FromFile("Pictures\\1.png");
            two = Image.FromFile("Pictures\\2.png");
            three = Image.FromFile("Pictures\\3.png");
            four = Image.FromFile("Pictures\\4.png");
            five = Image.FromFile("Pictures\\5.png");
            six = Image.FromFile("Pictures\\6.png");
            seven = Image.FromFile("Pictures\\7.png");
            eight = Image.FromFile("Pictures\\8.png");
        }

        private void SetButtonsSettings()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    Button button = new Button();
                    button.Location = new Point(j * blockSize, i * blockSize);
                    button.Size = new Size(blockSize, blockSize);
                    button.Image = closed;
                    button.MouseUp += new MouseEventHandler(OnButtonClick);
                    this.Controls.Add(button);
                    buttons[i, j] = button;
                }
            }
        }

        private void OnButtonClick(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;

            switch (e.Button)
            {
                case MouseButtons.Left:
                    LeftClick(button);
                    break;
                case MouseButtons.Right:
                    RightClick(button);
                    break;
            }
        }

        private void LeftClick(Button button)
        {
            int posX = button.Location.Y / blockSize;
            int posY = button.Location.X / blockSize;

            if (!isStart)
            {
                startPos = new Point(posX, posY);
                SetBomb();
                GetBombCountArroundBlock();
                isStart = true;
            }

            OpenPossibleBlocks(posX, posY);

            if (map[posX, posY] == -1){
                ShowAllBombs(posX, posY);
                MessageBox.Show("Вы проиграли");
            }
        }

        private void RightClick(Button button)
        {
            if (!isStart)
                return;

            if (button.Image == closed)
            {
                button.Image = flag;
            }
            else
            {
                button.Image = closed;
            }
        }

        private void ShowAllBombs(int posX, int posY)
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (i == posX && j == posY)
                        continue;
                    if (map[i, j] == -1)
                    {
                        buttons[i, j].Image = bomb;
                    }
                }
            }
        }

        private void OpenOneBlock(int x, int y)
        {
            buttons[x, y].Enabled = false;

            switch (map[x, y])
            {
                case 1:
                    buttons[x, y].Image = one;
                    break;  
                case 2:     
                    buttons[x, y].Image = two;
                    break;  
                case 3:     
                    buttons[x, y].Image = three;
                    break;  
                case 4:    
                    buttons[x, y].Image = four;
                    break;  
                case 5:     
                    buttons[x, y].Image = five;
                    break;  
                case 6:     
                    buttons[x, y].Image = six;
                    break;  
                case 7:     
                    buttons[x, y].Image = seven;
                    break;  
                case 8:     
                    buttons[x, y].Image = eight;
                    break;  
                case -1:    
                    buttons[x, y].Image = bomb;
                    break;  
                case 0:     
                    buttons[x, y].Image = empty;
                    break;
            }
        }

        private void OpenPossibleBlocks(int x, int y)
        {
            OpenOneBlock(x, y);

            if (map[x, y] > 0)
                return;

            for (int i = x - 1; i < x + 2; i++)
            {
                for (int j = y - 1; j < y + 2; j++)
                {
                    if (IsNull(i, j))
                        continue;
                    if (!buttons[i, j].Enabled)
                        continue;
                    if (map[i, j] == 0)
                        OpenPossibleBlocks(i, j);
                    else if (map[i, j] > 0)
                        OpenOneBlock(i, j);
                }
            }
        }

        private void SetBomb()
        {
            Random random = new Random();

            int bombCount = random.Next(16,18);

            for (int i = 0; i < bombCount; i++)
            {
                int posX = random.Next(mapSize - 1);
                int posY = random.Next(mapSize - 1);

                while (map[posX, posY] == -1 || (Math.Abs(posX - startPos.X) <= 1 && Math.Abs(posY - startPos.Y) <= 1))
                {
                    posX = random.Next(mapSize - 1);
                    posY = random.Next(mapSize - 1);
                }

               
                map[posX, posY] = -1;
            }
        }

        private void GetBombCountArroundBlock()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i, j] == -1)
                    {
                        for (int k = i - 1; k < i + 2; k++)
                        {
                            for (int l = j - 1; l < j + 2; l++)
                            {
                                if (IsNull(k, l) || map[k, l] == -1)
                                    continue;
                                map[k, l] = map[k, l] + 1;
                            }
                        }
                    }
                }
            }
        }

        private static bool IsNull(int i, int j)
        {
            if (i < 0 || j < 0 || j > mapSize - 1 || i > mapSize - 1)
            {
                return true;
            }
            return false;
        }
    }
}

