// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Database.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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

namespace Allors.Adapters.Database.Npgsql.LongId
{
    using Allors.Adapters.Database.Sql;

    public class Database : Npgsql.Database 
    {
        private readonly IObjectIds allorsObjectIds;
       
        private Schema schema;

        public Database(Npgsql.Configuration configuration)
            : base(configuration)
        {
            this.allorsObjectIds = new ObjectLongIds();
        }

        public override IObjectIds AllorsObjectIds
        {
            get { return this.allorsObjectIds; }
        }
        
        public override Npgsql.Schema NpgsqlSchema
        {
            get
            {
                if (this.ObjectFactory.MetaPopulation != null)
                {
                    if (this.schema == null)
                    {
                        this.schema = new Schema(this);
                    }
                }

                return this.schema;
            }
        }

        protected override string IdentityType
        {
            get
            {
                return "bigserial";
            }
        }

        public override void ResetSchema()
        {
            this.schema = null;
        }
    }
}