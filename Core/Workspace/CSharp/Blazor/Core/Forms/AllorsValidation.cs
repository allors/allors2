namespace Allors.Blazor
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Components.Forms;

    public partial class AllorsValidation
    {
        public IList<IAllorsValidationField> Fields { get; } = new List<IAllorsValidationField>();

        public void Add(IAllorsValidationField field)
        {
            this.Fields.Add(field);
        }

        public void Remove(IAllorsValidationField field)
        {
            this.Fields.Remove(field);
        }

        internal void Validate(ValidationMessageStore messages)
        {
            foreach (var field in this.Fields)
            {
                field.Validate(messages);
            }
        }

        internal void Validate(FieldIdentifier fieldIdentifier, ValidationMessageStore messages)
        {
            foreach (var field in this.Fields.Where(v => v == fieldIdentifier.Model))
            {
                field.Validate(messages);
            }
        }
    }
}
