// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangeSet.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Relation.SQLite
{
    using System;
    using System.Collections.Generic;

    public class IdSequence
    {
        private static readonly object StaticLockObject;
        private static readonly Dictionary<string, IdSequence> IdSequenceByPopulationId; 

        private readonly object lockObject = new object();

        private long? current;

        static IdSequence()
        {
            StaticLockObject = new object();
            IdSequenceByPopulationId = new Dictionary<string, IdSequence>();
        }

        private IdSequence()
        {
            this.current = null;
        }

        public static IdSequence GetOrCreate(Database database)
        {
            lock (StaticLockObject)
            {
                IdSequence idSequence;
                if (!IdSequenceByPopulationId.TryGetValue(database.Id, out idSequence))
                {
                    idSequence = new IdSequence();
                    IdSequenceByPopulationId[database.Id] = idSequence;
                }

                return idSequence;
            }
        }

        public long Next(Session session)
        {
            lock (this.lockObject)
            {
                if (!this.current.HasValue)
                {
                    var command = session.CreateCommand("SELECT MAX(" + Mapping.ColumnNameForObject + ") FROM " + Mapping.TableNameForObjects);
                    var result = command.ExecuteScalar();
                    if (result == null || result.Equals(DBNull.Value))
                    {
                        this.current = 0;
                    }
                    else
                    {
                        this.current = (long)result;
                    }
                }
 
                ++this.current;
                return this.current.Value;
            }
        }

        public void Reset()
        {
            this.current = null;
        }
    }
}