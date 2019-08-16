using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Allors.Workspace.Client;
using Nito.AsyncEx;

namespace BaseExcelAddIn.Base
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

            this.button1.Enabled = false;
        }

        public Uri Uri { get; set; }

        public Database Database { get; set; }

        public bool IsLoggedIn { get; set; }
        private async void Button1_Click(object sender, EventArgs e)
        {
            this.HideError();

            if (string.IsNullOrEmpty(this.textBoxUser.Text))
            {
                this.ShowError("Enter Username.");
            }

            if (string.IsNullOrEmpty(this.textBoxPassword.Text))
            {
                this.ShowError("Enter Password.");
            }

            AsyncContext.Run(
                async () =>
                {
                    IsLoggedIn = await this.Database.Login(this.Uri, this.textBoxUser.Text,this.textBoxPassword.Text);
                });

            if (IsLoggedIn)
            {
                this.UserName = this.textBoxUser.Text;
                // Close the dialog
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.ShowError("Login failed.");
            }
        }

        public string UserName { get; set; }

        private void ShowError(string message)
        {
            this.labelErrorMessage.Text = message;
            this.labelErrorMessage.Visible = true;
        }

        private void HideError()
        {
            this.UserName = null;
            this.labelErrorMessage.Text = "";
            this.labelErrorMessage.Visible = false;
        }

        private void TextBoxUser_TextChanged(object sender, EventArgs e) => this.button1.Enabled = this.textBoxUser.Text.Length > 0 && this.textBoxPassword.Text.Length > 0;

        private void TextBoxPassword_TextChanged(object sender, EventArgs e) => this.button1.Enabled = this.textBoxUser.Text.Length > 0 && this.textBoxPassword.Text.Length > 0;

        private void ButtonTogglePassword_Click(object sender, EventArgs e)
        {
            if (this.textBoxPassword.PasswordChar != '\0')
            {
                this.textBoxPassword.PasswordChar = '\0';
            }
            else
            {
                this.textBoxPassword.PasswordChar = '*';
            }
        }
    }
}
