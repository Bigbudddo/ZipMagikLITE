using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZipMagikLITE.Views {
    public partial class Password : Form {

        public bool Required { get; private set; }
        public string Value { get; set; }

        public Password(string archiveName, bool required) {
            InitializeComponent();
            this.Required = required;

            lblMessage.Text = string.Format("Enter the password for the encrypted file{0}{1}", Environment.NewLine, archiveName);
        }

        private void Password_Shown(object sender, EventArgs e) {
            txtbPassword.Focus();
        }

        private void Password_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                //
            }
        }

        private void chkbShowPassword_CheckedChanged(object sender, EventArgs e) {
            txtbPassword.PasswordChar = (chkbShowPassword.Checked) ? '\0' : '*';
        }

        private void btnOkay_Click(object sender, EventArgs e) {
            string password = txtbPassword.Text;
            if (string.IsNullOrWhiteSpace(password) && Required) {
                DialogResult result = MessageBox.Show("It looks like you have left the password field empty. This archive requires a password in order to continue, would you like to retry?", "ZipMagikLITE: Empty Password", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes) {
                    return;
                }
                else {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            else if (string.IsNullOrWhiteSpace(password)) {
                DialogResult result = MessageBox.Show("It looks like you have left the password field empty. If you continue then it will not set a password for this archive. Are you sure you want to continue?", "ZipMagikLITE: Empty Password", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes) {
                    Value = null;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else {
                    return;
                }
            } 

            Value = txtbPassword.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
