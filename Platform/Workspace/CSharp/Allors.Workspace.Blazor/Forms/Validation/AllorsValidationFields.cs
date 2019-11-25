namespace Allors.Workspace.Blazor.Validation
{
    using System.Collections.Generic;

    public partial class AllorsValidationFields 
    {
        public IList<IAllorsValidationField> Fields { get; } = new List<IAllorsValidationField>();

        public void Add(IAllorsValidationField field) => this.Fields.Add(field);

        public void Remove(IAllorsValidationField field) => this.Fields.Remove(field);
    }
}
