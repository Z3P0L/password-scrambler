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

            if (string.IsNullOrEmpty(password) || password == "Type your normal password")
            {
                txtOutput.Text = "[ERROR]: Insert a valid password.";
                txtPassword.Clear();
                return;
            }

            // Use PBKDF2 to derive a 256-bit key from the password
            byte[] salt = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] key = pbkdf2.GetBytes(32);

            // Encrypt a fixed plaintext string using AES-256
            byte[] iv = new byte[] { 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };
            byte[] plaintext = Encoding.UTF8.GetBytes("The quick brown fox jumps over the lazy dog");
            byte[] ciphertext;
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(plaintext, 0, plaintext.Length);
                        cs.FlushFinalBlock();
                        ciphertext = ms.ToArray();
                    }
                }
            }

            // Convert the ciphertext to a base64-encoded string and display it in txtOutput
            txtOutput.Text = Convert.ToBase64String(ciphertext);

            Clipboard.SetText(txtOutput.Text);
            MessageBox.Show("[INFO]: The generated password is in your clipboard.");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtOutput.Text);
            MessageBox.Show("[INFO]: Text copied to clipboard");
        }
    }
}
