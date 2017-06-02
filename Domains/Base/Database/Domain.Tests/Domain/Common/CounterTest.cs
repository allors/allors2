// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CounterTest.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>
//   Defines the ApplicationTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Domain
{
    using System;
    using System.Data;

    using Allors;
    using Allors.Domain;
    using Xunit;

    
    public class CounterTest : DomainTest
    {
        // Teamcity doesn't pick up connection string from app.config,
        // that's why it's here.
        private const string ConnectionString = "server=(local);database=base;Integrated Security=SSPI";

        [Fact]
        public void Meta()
        {
            var counterBuilder = new CounterBuilder(this.Session).Build();

            Assert.True(counterBuilder.ExistUniqueId);
            Assert.NotEqual(Guid.Empty, counterBuilder.UniqueId);

            Assert.Equal(0, counterBuilder.Value);

            var secondCounterBuilder = new CounterBuilder(this.Session).Build();

            Assert.NotEqual(counterBuilder.UniqueId, secondCounterBuilder.UniqueId);
        }

        [Fact]
        public void WithoutSerializable()
        {
            var configuration = new Allors.Adapters.Object.SqlClient.Configuration
            {
                ConnectionString = ConnectionString,
                ObjectFactory = this.ObjectFactory,
            };

            var database = new Allors.Adapters.Object.SqlClient.Database(configuration);

            this.Setup(database, true);

            var id = Guid.NewGuid();

            new CounterBuilder(this.Session).WithUniqueId(id).Build();
            this.Session.Derive(true);
            this.Session.Commit();

            Assert.Equal(1, Counters.NextValue(this.Session, id));
            Assert.Equal(2, Counters.NextValue(this.Session, id));
            Assert.Equal(3, Counters.NextValue(this.Session, id));
            Assert.Equal(4, Counters.NextValue(this.Session, id));
        }

        [Fact]
        public void Serializable()
        {
            var serializableConfiguration = new Allors.Adapters.Object.SqlClient.Configuration
            {
                ConnectionString = ConnectionString,
                ObjectFactory = this.ObjectFactory,
                IsolationLevel = IsolationLevel.Serializable
            };

            var serializableDatabase = new Allors.Adapters.Object.SqlClient.Database(serializableConfiguration);

            var configuration = new Allors.Adapters.Object.SqlClient.Configuration
            {
                ConnectionString = ConnectionString,
                ObjectFactory = this.ObjectFactory,
                Serializable = serializableDatabase
            };

            var database = new Allors.Adapters.Object.SqlClient.Database(configuration);

            this.Setup(database, true);

            var id = Guid.NewGuid();

            new CounterBuilder(this.Session).WithUniqueId(id).Build();
            this.Session.Derive(true);
            this.Session.Commit();

            Assert.Equal(1, Counters.NextValue(this.Session, id));
            Assert.Equal(2, Counters.NextValue(this.Session, id));
            Assert.Equal(3, Counters.NextValue(this.Session, id));
            Assert.Equal(4, Counters.NextValue(this.Session, id));
        }
    }
}