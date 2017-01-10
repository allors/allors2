namespace WebApplication.Controls
{
    using System;
    using Allors.Web.Desktop;

    public partial class DefaultWindow : Window
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Button.Click += this.Button_Click;
        }


        protected void Button_Click(object sender, EventArgs e)
        {
            this.Label.Text = this.TextBox.Text;
        }
    }
}