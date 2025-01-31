using System;
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
            // The MiniMax algorithm returns a optimal_value as a row:column tuple.
            // Perhaps we could map the buttons to an array? Then access that array using the tuple?

            // On a button click we are responding to the human's optimal_value choice.
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
            Console.Write("...human moves...\n");
            buttonMatrix[row, column].Text = "X";
            buttonMatrix[row, column].Enabled = false;

            // Update the miniMax board to match the button matrix.
            // The miniMax board is the computer's view of the game state.
            miniMax.board[row, column] = 'x';
            miniMax.drawTheBoard(miniMax.board);

            turnCount++;//for the draws, the maxium play is 9
            if (checkWinner(true, 0))
            {
                // Stop the game if there is a winner or a draw.
                return;
            }

            // Switch to the computer as player.
            Console.Write("...computer moves...\n");

            // Computer makes a optimal_value.
            Move bestMove = findBestMove(miniMax.board, 'o', 'x');

            // If the computer has no moves then the returned values are:
            //   bestMove.row == -1;
            //   bestMove.column == -1;
            //   bestMove.optimal_value == -1000;

            // If the computer has made a winning optimal_value then the returned values are:
            //   bestMove.row == ?;
            //   bestMove.column == ?;
            //   bestMove.optimal_value == 10;

            if (bestMove.optimal_value < 0)
            {
                Console.Write("...no available moves for the computer : {0}!\n", bestMove.optimal_value);

                if (checkWinner(false, bestMove.optimal_value))
                {
                    // Stop the game if there is a winner or a draw.
                    return;
                }
            }

            // Need to translate from bestMove.row|column to buttonMatrix.
            // Computer is 'O'
            buttonMatrix[bestMove.row, bestMove.col].Text = "O";
            buttonMatrix[bestMove.row, bestMove.col].Enabled = false;

            // Update the miniMax board.
            miniMax.board[bestMove.row, bestMove.col] = 'o';
            miniMax.drawTheBoard(miniMax.board);

            Console.Write("...and the optimal score is {0}!\n", bestMove.optimal_value);

            turnCount++;//for the draws, the maxium play is 9
            if (checkWinner(false, bestMove.optimal_value))
            {
                // Stop the game if there is a winner or a draw.
                return;
            }
        }

        private bool checkWinner(bool isHuman, int score)
        {
            /*
            foreach(Control x in Controls)//emunmeration of all controls{}
            */
            bool weHaveWinner = ((score == 10) ? true : false);

            if (weHaveWinner)
            {
                //disableAllbtn();

                String winner = "";

                if (isHuman)
                    winner = "X";
                else
                    winner = "O";
                updateWinStreak(winner);

                Console.Write("...and the winner is {0}!\n", winner);
                MessageBox.Show(winner + " Wins!", "GG");
                autoNewGame();
                return true;
            }
            else
            {
                if (turnCount == 9)
                {
                    Console.Write("...we have a draw!\n");
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
                Console.Write("\nNEW GAME!\n");
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
