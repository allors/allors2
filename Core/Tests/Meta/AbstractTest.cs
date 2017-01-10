//------------------------------------------------------------------------------------------------- 
// <copyright file="AbstractTest.cs" company="Allors bvba">
// Copyright 2002-2013 Allors bvba.
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
// <summary>Defines the AbstractTest type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta.Static
{
    using System;

    using NUnit.Framework;

    public abstract class AbstractTest : IDisposable
    {
        protected MetaPopulation MetaPopulation 
        {
            get
            {
                return this.Population.MetaPopulation;
            }
        }

        protected Domain Domain
        {
            get
            {
                return this.Population.Domain;
            }
        }
        
        protected Population Population { get; set; }

        [SetUp]
        public void SetUp()
        {
            this.Population = new Population();
            var validation = this.Domain.MetaPopulation.Validate();
            Assert.IsFalse(validation.ContainsErrors);
        }

        [TearDown]
        public void Dispose()
        {
            this.Population = null;
        }

        protected virtual void Populate()
        {
            this.Population.Populate();
        }
    }
}