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
        bool goLeft;        // Left control boolean
        bool goRight;       // Right control boolean
        bool isGameOver;    // Stop game boolean

        int score;          // Score of the player
        int ballX;          // Ball horizontal speed
        int ballY;          // Ball vertical speed
        int playerSpeed;    // Player speed

        Random rnd = new Random();

        PictureBox[] blockArray;

        public BreakoutGame()
        {
            // Initialize all the components
            InitializeComponent();

            // Call method to set blocks
            placeBlocks();
        }

        // Game setup method, to set game properties before starting the game
        private void setupGame()
        {
            score = 0;          // Starting score
            ballX = 2;          // Set ball horizontal speed
            ballY = 2;          // Set ball vertical speed
            playerSpeed = 12;   // Set player speed
            isGameOver= false;
            txtScore.Text = "Score: " + score;

            ball.Left = 300;    // Set ball horizontal starting position
            ball.Top = 250;     // Set ball vertical starting position

            gameTimer.Start();  // Start game

            foreach (Control block in this.Controls)
            {
                // Check if blocks exist
                if (block is PictureBox && (string)block.Tag == "blocks")
                {
                    // Set the color of the block
                    block.BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                }
            }
        }

        // Game over method to stop the game
        private void gameOver(string message)
        {
            isGameOver = true;
            gameTimer.Stop();

            txtScore.Text = "Score: " + score + " " + message;
        }

        // Generate blocks method
        private void placeBlocks()
        {
            blockArray = new PictureBox[28];    // Set number of blocks

            int a = 0;      // Number of blocks in each row
            int top = 50;   // Set padding from the top
            int left = 25;  // Set padding from the left

            // Iteration to generate each block
            for (int i = 0; i < blockArray.Length; i++)
            {
                blockArray[i] = new PictureBox();
                blockArray[i].Height = 15;              // Set block height
                blockArray[i].Width = 60;               // Set block width
                blockArray[i].Tag = "blocks";           // Set block tag
                blockArray[i].BackColor = Color.White;  // Set starting block color

                // Set properties to make new rows of blocks
                if (a == 7)
                {
                    top = top + 30;
                    left = 25;
                    a = 0;
                }
                if (a < 7)
                {
                    a++;
                    blockArray[i].Left = left;
                    blockArray[i].Top = top;
                    this.Controls.Add(blockArray[i]);
                    left = left + 80;
                }

                // Call method to set game properties
                setupGame();
            }
        }

        private void removeBlock()
        {
            foreach(PictureBox block in blockArray)
            {
                this.Controls.Remove(block);
            }
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
            ball.Left += ballX;
            ball.Top += ballY;

            // The ball interacts with the edge
            if (ball.Left < 0 || ball.Left > 570)
            {
                ballX = -ballX;
            }
            if (ball.Top < 0)
            {
                ballY = -ballY;
            }

            // The ball interact with the player will random speed from 2 to 8 on both axes
            if (ball.Bounds.IntersectsWith(player.Bounds))
            {
                ballY = rnd.Next(2, 8) * -1;

                if (ballX < 0)
                {
                    ballX = rnd.Next(2, 8) * -1;
                }
                else
                {
                    ballX = rnd.Next(2, 8);
                }
            }

            foreach (Control block in this.Controls)
            {
                // Check if blocks exist
                if (block is PictureBox && (string)block.Tag == "blocks")
                {
                    if (ball.Bounds.IntersectsWith(block.Bounds))
                    {
                        score += 1;

                        ballY = -ballY;

                        this.Controls.Remove(block);
                    }
                }
            }

            // Check if the player finishes the game, then stop the game
            if (score == 28)
            {
                gameOver("You win!!!   Press Enter to Play Again");
            }

            // Check if the player loses the game, then stop the game
            if (ball.Top > 400)
            {
                gameOver("You lose!!!   Press Enter to Try Again");
            }
        }

        // Check if the player pressing the button method
        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }

        }

        // Check if the player releasing buttons method
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

            // When the game is over, check if the player presses Enter then restart the game
            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                removeBlock();
                placeBlocks();
            }
        }
    }
}
