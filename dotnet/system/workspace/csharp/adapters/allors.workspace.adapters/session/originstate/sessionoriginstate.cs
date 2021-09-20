// <copyright file="DatabaseObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Meta;
    using Ranges;

    public class SessionOriginState
    {
        private readonly IRanges<Strategy> ranges;
        private readonly PropertyByObjectByPropertyType store;

        public SessionOriginState(IRanges<Strategy> ranges)
        {
            this.ranges = ranges;
            this.store = new PropertyByObjectByPropertyType(ranges);
        }

        public void Checkpoint(ChangeSet changeSet) => changeSet.AddSessionStateChanges(this.store.Checkpoint());

        public object GetUnitRole(Strategy association, IPropertyType propertyType) => this.store.GetUnit(association, propertyType);

        public void SetUnitRole(Strategy association, IRoleType roleType, object role) => this.store.SetUnit(association, roleType, role);

        public Strategy GetCompositeRole(Strategy association, IRoleType propertyType) => this.store.GetComposite(association, propertyType);

        public void SetCompositeRole(Strategy association, IRoleType roleType, Strategy newRole)
        {
            if (newRole == null)
            {
                var role = this.store.GetComposite(association, roleType);
                if (role == null)
                {
                    return;
                }

                if (roleType.AssociationType.IsOne)
                {
                    this.RemoveCompositeRoleOne2One(association, roleType, role);
                }
                else
                {

                    this.RemoveCompositeRoleMany2One(association, roleType, role);
                }
            }
            else if (roleType.AssociationType.IsOne)
            {
                this.SetCompositeRoleOne2One(association, roleType, newRole);
            }
            else
            {
                this.SetCompositeRoleMany2One(association, roleType, newRole);
            }
        }

        public IRange<Strategy> GetCompositesRole(Strategy association, IRoleType propertyType) => this.store.GetComposites(association, propertyType) ?? EmptyRange<Strategy>.Instance;

        public void AddCompositesRole(Strategy association, IRoleType roleType, Strategy item)
        {
            if (roleType.AssociationType.IsOne)
            {
                this.AddCompositesRoleOne2Many(association, roleType, item);
            }
            else
            {
                this.AddCompositesRoleMany2Many(association, roleType, item);
            }
        }

        public void RemoveCompositesRole(Strategy association, IRoleType roleType, Strategy item)
        {
            if (roleType.AssociationType.IsOne)
            {
                this.RemoveCompositesRoleOne2Many(association, roleType, item);
            }
            else
            {
                this.RemoveCompositesRoleMany2Many(association, roleType, item);
            }
        }

        public void SetCompositesRole(Strategy association, IRoleType roleType, IRange<Strategy> role)
        {
            var previousRole = this.store.GetComposites(association, roleType);

            if (previousRole.Equals(role))
            {
                return;
            }

            foreach (var roleToAdd in role.Except(previousRole))
            {
                this.AddCompositesRole(association, roleType, roleToAdd);
            }

            foreach (var roleToRemove in previousRole.Except(role))
            {
                this.RemoveCompositesRole(association, roleType, roleToRemove);
            }
        }

        public Strategy GetCompositeAssociation(Strategy association, IAssociationType propertyType) => this.store.GetComposite(association, propertyType);

        public IEnumerable<Strategy> GetCompositesAssociation(Strategy role, IAssociationType propertyType) => this.store.GetComposites(role, propertyType) ?? (IEnumerable<Strategy>)Array.Empty<Strategy>();

        private void SetCompositeRoleOne2One(Strategy association, IRoleType roleType, Strategy role)
        {
            /*  [if exist]        [then remove]        set
             *
             *  RA ----- R         RA --x-- R       RA    -- R       RA    -- R
             *                ->                +        -        =       -
             *   A ----- PR         A --x-- PR       A --    PR       A --    PR
             */
            var associationType = roleType.AssociationType;
            var previousRole = this.store.GetComposite(association, roleType);

            // R = PR
            if (Equals(role, previousRole))
            {
                return;
            }

            // A --x-- PR
            if (previousRole != null)
            {
                this.RemoveCompositeRoleOne2One(association, roleType, previousRole);
            }

            var roleAssociation = this.store.GetComposite(role, roleType.AssociationType);

            // RA --x-- R
            if (roleAssociation != null)
            {
                this.RemoveCompositeRoleOne2One(roleAssociation, roleType, role);
            }

            // A <---- R
            this.store.SetComposite(role, associationType, association);

            // A ----> R
            this.store.SetComposite(association, roleType, role);
        }

        private void SetCompositeRoleMany2One(Strategy association, IRoleType roleType, Strategy role)
        {
            /*  [if exist]        [then remove]        set
             *
             *  RA ----- R         RA       R       RA    -- R       RA ----- R
             *                ->                +        -        =       -
             *   A ----- PR         A --x-- PR       A --    PR       A --    PR
             */
            var associationType = roleType.AssociationType;
            var previousRole = this.store.GetComposite(association, roleType);

            // R = PR
            if (Equals(role, previousRole))
            {
                return;
            }

            // A --x-- PR
            if (previousRole != null)
            {
                this.RemoveCompositeRoleMany2One(association, roleType, role);
            }

            // A <---- R
            var associations = this.store.GetComposites(role, associationType);
            associations = this.ranges.Add(associations, association);
            this.store.SetComposites(role, associationType, associations);

            // A ----> R
            this.store.SetComposite(association, roleType, role);

            association.Session.ChangeSetTracker.OnSessionChanged(this);
        }

        private void RemoveCompositeRoleOne2One(Strategy association, IRoleType roleType, Strategy role)
        {
            /*                        delete
            *
            *   A ----- R    ->     A       R  =   A       R 
            */

            // A <---- R
            this.store.SetComposite(role, roleType.AssociationType, null);

            // A ----> R
            this.store.SetComposite(association, roleType, null);
        }

        private void RemoveCompositeRoleMany2One(Strategy association, IRoleType roleType, Strategy role)
        {
            /*                        delete
              *  RA --                                RA --
              *       -        ->                 =        -
              *   A ----- R           A --x-- R             -- R
              */
            var associationType = roleType.AssociationType;

            // A <---- R
            var roleAssociations = this.store.GetComposites(role, associationType);
            roleAssociations = this.ranges.Remove(roleAssociations, association);
            this.store.SetComposites(role, associationType, roleAssociations);

            // A ----> R
            this.store.SetComposite(association, roleType, null);
        }

        private void AddCompositesRoleOne2Many(Strategy association, IRoleType roleType, Strategy role)
        {
            /*  [if exist]        [then remove]        set
             *
             *  RA ----- R         RA       R       RA    -- R       RA ----- R
             *                ->                +        -        =       -
             *   A ----- PR         A --x-- PR       A --    PR       A --    PR
             */

            var associationType = roleType.AssociationType;
            var previousRoles = this.store.GetComposites(association, roleType);

            // R in PR 
            if (previousRoles.Contains(role))
            {
                return;
            }

            // A --x-- PR
            var previousAssociation = this.store.GetComposite(role, associationType);
            if (previousAssociation != null)
            {
                this.RemoveCompositesRoleOne2Many(previousAssociation, roleType, role);
            }

            // A <---- R
            this.store.SetComposite(role, associationType, association);

            // A ----> R
            var roles = this.store.GetComposites(association, roleType);
            roles = this.ranges.Add(roles, role);
            this.store.SetComposites(association, roleType, roles);
        }

        private void AddCompositesRoleMany2Many(Strategy association, IRoleType roleType, Strategy role)
        {
            /*  [if exist]        [no remove]         set
             *
             *  RA ----- R         RA       R       RA    -- R       RA ----- R
             *                ->                +        -        =       -
             *   A ----- PR         A       PR       A --    PR       A ----- PR
             */
            var associationType = roleType.AssociationType;
            var previousRoles = this.store.GetComposites(association, roleType);

            // R in PR 
            if (previousRoles.Contains(role))
            {
                return;
            }

            // A <---- R
            var associations = this.store.GetComposites(role, associationType);
            associations = this.ranges.Add(associations, association);
            this.store.SetComposites(role, associationType, associations);

            // A ----> R
            var roles = this.store.GetComposites(association, roleType);
            roles = this.ranges.Add(roles, role);
            this.store.SetComposites(association, roleType, roles);
        }

        private void RemoveCompositesRoleOne2Many(Strategy association, IRoleType roleType, Strategy role)
        {
            var associationType = roleType.AssociationType;
            var roles = this.store.GetComposites(association, roleType);

            // R not in PR 
            if (!roles.Contains(role))
            {
                return;
            }

            // A <---- R
            this.store.SetComposite(role, associationType, null);

            // A ----> R
            roles = this.ranges.Remove(roles, role);
            this.store.SetComposites(association, roleType, roles);
        }

        private void RemoveCompositesRoleMany2Many(Strategy association, IRoleType roleType, Strategy role)
        {
            var associationType = roleType.AssociationType;
            var roles = this.store.GetComposites(association, roleType);

            // R not in PR 
            if (!roles.Contains(role))
            {
                return;
            }

            // A <---- R
            var associations = this.store.GetComposites(role, associationType);
            associations = this.ranges.Remove(associations, association);
            this.store.SetComposites(role, associationType, associations);

            // A ----> R
            roles = this.ranges.Remove(roles, role);
            this.store.SetComposites(association, roleType, roles);
        }
    }
}
