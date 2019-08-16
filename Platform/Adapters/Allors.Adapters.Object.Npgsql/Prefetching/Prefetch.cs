//------------------------------------------------------------------------------------------------- 
// <copyright file="Prefetch.cs" company="Allors bvba">
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
// <summary>Defines the Session type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Object.Npgsql
{
    using System.Collections.Generic;
    using System.Linq;

    using Allors;
    using Allors.Meta;

    internal class Prefetch
    {
        private readonly Prefetcher prefetcher;
        private readonly PrefetchPolicy prefetchPolicy;
        private readonly HashSet<Reference> references;

        public Prefetch(Prefetcher prefetcher, PrefetchPolicy prefetchPolicy, HashSet<Reference> references)
        {
            this.prefetcher = prefetcher;
            this.references = references;
            this.prefetchPolicy = prefetchPolicy;
        }

        public void Execute()
        {
            var leafs = new HashSet<long>();

            var nestedObjectIdsByRoleType = new Dictionary<IRoleType, HashSet<long>>();

            // Phase 1
            var unitRoles = false;
            foreach (var prefetchRule in this.prefetchPolicy)
            {
                var propertyType = prefetchRule.PropertyType;
                if (propertyType is IRoleType)
                {
                    var roleType = (IRoleType)propertyType;
                    var objectType = roleType.ObjectType;
                    if (objectType.IsUnit)
                    {
                        if (!unitRoles)
                        {
                            unitRoles = true;

                            var referencesByClass = new Dictionary<IClass, List<Reference>>();
                            foreach (var reference in this.references)
                            {
                                if (!referencesByClass.TryGetValue(reference.Class, out var classedReferences))
                                {
                                    classedReferences = new List<Reference>();
                                    referencesByClass.Add(reference.Class, classedReferences);
                                }

                                classedReferences.Add(reference);
                            }

                            foreach (var dictionaryEntry in referencesByClass)
                            {
                                var @class = dictionaryEntry.Key;
                                var classedReferences = dictionaryEntry.Value;

                                var referencesWithoutCachedRole = new HashSet<Reference>();
                                foreach (var reference in classedReferences)
                                {
                                    var roles = this.prefetcher.Session.State.GetOrCreateRoles(reference);
                                    if (!roles.TryGetUnitRole(roleType, out var role))
                                    {
                                        referencesWithoutCachedRole.Add(reference);
                                    }
                                }

                                this.prefetcher.PrefetchUnitRoles(@class, referencesWithoutCachedRole, roleType);
                            }
                        }
                    }
                    else
                    {
                        var nestedPrefetchPolicy = prefetchRule.PrefetchPolicy;
                        var existNestedPrefetchPolicy = nestedPrefetchPolicy != null;
                        var nestedObjectIds = existNestedPrefetchPolicy ? new HashSet<long>() : null;
                        if (existNestedPrefetchPolicy)
                        {
                            nestedObjectIdsByRoleType[roleType] = nestedObjectIds;
                        }

                        var relationType = roleType.RelationType;
                        if (roleType.IsOne)
                        {
                            if (relationType.ExistExclusiveClasses)
                            {
                                this.prefetcher.PrefetchCompositeRoleObjectTable(this.references, roleType, nestedObjectIds, leafs);
                            }
                            else
                            {
                                this.prefetcher.PrefetchCompositeRoleRelationTable(this.references, roleType, nestedObjectIds, leafs);
                            }
                        }
                        else
                        {
                            var associationType = relationType.AssociationType;
                            if (associationType.IsOne && relationType.ExistExclusiveClasses)
                            {
                                this.prefetcher.PrefetchCompositesRoleObjectTable(this.references, roleType, nestedObjectIds, leafs);
                            }
                            else
                            {
                                this.prefetcher.PrefetchCompositesRoleRelationTable(this.references, roleType, nestedObjectIds, leafs);
                            }
                        }
                    }
                }
                else
                {
                    var associationType = (IAssociationType)propertyType;
                    var relationType = associationType.RelationType;
                    var roleType = relationType.RoleType;

                    var nestedPrefetchPolicy = prefetchRule.PrefetchPolicy;
                    var existNestedPrefetchPolicy = nestedPrefetchPolicy != null;
                    var nestedObjectIds = existNestedPrefetchPolicy ? new HashSet<long>() : null;
                    if (existNestedPrefetchPolicy)
                    {
                        nestedObjectIdsByRoleType[roleType] = nestedObjectIds;
                    }

                    if (!(associationType.IsMany && roleType.IsMany) && relationType.ExistExclusiveClasses)
                    {
                        if (associationType.IsOne)
                        {
                            this.prefetcher.PrefetchCompositeAssociationObjectTable(this.references, associationType, nestedObjectIds, leafs);
                        }
                        else
                        {
                            this.prefetcher.PrefetchCompositesAssociationObjectTable(this.references, associationType, nestedObjectIds, leafs);
                        }
                    }
                    else
                    {
                        if (associationType.IsOne)
                        {
                            this.prefetcher.PrefetchCompositeAssociationRelationTable(this.references, associationType, nestedObjectIds, leafs);
                        }
                        else
                        {
                            this.prefetcher.PrefetchCompositesAssociationRelationTable(this.references, associationType, nestedObjectIds, leafs);
                        }
                    }
                }
            }

            var objectIds = new HashSet<long>();
            if (leafs.Count > 0)
            {
                objectIds.UnionWith(leafs);
            }

            foreach (var nestedObjectIds in nestedObjectIdsByRoleType.Values)
            {
                objectIds.UnionWith(nestedObjectIds);
            }

            var referenceByObjectId = this.prefetcher.GetReferencesForPrefetching(objectIds).ToDictionary(v => v.ObjectId);

            foreach (var prefetchRule in this.prefetchPolicy)
            {
                var propertyType = prefetchRule.PropertyType;
                if (propertyType is IRoleType)
                {
                    var roleType = (IRoleType)propertyType;
                    var objectType = roleType.ObjectType;
                    if (!objectType.IsUnit)
                    {
                        var nestedPrefetchPolicy = prefetchRule.PrefetchPolicy;
                        var existNestedPrefetchPolicy = nestedPrefetchPolicy != null;
                        if (existNestedPrefetchPolicy)
                        {
                            var nestedObjectIds = nestedObjectIdsByRoleType[roleType];
                            var nestedReferenceIds = new HashSet<Reference>(nestedObjectIds.Where(v => referenceByObjectId.ContainsKey(v)).Select(v => referenceByObjectId[v]));
                            new Prefetch(this.prefetcher, nestedPrefetchPolicy, nestedReferenceIds).Execute();
                        }
                    }
                }
                else
                {
                    var associationType = (IAssociationType)propertyType;
                    var relationType = associationType.RelationType;
                    var roleType = relationType.RoleType;

                    var nestedPrefetchPolicy = prefetchRule.PrefetchPolicy;
                    var existNestedPrefetchPolicy = nestedPrefetchPolicy != null;
                    if (existNestedPrefetchPolicy)
                    {
                        var nestedObjectIds = nestedObjectIdsByRoleType[roleType];
                        var nestedReferenceIds = new HashSet<Reference>(nestedObjectIds.Where(v => referenceByObjectId.ContainsKey(v)).Select(v => referenceByObjectId[v]));
                        new Prefetch(this.prefetcher, nestedPrefetchPolicy, nestedReferenceIds).Execute();
                    }
                }
            }
        }
    }
}
