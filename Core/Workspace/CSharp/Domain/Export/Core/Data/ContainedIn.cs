//-------------------------------------------------------------------------------------------------
// <copyright file="ContainedIn.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

using Allors.Workspace.Meta;

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Workspace;
    using Allors.Protocol.Data;

    public class ContainedIn : IPropertyPredicate
    {
        public ContainedIn(IPropertyType propertyType = null) => this.PropertyType = propertyType;

        public IPropertyType PropertyType { get; set; }

        public IExtent Extent { get; set; }

        public IEnumerable<SessionObject> Objects { get; set; }

        public string Parameter { get; set; }

        bool IPredicate.ShouldTreeShake(IReadOnlyDictionary<string, object> arguments)
        {
            if (this.Parameter != null)
            {
                if (arguments == null || !arguments.ContainsKey(this.Parameter))
                {
                    return false;
                }
            }

            if (this.Extent != null)
            {
                return this.Extent.HasMissingArguments(arguments);
            }

            return false;
        }

        bool IPredicate.HasMissingArguments(IReadOnlyDictionary<string, object> arguments)
        {
            if (this.Parameter != null)
            {
                if (arguments == null || !arguments.ContainsKey(this.Parameter))
                {
                    return true;
                }
            }

            if (this.Extent != null)
            {
                return this.Extent.HasMissingArguments(arguments);
            }

            return false;
        }

        public Predicate ToJson() =>
            new Predicate
            {
                Kind = PredicateKind.ContainedIn,
                PropertyType = this.PropertyType?.Id,
                Extent = this.Extent?.ToJson(),
                Values = this.Objects.Select(v => v.Id.ToString()).ToArray(),
                Parameter = this.Parameter,
            };
    }
}
