namespace BaseExcelAddIn.Base
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxUser = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.labelErrorMessage = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonTogglePassword = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 48);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "User Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 102);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password:";
            // 
            // textBoxUser
            // 
            this.textBoxUser.Location = new System.Drawing.Point(187, 45);
            this.textBoxUser.MaxLength = 48;
            this.textBoxUser.Name = "textBoxUser";
            this.textBoxUser.Size = new System.Drawing.Size(271, 26);
            this.textBoxUser.TabIndex = 2;
            this.textBoxUser.TextChanged += new System.EventHandler(this.TextBoxUser_TextChanged);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxPassword.Location = new System.Drawing.Point(0, 0);
            this.textBoxPassword.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxPassword.MaxLength = 32;
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(271, 26);
            this.textBoxPassword.TabIndex = 3;
            this.textBoxPassword.TextChanged += new System.EventHandler(this.TextBoxPassword_TextChanged);
            // 
            // button1
            // 
            this.button1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button1.Location = new System.Drawing.Point(187, 171);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(138, 51);
            this.button1.TabIndex = 4;
            this.button1.Text = "Login";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // labelErrorMessage
            // 
            this.labelErrorMessage.AutoSize = true;
            this.labelErrorMessage.ForeColor = System.Drawing.Color.Firebrick;
            this.labelErrorMessage.Location = new System.Drawing.Point(181, 241);
            this.labelErrorMessage.Name = "labelErrorMessage";
            this.labelErrorMessage.Size = new System.Drawing.Size(84, 20);
            this.labelErrorMessage.TabIndex = 5;
            this.labelErrorMessage.Text = "message";
            this.labelErrorMessage.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonTogglePassword);
            this.panel1.Controls.Add(this.textBoxPassword);
            this.panel1.Location = new System.Drawing.Point(187, 102);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(328, 40);
            this.panel1.TabIndex = 6;
            // 
            // buttonTogglePassword
            // 
            this.buttonTogglePassword.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.buttonTogglePassword.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonTogglePassword.Location = new System.Drawing.Point(274, 0);
            this.buttonTogglePassword.Name = "buttonTogglePassword";
            this.buttonTogglePassword.Size = new System.Drawing.Size(54, 40);
            this.buttonTogglePassword.TabIndex = 7;
            this.buttonTogglePassword.Text = "...";
            this.buttonTogglePassword.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonTogglePassword.UseVisualStyleBackColor = false;
            this.buttonTogglePassword.Click += new System.EventHandler(this.ButtonTogglePassword_Click);
            // 
            // LoginForm
            // 
            this.AcceptButton = this.button1;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(580, 363);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelErrorMessage);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelErrorMessage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonTogglePassword;
    }
}