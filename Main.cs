using PasswordScrambler.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordScrambler
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnScramble_Click(object sender, EventArgs e)
        {
            string password = txtPassword.Text;
            string scrambledPassword = Scrambler.ScramblePassword(password);
            txtOutput.Text = scrambledPassword;

            Clipboard.SetText(scrambledPassword);
            if (scrambledPassword != "[ERROR]: Insert a valid password.")
                MessageBox.Show("[INFO]: The generated password is in your clipboard.");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (txtOutput.Text != "")
            {
                Clipboard.SetText(txtOutput.Text);
                MessageBox.Show("[INFO]: Text copied to clipboard");
            }
        }
    }
}
