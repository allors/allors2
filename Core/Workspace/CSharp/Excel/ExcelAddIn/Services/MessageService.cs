namespace Allors.Excel
{
    using System.Windows.Forms;

    public class MessageService : IMessageService
    {
        public void Show(string text, string caption) => MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
