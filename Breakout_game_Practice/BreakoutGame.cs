using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Breakout_game_Practice
{
    public partial class BreakoutGame : Form
    {
        bool goLeft;
        bool goRight;
        bool isGameOver;

        int score;
        int ballx;
        int bally;
        int playerSpeed;

        Random rnd = new Random();

        PictureBox[] blockArray;

        public BreakoutGame()
        {
            // Initialize all the components
            InitializeComponent();

            // Call function to setup the game
            setupGame();
        }

        // Game setup before start the game
        private void setupGame()
        {
            score = 0;          // Start score
            ballx = 2;          // Ball horizontal spped
            bally = 2;          // Ball vertical speed
            playerSpeed = 12;   // Player speed
            txtScore.Text = "Score: " + score;

            gameTimer.Start();

            foreach (Control x in this.Controls)
            {
                // Check if blocks are exist.
                if (x is PictureBox && (string)x.Tag == "blocks")
                {
                    // Set color of the block
                    x.BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                }
            }
        }
        private void gameOver(string message)
        {
            isGameOver = true;
            gameTimer.Stop();

            txtScore.Text = "Score: " + score + " " + message;
        }

        private void placeBlocks()
        {
            blockArray = new PictureBox[28];

            int a = 0;
            int top = 50;
            int left = 100;
        }


        // Playing game events
        private void mainGameTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score;

            // Player left move
            if (goLeft == true && player.Left > 0)
            {
                player.Left -= playerSpeed;
            }

            // Player right move
            if (goRight == true && player.Left < 510)
            {
                player.Left += playerSpeed;
            }

            // Ball move
            ball.Left += ballx;
            ball.Top += bally;

            // Ball interact with edge
            if (ball.Left < 0 || ball.Left > 570)
            {
                ballx = -ballx;
            }
            if (ball.Top < 0)
            {
                bally = -bally;
            }

            // Ball interact with player will random speed from 2 to 8 in both axes
            if (ball.Bounds.IntersectsWith(player.Bounds))
            {
                bally = rnd.Next(2, 8) * -1;

                if (ballx < 0)
                {
                    ballx = rnd.Next(2, 8) * -1;
                }
                else
                {
                    ballx = rnd.Next(2, 8);
                }
            }

            foreach (Control x in this.Controls)
            {
                // Check if blocks are exist.
                if (x is PictureBox && (string)x.Tag == "blocks")
                {
                    if (ball.Bounds.IntersectsWith(x.Bounds))
                    {
                        score += 1;

                        bally = -bally;

                        this.Controls.Remove(x);
                    }
                }
            }

            if (score == 28)
            {
                gameOver("You win!!!");
            }

            if (ball.Top > 400)
            {
                gameOver("You lose!!!");
            }
        }

        // Check if press buttons
        private void keyisdown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if(e.KeyCode == Keys.Right)
            {
                goRight = true;
            }

        }

        // Check if release buttons
        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
        }

        private void player_Click(object sender, EventArgs e)
        {

        }
    }
}
