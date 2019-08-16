//------------------------------------------------------------------------------------------------- 
// <copyright file="State.cs" company="Allors bvba">
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

namespace Allors.Adapters.Object.SqlClient
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
            Reference reference;
            if (!this.ReferenceByObjectId.TryGetValue(objectId, out reference))
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
                Reference reference;
                if (!this.ReferenceByObjectId.TryGetValue(objectId, out reference))
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
                Reference reference;
                if (!this.ReferenceByObjectId.TryGetValue(objectId, out reference))
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
            Reference reference;
            if (!this.ReferenceByObjectId.TryGetValue(objectId, out reference))
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
            Reference reference;
            if (!this.ReferenceByObjectId.TryGetValue(objectId, out reference))
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
                Roles roles;
                if (this.ModifiedRolesByReference.TryGetValue(reference, out roles))
                {
                    return roles;
                }
            }

            return new Roles(reference);
        }

        public Dictionary<Reference, Reference> GetAssociationByRole(IAssociationType associationType)
        {
            Dictionary<Reference, Reference> associationByRole;
            if (!this.AssociationByRoleByAssociationType.TryGetValue(associationType, out associationByRole))
            {
                associationByRole = new Dictionary<Reference, Reference>();
                this.AssociationByRoleByAssociationType[associationType] = associationByRole;
            }

            return associationByRole;
        }

        public Dictionary<Reference, long[]> GetAssociationsByRole(IAssociationType associationType)
        {
            Dictionary<Reference, long[]> associationsByRole;
            if (!this.AssociationsByRoleByAssociationType.TryGetValue(associationType, out associationsByRole))
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
