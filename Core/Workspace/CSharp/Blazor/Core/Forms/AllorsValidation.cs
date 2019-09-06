namespace Allors.Blazor
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Components;

    public partial class AllorsValidation
    {
        public ISet<IComponent> Components { get; } = new HashSet<IComponent>();
    }
}
