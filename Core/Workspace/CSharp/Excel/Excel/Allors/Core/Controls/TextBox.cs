using Allors.Excel;
using Allors.Workspace;

namespace Application.Excel
{
    public class TextBox : TextBox<ISessionObject>
    {
        public TextBox(ICell cell) : base(cell)
        {
        }
    }
}