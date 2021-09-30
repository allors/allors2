namespace Application.Excel
{
    using Allors.Excel;
    using Allors.Workspace;

    public class TextBox : TextBox<ISessionObject>
    {
        public TextBox(ICell cell) : base(cell)
        {
        }
    }
}