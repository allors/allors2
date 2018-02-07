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

        public Query[] Union { get; set; }

        public Query[] Intersect { get; set; }

        public Query[] Except { get; set; }

        public Tree Include { get; set; }

        public Sort[] Sort { get; set; }

        public Page Page { get; set; }

        public QueryValidation Validate()
        {
            var validation = new QueryValidation(this);

            if (this.Name == null)
            {
                validation.AddError("Missing Name");
            }

            if (this.ObjectType == null)
            {
                validation.AddError("Missing ObjectType");
            }

            this.Predicate?.Validate(validation);

            return validation;
        }

        internal Extent Build(ISession session)
        {
            var extent = session.Extent(this.ObjectType);

            this.Predicate?.Build(session, extent.Filter);

            if (this.Union != null)
            {
                return session.Union(this.Union[0].Build(session), this.Union[1].Build(session));
            }

            if (this.Intersect != null)
            {
                return session.Intersect(this.Intersect[0].Build(session), this.Intersect[1].Build(session));
            }

            if (this.Except != null)
            {
                return session.Except(this.Except[0].Build(session), this.Except[1].Build(session));
            }

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