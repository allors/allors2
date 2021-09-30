namespace Application.Excel
{
    using System.Collections.Generic;
    using Allors.Excel;
    using Allors.Workspace;
    using Allors.Workspace.Meta;

    public class CompositeControl : IControl
    {
        /// <summary>
        /// Composite control can Bind multiple cells to a common SessionObject.RoleType
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="cell"></param>
        public CompositeControl(Controls controls, ICell cell)
        {
            this.Cell = cell;
            this.Controls = controls;
            this.Members = new Dictionary<object, IControl>();
        }
        
        public ICell Cell { get; }

        public Controls Controls { get; }

        private IDictionary<object, IControl> Members { get; }

        public void TextBox(ISessionObject sessionObject, RoleType roleType)
        {
            var key = $"{sessionObject?.Id}:{roleType?.Id}";

            if (!this.Members.TryGetValue(key, out var control))
            {
                var textBox = new TextBox(this.Cell);

                textBox.SessionObject = sessionObject;
                textBox.RoleType = roleType;

                this.Members.Add(key, textBox);
            }
        }

        public void Bind()
        {
            foreach (var member in this.Members)
            {
                member.Value.Bind();
            }
        }

        public void Unbind()
        {
            foreach (var member in this.Members)
            {
                member.Value.Unbind();
            }
        }

        public void OnCellChanged()
        {
            foreach (var member in this.Members)
            {
                member.Value.OnCellChanged();
            }
        }
    }
}
