using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceRace
{
    public partial class Form1 : Form
    {
        Rectangle hero = new Rectangle(100, 360, 20, 40);
        Rectangle hero2 = new Rectangle(500, 360, 20, 40);
        int heroSpeed = 10;

        List<Rectangle> balls = new List<Rectangle>();
        List<Rectangle> balls2 = new List<Rectangle>();

        int ballSize = 10;
        int ballSpeed = 6;
        
        int score = 0;
        int time = 500;

        bool downDown = false;
        bool upDown = false;
        bool wDown = false;
        bool sDown = false;

        SolidBrush whiteBrush = new SolidBrush(Color.White);
       
        Random randGen = new Random();
        int randValue = 0;

        string gameState = "waiting";
        public Form1()
        {
            InitializeComponent();
        }

        public void GameSetup()
        {
            gameState = "running";

            titleLabel.Text = "";
            subtitleLabel.Text = "";

            gameLoop.Enabled = true;
            time = 500;
            score = 0;

            hero.Y = 360;
            hero2.Y = 360;
            
            balls.Clear();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upDown = true;
                    break;
                case Keys.Down:
                    downDown = true;
                    break;
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;

                case Keys.Space:
                    if (gameState == "waiting")
                    {
                        GameSetup();
                    }
                    if (gameState == "over")
                    {
                        GameSetup();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "waiting")
                    {
                        this.Close();
                    }
                    if (gameState == "over")
                    {
                        this.Close();
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upDown = false;
                    break;
                case Keys.Down:
                    downDown = false;
                    break;
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "waiting")
            {
                titleLabel.Text = "Space Race";
                subtitleLabel.Text = "Press Space to Start or Esc to Exit";
            }
            if (gameState == "running")
            {
                //draw hero 
                e.Graphics.FillRectangle(whiteBrush, hero);
                e.Graphics.FillRectangle(whiteBrush, hero2);

                //draw balls 
                for (int i = 0; i < balls.Count(); i++)
                {
                    e.Graphics.FillEllipse(whiteBrush, balls[i]);
                }
                for (int i = 0; i < balls2.Count(); i++)
                {
                    e.Graphics.FillEllipse(whiteBrush, balls2[i]);
                }
            }
            else if (gameState == "over")
            {
                titleLabel.Text = "Game Over";
                subtitleLabel.Text = "To play again, Press Space. To exit, Press Esc";
            }
        }

        private void gameLoop_Tick(object sender, EventArgs e)
        {
            if (upDown == true && hero.Y > 0)
            {
                hero.Y -= heroSpeed;
            }
            if (downDown == true && hero.Y < 400 - hero.Height)
            {
                hero.Y += heroSpeed;
            }
            if (wDown == true && hero2.Y > 0)
            {
                hero2.Y -= heroSpeed;
            }
            if (sDown == true && hero2.Y < 400 - hero.Height)
            {
                hero2.Y += heroSpeed;
            }

            for (int i = 0; i < balls.Count; i++)
            {
                int x = balls[i].X + ballSpeed;
                balls[i] = new Rectangle(x, balls[i].Y, ballSize, ballSize);
            }
            for (int i = 0; i < balls2.Count; i++)
            {
                int y = balls2[i].X - ballSpeed;
                balls2[i] = new Rectangle(y, balls2[i].Y, ballSize, ballSize);
            }

            //generate a random value
            randValue = randGen.Next(1, 101);

            //generate new ball if it is time
            if (randValue < 21)
            {
                balls.Add(new Rectangle(0, randGen.Next(0, this.Height - 50), ballSize, ballSize));
            }
            else if (randValue < 42)
            {
                balls2.Add(new Rectangle(600, randGen.Next(0, this.Height - 50), ballSize, ballSize));
            }

            //remove ball if it goes off the screen, (test at y = 400)
            for (int i = 0; balls.Count > i; i++)
            {
                if (balls[i].X >= this.Width)
                {
                    balls.RemoveAt(i);
                }
            }
            for (int i = 0; balls2.Count > i; i++)
            {
                if (balls2[i].X <= 0)
                {
                    balls2.RemoveAt(i);
                }
            }

            for (int i = 0; balls.Count > i; i++)
            {
                if (hero.IntersectsWith(balls[i]))
                {
                    hero.Y = 360;
                    balls.RemoveAt(i);
                }

                else if (hero2.IntersectsWith(balls[i]))
                {
                    hero2.Y = 360;
                    balls.RemoveAt(i);
                }
            }
            for (int i = 0; balls2.Count > i; i++)
            {
                if (hero.IntersectsWith(balls2[i]))
                {
                    hero.Y = 360;
                    balls2.RemoveAt(i);
                }

                else if (hero2.IntersectsWith(balls2[i]))
                {
                    hero2.Y = 360;
                    balls2.RemoveAt(i);
                }
            }

            //decrease time
            time--;

            //end game if time runs out
            if (time <= 0)
            {
                gameLoop.Enabled = false;
                gameState = "over";
            }

            if(hero.Y == 0)
            {
                gameState = "over";
                gameLoop.Enabled = false;
                titleLabel.Text = "Player 1 WINS";
            }
            if (hero2.Y == 0)
            {
                gameState = "over";
                gameLoop.Enabled = false;
                titleLabel.Text = "Player 2 WINS";
            }
            Refresh();
        }
    }
    }

