namespace Allors.Workspace.Blazor.Validation
{
    using System.Collections.Generic;

    public partial class Fields 
    {
        public IList<IField> Items { get; } = new List<IField>();

        public void Add(IField field) => this.Items.Add(field);

        public void Remove(IField field) => this.Items.Remove(field);
    }
}
