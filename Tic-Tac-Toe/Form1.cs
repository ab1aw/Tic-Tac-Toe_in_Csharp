using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Tic_Tac_Toe.MiniMax;

namespace Tic_Tac_Toe
{
    public partial class Form1 : Form
    {

        int turnCount = 0;
        int winStreak = 0; // Positive for X, Negative for O

        MiniMax miniMax;

        public Form1()
        {
            Home startPage = new Home();
            startPage.ShowDialog();
            InitializeComponent();
            initializeButtonMatrix();
            updateWinStreak("");

            miniMax = new MiniMax();

        }

        private void buttonClick(object sender, EventArgs e)
        {
            Button theButton = (Button)sender;

            // This may be the place to integrate the MiniMax algorithm.
            // We are switching between the two human players here.
            // We could modify this to switch between a human player and the computer.
            // How would we translate the MiniMax algorithm output into the corresponding button push?

            // The sender (aka theButton) needs to be derived from the MiniMax algorithm output value.
            // The MiniMax algorithm returns a move as a row:column tuple.
            // Perhaps we could map the buttons to an array? Then access that array using the tuple?

            // On a button click we are responding to the human's move choice.
            // We need to translate that back into a board state. This requires translating
            // the button into a row|column tuple.

            // Instead of the if-else, we want to react to the human's button push,
            // first updating the button state, then updating the board state,
            // then finally having the MiniMax algorithm react to the new board state.

            // The human always starts first. Perhaps rename this to humanTurn?

            // How do we map theButton to the buttonMatix (and thus to the board)?
            // Perhaps based on the .Name field?
            // Need to convert the button name to a [row, column] tuple.
            // Going to be yet another hack.

            char[] rowAndColumn = theButton.Name.ToCharArray();
            int row = rowAndColumn[0] - 'A';
            int column = rowAndColumn[1] - '1';

            // Human moves first as 'X'
            buttonMatrix[row, column].Text = "X";
            buttonMatrix[row, column].Enabled = false;

            // Update the miniMax board to match the button matrix.
            // The miniMax board is the computer's view of the game state.
            miniMax.board[row, column] = 'x';
            miniMax.drawTheBoard(miniMax.board);

            turnCount++;//for the draws, the maxium play is 9
            if (checkWinner(true))
            {
                // Stop the game if there is a winner or a draw.
                return;
            }

            // Switch to the computer as player.

            // Computer makes a move.
            Move bestMove = findBestMove(miniMax.board, 'o', 'x');

            // Need to translate from bestMove.row|column to buttonMatrix.
            // Computer is 'O'
            buttonMatrix[bestMove.row, bestMove.col].Text = "O";
            buttonMatrix[bestMove.row, bestMove.col].Enabled = false;

            // Update the miniMax board.
            miniMax.board[bestMove.row, bestMove.col] = 'o';
            miniMax.drawTheBoard(miniMax.board);

            //            int optimalScore = bestMove.move;

            turnCount++;//for the draws, the maxium play is 9
            if (checkWinner(false))
            {
                // Stop the game if there is a winner or a draw.
                return;
            }
        }

        private bool checkWinner(bool isHuman)
        {
            /*
            foreach(Control x in Controls)//emunmeration of all controls{}
            */
            bool weHaveWinner = false;

            //Switch possible ?

            //---
            if ((A1.Text == A2.Text) && (A2.Text == A3.Text) && (!A2.Enabled))
                weHaveWinner = true;
            else if ((B1.Text == B2.Text) && (B2.Text == B3.Text) && (!B2.Enabled))
                weHaveWinner = true;
            else if ((C1.Text == C2.Text) && (C2.Text == C3.Text) && (!C2.Enabled))
                weHaveWinner = true;

            // |||
            else if ((A1.Text == B1.Text) && (B1.Text == C1.Text) && (!B1.Enabled))
                weHaveWinner = true;
            else if ((A2.Text == B2.Text) && (B2.Text == C2.Text) && (!B2.Enabled))
                weHaveWinner = true;
            else if ((A3.Text == B3.Text) && (B3.Text == C3.Text) && (!B3.Enabled))
                weHaveWinner = true;

            //X
            else if ((A1.Text == B2.Text) && (B2.Text == C3.Text) && (!B2.Enabled))
                weHaveWinner = true;
            else if ((A3.Text == B2.Text) && (B2.Text == C1.Text) && (!B2.Enabled))
                weHaveWinner = true;
              

            if (weHaveWinner)
            {
                //disableAllbtn();

                String winner = "";

                if (isHuman)
                    winner = "X";
                else
                    winner = "O";
                updateWinStreak(winner);

                MessageBox.Show(winner + " Wins!", "GG");
                autoNewGame();
                return true;
            }
            else
            {
                if (turnCount == 9)
                {
                    MessageBox.Show("Draw");
                    //disableAllbtn();
                    autoNewGame();
                    return true;
                }

            }

            return false;
        }

        /*
        private void disableAllbtn()
        {
            try
            {
                foreach (Control c in Controls)
                {
                    Button b = (Button)c;
                    b.Enabled = false;
                }
            }
            catch { }//Ignoring the menu strip (is not a button)
        }*/

        private void autoNewGame()
        {
            try
            {
                // Clear the miniMax board.
                miniMax.resetTheBoard();

                foreach (Control c in Controls)
                {
                    // This check is important. You can not rely on the exception handling to only process buttons.
                    // If another type of control is encountered, the exception handling will cause this code to exit
                    // without necessarily clearing all the buttons.
                    if (c is Button)
                    {
                        (c as Button).Enabled = true;
                        (c as Button).Text = "";
                    }
                }

                turnCount = 0;
            }
            catch { }
        } 

        /// <summary>
        /// Updates the label indicating the current win streak
        /// </summary>
        /// <param name="winner">Can take "X" or "O" or any other value to reset</param>
        private void updateWinStreak(string winner)
        {
            if (winner == "X")
            {
                if (winStreak < 0) // If O was on a win streak, zero it before incrementing
                    winStreak = 0;
                winStreak++;
            }
            else if (winner == "O")
            {
                if (winStreak > 0) // If X was on a win streak, zero it before decrementing
                    winStreak = 0;
                winStreak--;
            }
            else
            {
                winStreak = 0;
            }

            winStreakLabel.Visible = (winStreak != 0);
            winStreakLabel.Text = String.Format("{0} is on a win streak of {1}", winner, Math.Abs(winStreak));
        }

        private void winStreakLabel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset the win streak?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                updateWinStreak("");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
