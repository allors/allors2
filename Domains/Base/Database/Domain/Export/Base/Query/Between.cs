// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Between.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.Query
{
    using Allors.Meta;

    public class Between : Predicate
    {
        public RoleType RoleType { get; set; }

        public object First { get; set; }

        public object Second { get; set; }

        public override void Build(ISession session, ICompositePredicate compositePredicate)
        {
            compositePredicate.AddBetween(this.RoleType, this.First, this.Second);
        }

        public override void Validate(QueryValidation validation)
        {
            this.AssertExists(validation, "RoleType is required", v => v.RoleType);
            this.AssertExists(validation, "First is required", v => v.First);
            this.AssertExists(validation, "Second is required", v => v.Second);
        }

        public override string ToString()
        {
            return $"{base.ToString()} {this.RoleType}";
        }
    }
}