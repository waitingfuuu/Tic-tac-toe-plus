using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToePlus
{
    public partial class Form1 : Form
    {
        int round;
        int player = 1;
        bool isMove;
        int place;
        int player1win = 0;
        int player2win = 0;
        int winRound = 1;
        string[] picBoxName = { "", "pictureBox1" , "pictureBox2" , "pictureBox3" , "pictureBox4",
                                "pictureBox5", "pictureBox6", "pictureBox7", "pictureBox8", "pictureBox9"};
        int[,] MovePlace = { {-1,-1,-1,-1,-1, -1, -1, -1, -1},
                               {1, 2, 4, 5, -1, -1, -1, -1, -1},
                               {2, 1, 3, 4, 5, 6, -1, -1, -1},
                               {3, 2, 5, 6, -1, -1, -1, -1, -1},
                               {4, 1, 2, 5, 7, 8, -1, -1, -1},
                               {5, 1, 2, 3, 4, 6, 7, 8, 9},
                               {6, 2, 3, 5, 8, 9, -1, -1, -1},
                               {7, 4, 5, 8, -1, -1, -1, -1, -1},
                               {8, 4, 5, 6, 7, 9, -1, -1, -1},
                               {9, 5, 6, 8, -1, -1, -1, -1, -1}};
        int[] picBoxPlayer = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        int[,] winNum = {{1, 2, 3},
                         {4, 5, 6 },
                         {7, 8, 9 },
                         {1, 4, 7 },
                         {2, 5, 8 },
                         {3, 6, 9 },
                         {1, 5, 9 },
                         {3, 5, 7 }};

        public Form1()
        {
            InitializeComponent();
        }

        private void Init()
        {
            BtnStart.Enabled = true;
            pictureBox1.Enabled = false;
            pictureBox2.Enabled = false;
            pictureBox3.Enabled = false;
            pictureBox4.Enabled = false;
            pictureBox5.Enabled = false;
            pictureBox6.Enabled = false;
            pictureBox7.Enabled = false;
            pictureBox8.Enabled = false;
            pictureBox9.Enabled = false;
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;
            pictureBox6.Image = null;
            pictureBox7.Image = null;
            pictureBox8.Image = null;
            pictureBox9.Image = null;

            isMove = false;
            round = 0;
            place = 0;
            for (int i = 0; i <= 9; i++)
                picBoxPlayer[i] = -1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            BtnStart.Text = "Next Round";
            BtnStart.Enabled = false;
            pictureBox1.Enabled = true;
            pictureBox2.Enabled = true;
            pictureBox3.Enabled = true;
            pictureBox4.Enabled = true;
            pictureBox5.Enabled = true;
            pictureBox6.Enabled = true;
            pictureBox7.Enabled = true;
            pictureBox8.Enabled = true;
            pictureBox9.Enabled = true;

            if (winRound % 2 == 1) player = 1;
            else player = 2;

            if (player == 1)
            {
                label1.ForeColor = Color.Red;
                label2.ForeColor = Color.Black;
            }
            else
            {
                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Red;
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            BtnStart.Text = "Start";
            Init();
            player1win = 0;
            player2win = 0;
            winRound = 1;
            player = 1;
            label1.ForeColor = Color.Red;
            label2.ForeColor = Color.Black;
            lblPlayer1.Text = "Win: " + player1win.ToString();
            lblPlayer2.Text = "Win: " + player2win.ToString();
        }

        // ------------------------------- Play -------------------------------
        private int ID(PictureBox pictureBox)
        {
            int n = 0;
            for (int i = 1; i <= 9; i++)
            {
                if (pictureBox.Name == picBoxName[i]) n = i;
            }
            return n;
        }

        private void draw(int Player, PictureBox pictureBox) //  畫圈圈叉叉
        {
            round++;
            if (Player == 1)
            {
                pictureBox.Image = imageList1.Images[0];
                picBoxPlayer[ID(pictureBox)] = 1;
                if (isWin(player))
                {
                    MessageBox.Show("Player 1 Win !!!");
                    player1win++;
                    lblPlayer1.Text = "Win: " + player1win.ToString();
                    Init();
                    winRound++;
                }
                else player = 2;
            }
            else
            {
                pictureBox.Image = imageList1.Images[1];
                picBoxPlayer[ID(pictureBox)] = 2;
                if (isWin(player))
                {
                    MessageBox.Show("Player 2 Win !!!");
                    player2win++;
                    lblPlayer2.Text = "Win: " + player2win.ToString();
                    Init();
                    winRound++;
                }
                else player = 1;
            }
        }

        private bool rightPlace(int Player, PictureBox pictureBox)
        {
            if ((Player == 1 && picBoxPlayer[ID(pictureBox)] == 1) ||
                (Player == 2 && picBoxPlayer[ID(pictureBox)] == 2)) return true;
            else return false;
        }

        private int choose(PictureBox pictureBox) // 選擇要移動的格子
        {
            string picBoxTxt = pictureBox.Name;
            int moveNum = 0;

            for (int i = 1; i <= 9; i++)
            {
                if (picBoxTxt == picBoxName[i])
                {
                    moveNum = i;
                    break;
                }
            }
            pictureBox.Image = null;
            picBoxPlayer[ID(pictureBox)] = -1;
            return moveNum;
        }

        private void move(int Player, PictureBox pictureBox, int Place) // 移動
        {
            bool flag = false;
            for (int i = 1; i <= 8; i++)
            {
                if (MovePlace[Place, i] > 0)
                {
                    if (pictureBox.Name == picBoxName[MovePlace[Place, i]] && pictureBox.Image == null) // 如果是可移動且為空的格子
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (flag)
            {
                draw(player, pictureBox);
                isMove = false;
            }
            else MessageBox.Show("You can't move here!");
        }

        private bool isWin(int Player)
        {
            for (int i = 0; i < 8; i++)
            {
                if ((picBoxPlayer[winNum[i, 0]] == Player) &&
                    (picBoxPlayer[winNum[i, 1]] == Player) &&
                    (picBoxPlayer[winNum[i, 2]] == Player))
                {
                    return true;
                }
            }
            return false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox picBox = (PictureBox)sender;
            string picBoxTxt = picBox.Name;

            if (round < 6)
            {
                if (picBox.Image == null)
                {
                    draw(player, picBox);
                }
                else MessageBox.Show("Already draw");
            }
            else
            {
                if (isMove == false)
                {
                    if (rightPlace(player, picBox))
                    {
                        place = choose(picBox);
                        isMove = true;
                    }
                    else MessageBox.Show("Not yours");
                }
                else
                {
                    if (picBox.Image == null)
                    {
                        move(player, picBox, place);
                    }
                    else MessageBox.Show("You can't move here!");
                }
            }

            if (player == 1)
            {
                label1.ForeColor = Color.Red;
                label2.ForeColor = Color.Black;
            }
            else
            {
                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Red;
            }
        }
    }
}
