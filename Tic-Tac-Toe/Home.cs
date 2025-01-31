using System;
using System.Windows.Forms;

namespace Tic_Tac_Toe
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
