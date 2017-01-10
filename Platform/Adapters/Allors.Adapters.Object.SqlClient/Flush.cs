// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Flush.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Object.SqlClient
{
    using System.Collections.Generic;

    using Allors;
    using Allors.Meta;

    internal class Flush
    {
        private const int BatchSize = 1000;
        private readonly Session session;

        private Dictionary<IClass, Dictionary<IRoleType, List<UnitRelation>>> setUnitRoleRelationsByRoleTypeByExclusiveClass;
        private Dictionary<IRoleType, List<CompositeRelation>> setCompositeRoleRelationsByRoleType;
        private Dictionary<IRoleType, List<CompositeRelation>> addCompositeRoleRelationsByRoleType;
        private Dictionary<IRoleType, List<CompositeRelation>> removeCompositeRoleRelationsByRoleType;
        private Dictionary<IRoleType, IList<long>> clearCompositeAndCompositesRoleRelationsByRoleType;

        internal Flush(Session session, Dictionary<Reference, Roles> unsyncedRolesByReference)
        {
            this.session = session;

            foreach (var dictionaryEntry in unsyncedRolesByReference)
            {
                var roles = dictionaryEntry.Value;
                roles.Flush(this);
            }
        }

        internal void Execute()
        {
            if (this.setUnitRoleRelationsByRoleTypeByExclusiveClass != null)
            {
                foreach (var firstDictionaryEntry in this.setUnitRoleRelationsByRoleTypeByExclusiveClass)
                {
                    var exclusiveRootClass = firstDictionaryEntry.Key;
                    var setUnitRoleRelationsByRoleType = firstDictionaryEntry.Value;
                    foreach (var secondDictionaryEntry in setUnitRoleRelationsByRoleType)
                    {
                        var roleType = secondDictionaryEntry.Key;
                        var relations = secondDictionaryEntry.Value;
                        if (relations.Count > 0)
                        {
                            this.session.Commands.SetUnitRole(relations, exclusiveRootClass, roleType);
                        }
                    }
                }
            }

            this.setUnitRoleRelationsByRoleTypeByExclusiveClass = null;

            if (this.setCompositeRoleRelationsByRoleType != null)
            {
                foreach (var dictionaryEntry in this.setCompositeRoleRelationsByRoleType)
                {
                    var roleType = dictionaryEntry.Key;
                    var relations = dictionaryEntry.Value;
                    if (relations.Count > 0)
                    {
                        this.session.Commands.SetCompositeRole(relations, roleType);
                    }
                }
            }

            this.setCompositeRoleRelationsByRoleType = null;

            if (this.addCompositeRoleRelationsByRoleType != null)
            {
                foreach (var dictionaryEntry in this.addCompositeRoleRelationsByRoleType)
                {
                    var roleType = dictionaryEntry.Key;
                    var relations = dictionaryEntry.Value;
                    if (relations.Count > 0)
                    {
                        this.session.Commands.AddCompositeRole(relations, roleType);
                    }
                }
            }

            this.addCompositeRoleRelationsByRoleType = null;

            if (this.removeCompositeRoleRelationsByRoleType != null)
            {
                foreach (var dictionaryEntry in this.removeCompositeRoleRelationsByRoleType)
                {
                    var roleType = dictionaryEntry.Key;
                    var relations = dictionaryEntry.Value;
                    if (relations.Count > 0)
                    {
                        this.session.Commands.RemoveCompositeRole(relations, roleType);
                    }
                }
            }

            this.removeCompositeRoleRelationsByRoleType = null;

            if (this.clearCompositeAndCompositesRoleRelationsByRoleType != null)
            {
                foreach (var dictionaryEntry in this.clearCompositeAndCompositesRoleRelationsByRoleType)
                {
                    var roleType = dictionaryEntry.Key;
                    var relations = dictionaryEntry.Value;
                    if (relations.Count > 0)
                    {
                        this.session.Commands.ClearCompositeAndCompositesRole(relations, roleType);
                    }
                }
            }

            this.clearCompositeAndCompositesRoleRelationsByRoleType = null;
        }

        internal void SetUnitRole(Reference association, IRoleType roleType, object role)
        {
            if (this.setUnitRoleRelationsByRoleTypeByExclusiveClass == null)
            {
                this.setUnitRoleRelationsByRoleTypeByExclusiveClass = new Dictionary<IClass, Dictionary<IRoleType, List<UnitRelation>>>();
            }

            var exclusiveClass = association.Class.ExclusiveClass;

            Dictionary<IRoleType, List<UnitRelation>> setUnitRoleRelationsByRoleType;
            if (!this.setUnitRoleRelationsByRoleTypeByExclusiveClass.TryGetValue(exclusiveClass, out setUnitRoleRelationsByRoleType))
            {
                setUnitRoleRelationsByRoleType = new Dictionary<IRoleType, List<UnitRelation>>();
                this.setUnitRoleRelationsByRoleTypeByExclusiveClass[exclusiveClass] = setUnitRoleRelationsByRoleType;
            }

            List<UnitRelation> relations;
            if (!setUnitRoleRelationsByRoleType.TryGetValue(roleType, out relations))
            {
                relations = new List<UnitRelation>();
                setUnitRoleRelationsByRoleType[roleType] = relations;
            }

            var unitRelation = new UnitRelation(association.ObjectId, role);
            relations.Add(unitRelation);

            if (relations.Count > BatchSize)
            {
                this.session.Commands.SetUnitRole(relations, exclusiveClass, roleType);
                relations.Clear();
            }
        }

        internal void SetCompositeRole(Reference association, IRoleType roleType, long role)
        {
            if (this.setCompositeRoleRelationsByRoleType == null)
            {
                this.setCompositeRoleRelationsByRoleType = new Dictionary<IRoleType, List<CompositeRelation>>();
            }

            List<CompositeRelation> relations;
            if (!this.setCompositeRoleRelationsByRoleType.TryGetValue(roleType, out relations))
            {
                relations = new List<CompositeRelation>();
                this.setCompositeRoleRelationsByRoleType[roleType] = relations;
            }

            relations.Add(new CompositeRelation(association.ObjectId, role));

            if (relations.Count > BatchSize)
            {
                this.session.Commands.SetCompositeRole(relations, roleType);
                relations.Clear();
            }
        }

        internal void AddCompositeRole(Reference association, IRoleType roleType, HashSet<long> added)
        {
            if (this.addCompositeRoleRelationsByRoleType == null)
            {
                this.addCompositeRoleRelationsByRoleType = new Dictionary<IRoleType, List<CompositeRelation>>();
            }

            List<CompositeRelation> relations;
            if (!this.addCompositeRoleRelationsByRoleType.TryGetValue(roleType, out relations))
            {
                relations = new List<CompositeRelation>();
                this.addCompositeRoleRelationsByRoleType[roleType] = relations;
            }

            foreach (var roleObjectId in added)
            {
                relations.Add(new CompositeRelation(association.ObjectId, roleObjectId)); 
            }

            if (relations.Count > BatchSize)
            {
                this.session.Commands.AddCompositeRole(relations, roleType);
                relations.Clear();
            }
        }

        internal void RemoveCompositeRole(Reference association, IRoleType roleType, HashSet<long> removed)
        {
            if (this.removeCompositeRoleRelationsByRoleType == null)
            {
                this.removeCompositeRoleRelationsByRoleType = new Dictionary<IRoleType, List<CompositeRelation>>();
            }

            List<CompositeRelation> relations;
            if (!this.removeCompositeRoleRelationsByRoleType.TryGetValue(roleType, out relations))
            {
                relations = new List<CompositeRelation>();
                this.removeCompositeRoleRelationsByRoleType[roleType] = relations;
            }

            foreach (var roleObjectId in removed)
            {
                relations.Add(new CompositeRelation(association.ObjectId, roleObjectId));
            }

            if (relations.Count > BatchSize)
            {
                this.session.Commands.RemoveCompositeRole(relations, roleType);
                relations.Clear();
            }
        }

        internal void ClearCompositeAndCompositesRole(Reference association, IRoleType roleType)
        {
            if (this.clearCompositeAndCompositesRoleRelationsByRoleType == null)
            {
                this.clearCompositeAndCompositesRoleRelationsByRoleType = new Dictionary<IRoleType, IList<long>>();
            }

            IList<long> relations;
            if (!this.clearCompositeAndCompositesRoleRelationsByRoleType.TryGetValue(roleType, out relations))
            {
                relations = new List<long>();
                this.clearCompositeAndCompositesRoleRelationsByRoleType[roleType] = relations;
            }

            relations.Add(association.ObjectId);

            if (relations.Count > BatchSize)
            {
                this.session.Commands.ClearCompositeAndCompositesRole(relations, roleType);
                relations.Clear();
            }
        }
    }
}