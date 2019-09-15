// <copyright file="State.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Session type.</summary>

namespace Allors.Database.Adapters.SqlClient
{
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Meta;

    public sealed class State
    {
        public State()
        {
            this.ReferenceByObjectId = new Dictionary<long, Reference>();

            this.ExistingObjectIdsWithoutReference = new HashSet<long>();
            this.ReferencesWithoutVersions = new HashSet<Reference>();

            this.AssociationByRoleByAssociationType = new Dictionary<IAssociationType, Dictionary<Reference, Reference>>();
            this.AssociationsByRoleByAssociationType = new Dictionary<IAssociationType, Dictionary<Reference, long[]>>();

            this.ChangeSet = new ChangeSet();
        }

        internal ChangeSet ChangeSet { get; set; }

        internal Dictionary<long, Reference> ReferenceByObjectId { get; set; }

        internal Dictionary<Reference, Roles> ModifiedRolesByReference { get; set; }

        internal Dictionary<IAssociationType, Dictionary<Reference, Reference>> AssociationByRoleByAssociationType { get; set; }

        internal Dictionary<IAssociationType, Dictionary<Reference, long[]>> AssociationsByRoleByAssociationType { get; set; }

        internal Dictionary<Reference, Roles> UnflushedRolesByReference { get; set; }

        internal Dictionary<IAssociationType, HashSet<long>> TriggersFlushRolesByAssociationType { get; set; }

        internal HashSet<long> ExistingObjectIdsWithoutReference { get; set; }

        internal HashSet<Reference> ReferencesWithoutVersions { get; set; }

        public Reference GetOrCreateReferenceForExistingObject(long objectId, Session session)
        {
            if (!this.ReferenceByObjectId.TryGetValue(objectId, out var reference))
            {
                var objectType = session.Database.Cache.GetObjectType(objectId);
                if (objectType == null)
                {
                    this.ExistingObjectIdsWithoutReference.Add(objectId);

                    session.InstantiateReferences(this.ExistingObjectIdsWithoutReference);

                    this.ExistingObjectIdsWithoutReference = new HashSet<long>();
                    this.ReferenceByObjectId.TryGetValue(objectId, out reference);
                }
                else
                {
                    reference = new Reference(session, objectType, objectId, false);
                    this.ReferenceByObjectId[objectId] = reference;
                    this.ReferencesWithoutVersions.Add(reference);
                }
            }
            else
            {
                reference.Exists = true;
            }

            return reference;
        }

        public Reference[] GetOrCreateReferencesForExistingObjects(IEnumerable<long> objectIds, Session session)
        {
            var objectIdArray = objectIds.ToArray();

            var instantiate = false;
            foreach (var objectId in objectIdArray)
            {
                if (!this.ReferenceByObjectId.TryGetValue(objectId, out var reference))
                {
                    var objectType = session.Database.Cache.GetObjectType(objectId);
                    if (objectType == null)
                    {
                        instantiate = true;
                        this.ExistingObjectIdsWithoutReference.Add(objectId);
                    }
                }
            }

            if (instantiate)
            {
                session.InstantiateReferences(this.ExistingObjectIdsWithoutReference);
                this.ExistingObjectIdsWithoutReference = new HashSet<long>();
            }

            var references = new List<Reference>();
            foreach (var objectId in objectIdArray)
            {
                if (!this.ReferenceByObjectId.TryGetValue(objectId, out var reference))
                {
                    var objectType = session.Database.Cache.GetObjectType(objectId);
                    if (objectType == null)
                    {
                        this.ExistingObjectIdsWithoutReference.Add(objectId);
                    }
                    else
                    {
                        reference = new Reference(session, objectType, objectId, false);
                        this.ReferenceByObjectId[objectId] = reference;
                        this.ReferencesWithoutVersions.Add(reference);
                    }
                }
                else
                {
                    reference.Exists = true;
                }

                references.Add(reference);
            }

            return references.ToArray();
        }

        public Reference GetOrCreateReferenceForExistingObject(IClass objectType, long objectId, Session session)
        {
            if (!this.ReferenceByObjectId.TryGetValue(objectId, out var reference))
            {
                reference = new Reference(session, objectType, objectId, false);
                this.ReferenceByObjectId[objectId] = reference;
                this.ReferencesWithoutVersions.Add(reference);
            }
            else
            {
                reference.Exists = true;
            }

            return reference;
        }

        public Reference GetOrCreateReferenceForExistingObject(IClass objectType, long objectId, long version, Session session)
        {
            if (!this.ReferenceByObjectId.TryGetValue(objectId, out var reference))
            {
                reference = new Reference(session, objectType, objectId, version);
                this.ReferenceByObjectId[objectId] = reference;
            }
            else
            {
                reference.Version = version;
                reference.Exists = true;
            }

            return reference;
        }

        public Reference CreateReferenceForNewObject(IClass objectType, long objectId, Session session)
        {
            var strategyReference = new Reference(session, objectType, objectId, true);
            this.ReferenceByObjectId[objectId] = strategyReference;
            return strategyReference;
        }

        public Roles GetOrCreateRoles(Reference reference)
        {
            if (this.ModifiedRolesByReference != null)
            {
                if (this.ModifiedRolesByReference.TryGetValue(reference, out var roles))
                {
                    return roles;
                }
            }

            return new Roles(reference);
        }

        public Dictionary<Reference, Reference> GetAssociationByRole(IAssociationType associationType)
        {
            if (!this.AssociationByRoleByAssociationType.TryGetValue(associationType, out var associationByRole))
            {
                associationByRole = new Dictionary<Reference, Reference>();
                this.AssociationByRoleByAssociationType[associationType] = associationByRole;
            }

            return associationByRole;
        }

        public Dictionary<Reference, long[]> GetAssociationsByRole(IAssociationType associationType)
        {
            if (!this.AssociationsByRoleByAssociationType.TryGetValue(associationType, out var associationsByRole))
            {
                associationsByRole = new Dictionary<Reference, long[]>();
                this.AssociationsByRoleByAssociationType[associationType] = associationsByRole;
            }

            return associationsByRole;
        }

        public long GetObjectIdForExistingObject(string objectStringId)
        {
            var objectId = long.Parse(objectStringId);
            if (!this.ReferenceByObjectId.ContainsKey(objectId))
            {
                this.ExistingObjectIdsWithoutReference.Add(objectId);
            }

            return objectId;
        }
    }
}
