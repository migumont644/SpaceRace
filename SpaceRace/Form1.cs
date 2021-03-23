using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace SpaceRace
{
    public partial class spaceRaceTitle : Form
    {
        List<int> asteroidsXLeftList = new List<int>();
        List<int> asteroidsYLeftList = new List<int>();

        List<int> asteroidsXRightList = new List<int>();
        List<int> asteroidsYRightList = new List<int>();

        int asteroidsHeight = 5;
        int asteroidsWidth = 10;
        int asteroidsSpeed = 8;

        int playerOneX = 250;
        int playerTwoX = 550;

        int playerOneY = 585;
        int playerTwoY = 585;

        int spaceShipHeight = 20;
        int spaceShipWidth = 10;
        int spaceShipSpeed = 6;

        bool upDown = false;
        bool downDown = false;

        bool wDown = false;
        bool sDown = false;

        Random randGen = new Random();
        int randValueLeft = 0;
        int randValueRight = 0;

        int scorePlayerOne = 0;
        int scorePlayerTwo = 0;

        string gameState = "waiting";
        string playerWon = "";

        SolidBrush whiteBrush = new SolidBrush(Color.White);

        SoundPlayer hitSound = new SoundPlayer(Properties.Resources.hitSound);
        SoundPlayer startSound = new SoundPlayer(Properties.Resources.startSound);
        SoundPlayer endSound = new SoundPlayer(Properties.Resources.reachTheEndSound);
        SoundPlayer upSound = new SoundPlayer(Properties.Resources.goingUp);
        SoundPlayer downSound = new SoundPlayer(Properties.Resources.goingDown);
        public spaceRaceTitle()
        {
            InitializeComponent();
        }
        public void GameInitialize()
        {
            startSound.Play();

            titleLabel.Text = "";
            subTitleLabel.Text = "";

            gameTimer.Enabled = true;
            gameState = "running";

            asteroidsXLeftList.Clear();
            asteroidsYLeftList.Clear();
            asteroidsXRightList.Clear();
            asteroidsYRightList.Clear();

            scorePlayerOne = 0;
            scorePlayerTwo = 0;

            playerWon = "";

            playerOneY = 585;
            playerTwoY = 585;
        }
            private void SpaceRaceTitle_KeyDown(object sender, KeyEventArgs e)
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
                    if (gameState == "waiting" || gameState == "over")
                    {
                        GameInitialize();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "waiting" || gameState == "over")
                    {
                        Application.Exit();
                    }
                    break;
            }
        }

        private void SpaceRaceTitle_KeyUp(object sender, KeyEventArgs e)
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

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (upDown == true)
            {
                playerOneY -= spaceShipSpeed;
            }

            if (downDown == true && playerOneY < 590)
            {
                playerOneY += spaceShipSpeed;
            }
            if (wDown == true)
            {             
                playerTwoY -= spaceShipSpeed;
            }

            if (sDown == true && playerTwoY < 590)
            {        
                playerTwoY += spaceShipSpeed;
            }

            randValueLeft = randGen.Next(0, 101);
            if (randValueLeft < 25)
            {
                asteroidsYLeftList.Add(randGen.Next(10, this.Height - 30));
                asteroidsXLeftList.Add(0);
            }

            for (int i = 0; i < asteroidsYLeftList.Count(); i++)
            {
                asteroidsXLeftList[i] += asteroidsSpeed;
            }

            randValueRight = randGen.Next(0, 101);
            if (randValueRight < 25)
            {
                asteroidsYRightList.Add(randGen.Next(10, this.Height - 30));
                asteroidsXRightList.Add(800);
            }

            for (int i = 0; i < asteroidsYRightList.Count(); i++)
            {
                asteroidsXRightList[i] -= asteroidsSpeed;
            }

            for (int i = 0; i < asteroidsXLeftList.Count(); i++)
            {
                if (asteroidsXLeftList[i] > 800)
                {
                    asteroidsXLeftList.RemoveAt(i);
                    asteroidsYLeftList.RemoveAt(i);
                    break;
                }
            }

            for (int i = 0; i < asteroidsXRightList.Count(); i++)
            {
                if (asteroidsXRightList[i] < 0)
                {
                    asteroidsXRightList.RemoveAt(i);
                    asteroidsYRightList.RemoveAt(i);
                    break;
                }
            }

            Rectangle playerOneRec = new Rectangle(playerOneX, playerOneY, spaceShipWidth, spaceShipHeight);
            Rectangle playerTwoRec = new Rectangle(playerTwoX, playerTwoY, spaceShipWidth, spaceShipHeight);
            for (int i = 0; i < asteroidsYLeftList.Count(); i++)
            {
                Rectangle asteroidLeftRec = new Rectangle(asteroidsXLeftList[i], asteroidsYLeftList[i], asteroidsWidth, asteroidsHeight);
                if (playerOneRec.IntersectsWith(asteroidLeftRec))
                {
                    hitSound.Play();
                    playerOneY = 585;
                    break;
                }
                else if (playerTwoRec.IntersectsWith(asteroidLeftRec))
                {
                    hitSound.Play();
                    playerTwoY = 585;
                    break;
                }
            }
            for (int i = 0; i < asteroidsYRightList.Count(); i++)
            {
                Rectangle asteroidRightRec = new Rectangle(asteroidsXRightList[i], asteroidsYRightList[i], asteroidsWidth, asteroidsHeight);
                if (playerOneRec.IntersectsWith(asteroidRightRec))
                {
                    hitSound.Play();
                    playerOneY = 585;
                    break;
                }
                else if (playerTwoRec.IntersectsWith(asteroidRightRec))
                {
                    hitSound.Play();
                    playerTwoY = 585;
                    break;
                }
            }
            playerOneScore.Text = $"{scorePlayerOne}";
            playerTwoScore.Text = $"{scorePlayerTwo}";
            if (playerOneY < 0)
            {
                endSound.Play();
                scorePlayerOne++;
                playerOneScore.Text = $"{scorePlayerOne}";
                playerOneY = 585;
            }
            if (playerTwoY < 0)
            {
                endSound.Play();
                scorePlayerTwo++;
                playerTwoScore.Text = $"{scorePlayerTwo}";
                playerTwoY = 585;
            }
            if (scorePlayerOne == 3)
            {
                playerWon = "Player 1";
                gameTimer.Enabled = false;
                gameState = "over";
            }
            if (scorePlayerTwo == 3)
            {
                playerWon = "Player 2";
                gameTimer.Enabled = false;
                gameState = "over";
            }
            Refresh();
        }     
        private void SpaceRaceTitle_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "running")
            {
                e.Graphics.FillRectangle(whiteBrush, playerOneX, playerOneY, spaceShipWidth, spaceShipHeight);
                e.Graphics.FillRectangle(whiteBrush, playerTwoX, playerTwoY, spaceShipWidth, spaceShipHeight);
                for (int i = 0; i < asteroidsXLeftList.Count(); i++)
                {
                    e.Graphics.FillEllipse(whiteBrush, asteroidsXLeftList[i], asteroidsYLeftList[i], asteroidsWidth, asteroidsHeight);
                }
                for (int i = 0; i < asteroidsXRightList.Count(); i++)
                {
                    e.Graphics.FillEllipse(whiteBrush, asteroidsXRightList[i], asteroidsYRightList[i], asteroidsWidth, asteroidsHeight);
                }
            }
                if (gameState == "waiting")
            {
                titleLabel.Text = "SPACE RACE";
                subTitleLabel.Text = "Press Space to Start or Escape to Exit";
            }
            else if (gameState == "over")
            {
                titleLabel.Text = "GAME OVER";
                subTitleLabel.Text = $"{playerWon} is the winner! \n Press space to start or escape to exit";
            }



        }
    }
}
