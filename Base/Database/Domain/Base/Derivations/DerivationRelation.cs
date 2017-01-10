// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DerivationRelation.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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

namespace Allors.Domain
{
    using System.Text;

    using Allors;
    using Allors.Meta;

    public class DerivationRelation
    {
        public DerivationRelation(IObject association, RoleType roleType)
        {
            this.Association = association;
            this.RoleType = roleType;
        }

        public DerivationRelation(IObject role, AssociationType associationType)
        {
            this.Role = role;
            this.AssociationType = associationType;
        }

        public IObject Association { get; }

        public RoleType RoleType { get; }
        
        public IObject Role { get; }

        public AssociationType AssociationType { get; }

        public static DerivationRelation[] Create(IObject association, params RoleType[] roleTypes)
        {
            var derivationRoles = new DerivationRelation[roleTypes.Length];
            for (var i = 0; i < derivationRoles.Length; i++)
            {
                derivationRoles[i] = new DerivationRelation(association, roleTypes[i]);
            }

            return derivationRoles;
        }

        public static DerivationRelation[] Create(IObject role, params AssociationType[] associationTypes)
        {
            var derivationRoles = new DerivationRelation[associationTypes.Length];
            for (var i = 0; i < derivationRoles.Length; i++)
            {
                derivationRoles[i] = new DerivationRelation(role, associationTypes[i]);
            }

            return derivationRoles;
        }

        public static string ToString(DerivationRelation[] relations)
        {
            var stringBuilder = new StringBuilder();
            var first = true;
            foreach (var relation in relations)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    stringBuilder.Append(", ");
                }

                stringBuilder.Append(relation);
            }

            return stringBuilder.ToString();
        }

        public override string ToString()
        {
            if (this.Association != null)
            {
                if (this.RoleType != null)
                {
                    return this.Association.Strategy.Class.Name + "." + this.RoleType.Name;
                }
                
                return this.Association.Strategy.Class.Name;
            }
            else
            {
                if (this.AssociationType != null)
                {
                    return this.Role.Strategy.Class.Name + "." + this.AssociationType.Name;
                }

                return this.Role.Strategy.Class.Name;
            }
        }
    }
}