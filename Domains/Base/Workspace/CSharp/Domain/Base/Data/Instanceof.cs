//------------------------------------------------------------------------------------------------- 
// <copyright file="Instanceof.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Workspace.Data
{
    using System.Collections.Generic;

    using Allors.Meta;
    using Allors.Workspace.Data.Protocol;

    public class Instanceof : IPropertyPredicate
    {
        public string Parameter { get; set; }

        public Instanceof(IComposite objectType = null)
        {
            this.ObjectType = objectType;
        }

        public IComposite ObjectType { get; set; }

        public IPropertyType PropertyType { get; set; }

        public Predicate Save()
        {
            return new Predicate
            {
                Kind = PredicateKind.Instanceof,
                ObjectType = this.ObjectType?.Id,
                PropertyType = this.PropertyType?.Id
            };
        }

        bool IPredicate.ShouldTreeShake(IReadOnlyDictionary<string, object> arguments)
        {
            return ((IPredicate)this).HasMissingArguments(arguments);
        }

        bool IPredicate.HasMissingArguments(IReadOnlyDictionary<string, object> arguments)
        {
            return this.Parameter != null && (arguments == null || !arguments.ContainsKey(this.Parameter));
        }
    }
}