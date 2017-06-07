// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Query.cs" company="Allors bvba">
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

    public class Query
    {
        public string Name { get; set; }

        public Composite ObjectType { get; set; }

        public Predicate Predicate { get; set; }

        public Tree Fetch { get; set; }

        public Sort[] Sort { get; set; }

        public Page Page { get; set; }

        internal Extent Build(ISession session)
        {
            var extent = session.Extent(this.ObjectType);

            this.Predicate?.Build(extent.Filter);

            if (this.Sort != null)
            {
                foreach (var sort in this.Sort)
                {
                    extent.AddSort(sort.RoleType, sort.Direction);
                }
            }

            return extent;
        }
    }
}