namespace Autocomplete
{
    using System.Collections.Generic;

    public partial class Model
    {
        public MenuItem[] Menu { get; set; }

        public ValidationLog Validate()
        {
            return new ValidationLog();
        }
    }
}
