//-------------------------------------------------------------------------------------------------
// <copyright file="ContainedIn.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Meta;
    using Allors.Protocol.Data;

    public class ContainedIn : IPropertyPredicate
    {
        public ContainedIn(IPropertyType propertyType = null) => this.PropertyType = propertyType;

        public IPropertyType PropertyType { get; set; }

        public IExtent Extent { get; set; }

        public IEnumerable<IObject> Objects { get; set; }

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

        public Predicate Save() =>
            new Predicate
            {
                Kind = PredicateKind.ContainedIn,
                PropertyType = this.PropertyType?.Id,
                Extent = this.Extent?.Save(),
                Values = this.Objects.Select(v => v.Id.ToString()).ToArray(),
                Parameter = this.Parameter
            };

        void IPredicate.Build(ISession session, IReadOnlyDictionary<string, object> arguments, Allors.ICompositePredicate compositePredicate)
        {
            var objects = this.Parameter != null ? session.GetObjects(arguments[this.Parameter]) : this.Objects;

            if (this.PropertyType is IRoleType roleType)
            {
                if (objects != null)
                {
                    compositePredicate.AddContainedIn(roleType, objects);
                }
                else
                {
                    compositePredicate.AddContainedIn(roleType, this.Extent.Build(session, arguments));
                }
            }
            else
            {
                var associationType = (IAssociationType)this.PropertyType;
                if (objects != null)
                {
                    compositePredicate.AddContainedIn(associationType, objects);
                }
                else
                {
                    compositePredicate.AddContainedIn(associationType, this.Extent.Build(session, arguments));
                }
            }
        }
    }
}
