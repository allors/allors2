// <copyright file="Flush.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using System.Collections.Generic;

    using Meta;

    internal class Flush
    {
        private const int BatchSize = 1000;
        private readonly Transaction transaction;

        private Dictionary<IClass, Dictionary<IRoleType, List<UnitRelation>>> setUnitRoleRelationsByRoleTypeByExclusiveClass;
        private Dictionary<IRoleType, List<CompositeRelation>> setCompositeRoleRelationsByRoleType;
        private Dictionary<IRoleType, List<CompositeRelation>> addCompositeRoleRelationsByRoleType;
        private Dictionary<IRoleType, List<CompositeRelation>> removeCompositeRoleRelationsByRoleType;
        private Dictionary<IRoleType, IList<long>> clearCompositeAndCompositesRoleRelationsByRoleType;

        internal Flush(Transaction transaction, Dictionary<Reference, Strategy> unsyncedRolesByReference)
        {
            this.transaction = transaction;

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
                    foreach (var secondDictionaryEntry in firstDictionaryEntry.Value)
                    {
                        var roleType = secondDictionaryEntry.Key;
                        var relations = secondDictionaryEntry.Value;
                        if (relations.Count > 0)
                        {
                            this.transaction.Commands.SetUnitRole(relations, exclusiveRootClass, roleType);
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
                        this.transaction.Commands.SetCompositeRole(relations, roleType);
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
                        this.transaction.Commands.AddCompositeRole(relations, roleType);
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
                        this.transaction.Commands.RemoveCompositeRole(relations, roleType);
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
                        this.transaction.Commands.ClearCompositeAndCompositesRole(relations, roleType);
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

            var exclusiveClass = association.Class.ExclusiveDatabaseClass;

            if (!this.setUnitRoleRelationsByRoleTypeByExclusiveClass.TryGetValue(exclusiveClass, out var setUnitRoleRelationsByRoleType))
            {
                setUnitRoleRelationsByRoleType = new Dictionary<IRoleType, List<UnitRelation>>();
                this.setUnitRoleRelationsByRoleTypeByExclusiveClass[exclusiveClass] = setUnitRoleRelationsByRoleType;
            }

            if (!setUnitRoleRelationsByRoleType.TryGetValue(roleType, out var relations))
            {
                relations = new List<UnitRelation>();
                setUnitRoleRelationsByRoleType[roleType] = relations;
            }

            var unitRelation = new UnitRelation(association.ObjectId, role);
            relations.Add(unitRelation);

            if (relations.Count > BatchSize)
            {
                this.transaction.Commands.SetUnitRole(relations, exclusiveClass, roleType);
                relations.Clear();
            }
        }

        internal void SetCompositeRole(Reference association, IRoleType roleType, long role)
        {
            if (this.setCompositeRoleRelationsByRoleType == null)
            {
                this.setCompositeRoleRelationsByRoleType = new Dictionary<IRoleType, List<CompositeRelation>>();
            }

            if (!this.setCompositeRoleRelationsByRoleType.TryGetValue(roleType, out var relations))
            {
                relations = new List<CompositeRelation>();
                this.setCompositeRoleRelationsByRoleType[roleType] = relations;
            }

            relations.Add(new CompositeRelation(association.ObjectId, role));

            if (relations.Count > BatchSize)
            {
                this.transaction.Commands.SetCompositeRole(relations, roleType);
                relations.Clear();
            }
        }

        internal void AddCompositeRole(Reference association, IRoleType roleType, HashSet<long> added)
        {
            if (this.addCompositeRoleRelationsByRoleType == null)
            {
                this.addCompositeRoleRelationsByRoleType = new Dictionary<IRoleType, List<CompositeRelation>>();
            }

            if (!this.addCompositeRoleRelationsByRoleType.TryGetValue(roleType, out var relations))
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
                this.transaction.Commands.AddCompositeRole(relations, roleType);
                relations.Clear();
            }
        }

        internal void RemoveCompositeRole(Reference association, IRoleType roleType, HashSet<long> removed)
        {
            if (this.removeCompositeRoleRelationsByRoleType == null)
            {
                this.removeCompositeRoleRelationsByRoleType = new Dictionary<IRoleType, List<CompositeRelation>>();
            }

            if (!this.removeCompositeRoleRelationsByRoleType.TryGetValue(roleType, out var relations))
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
                this.transaction.Commands.RemoveCompositeRole(relations, roleType);
                relations.Clear();
            }
        }

        internal void ClearCompositeAndCompositesRole(Reference association, IRoleType roleType)
        {
            if (this.clearCompositeAndCompositesRoleRelationsByRoleType == null)
            {
                this.clearCompositeAndCompositesRoleRelationsByRoleType = new Dictionary<IRoleType, IList<long>>();
            }

            if (!this.clearCompositeAndCompositesRoleRelationsByRoleType.TryGetValue(roleType, out var relations))
            {
                relations = new List<long>();
                this.clearCompositeAndCompositesRoleRelationsByRoleType[roleType] = relations;
            }

            relations.Add(association.ObjectId);

            if (relations.Count > BatchSize)
            {
                this.transaction.Commands.ClearCompositeAndCompositesRole(relations, roleType);
                relations.Clear();
            }
        }
    }
}
