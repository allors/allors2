//------------------------------------------------------------------------------------------------- 
// <copyright file="DomainTest.cs" company="Allors bvba">
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
// <summary>Defines the DomainTest type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta.Static
{
    using System;
    using System.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class DomainTest : AbstractTest
    {
        [Test]
        public void Create()
        {
            var metaPopulation = new MetaPopulation();
            var domain = new Domain(metaPopulation, Guid.NewGuid()) { Name = "Domain" };

            var validationReport = this.MetaPopulation.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);

            Assert.AreEqual(1, metaPopulation.Domains.Count());

            Assert.AreEqual(0, domain.DirectSuperdomains.Count());

            var superdomain = new Domain(metaPopulation, Guid.NewGuid()) { Name = "Superdomain" };

            Assert.AreEqual(2, metaPopulation.Domains.Count());

            domain.AddDirectSuperdomain(superdomain);

            Assert.AreEqual(2, metaPopulation.Domains.Count());

            Assert.AreEqual(1, domain.DirectSuperdomains.Count());

            Assert.AreEqual(0, superdomain.DirectSuperdomains.Count());
        }

        [Test]
        public void Validate()
        {
            var metaPopulation = new MetaPopulation();
            var domain = new Domain(metaPopulation, Guid.NewGuid()) { Name = "Domain" };

            var validationReport = metaPopulation.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);

            domain.Name = string.Empty;

            validationReport = metaPopulation.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(domain, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual("Domain.Name", validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Required, validationReport.Errors[0].Kind);

            domain.Name = "_";

            validationReport = metaPopulation.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(domain, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual("Domain.Name", validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Format, validationReport.Errors[0].Kind);

            domain.Name = "a_";

            validationReport = metaPopulation.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(domain, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual("Domain.Name", validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Format, validationReport.Errors[0].Kind);

            domain.Name = "1";

            validationReport = metaPopulation.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(domain, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual("Domain.Name", validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Format, validationReport.Errors[0].Kind);

            domain.Name = "a1";

            validationReport = metaPopulation.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);
            Assert.AreEqual(0, validationReport.Errors.Length);

            domain.Name = "a";

            validationReport = metaPopulation.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);
            Assert.AreEqual(0, validationReport.Errors.Length);
        }

        [Test]
        public void SuperDomains()
        {
            var metaPopulation = new MetaPopulation();
            var domain = new Domain(metaPopulation, Guid.NewGuid()) { Name = "Domain" };

            Assert.AreEqual(0, domain.Superdomains.Count());

            var superdomain = new Domain(metaPopulation, Guid.NewGuid()) { Name = "Superdomain" };
            domain.AddDirectSuperdomain(superdomain);

            Assert.AreEqual(1, domain.Superdomains.Count());
            Assert.Contains(superdomain, domain.Superdomains.ToList());
            Assert.AreEqual(0, superdomain.Superdomains.Count());

            var supersuperdomain = new Domain(metaPopulation, Guid.NewGuid()) { Name = "Supersuperdomain" };
            superdomain.AddDirectSuperdomain(supersuperdomain);

            Assert.AreEqual(2, domain.Superdomains.Count());
            Assert.Contains(superdomain, domain.Superdomains.ToList());
            Assert.Contains(supersuperdomain, domain.Superdomains.ToList());
            Assert.AreEqual(1, superdomain.Superdomains.Count());
            Assert.Contains(supersuperdomain, superdomain.Superdomains.ToList());
            Assert.AreEqual(0, supersuperdomain.Superdomains.Count());
        }


        [Test]
        public void CircularSuperDomains()
        {
            var metaPopulation = new MetaPopulation();
            var domain = new Domain(metaPopulation, Guid.NewGuid()) { Name = "Domain" };

            var superdomain = new Domain(metaPopulation, Guid.NewGuid()) { Name = "Superdomain" };
            domain.AddDirectSuperdomain(superdomain);

            var exceptionThrown = false;
            try
            {
                superdomain.AddDirectSuperdomain(domain);
            }
            catch
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);

            exceptionThrown = false;
            try
            {
                domain.AddDirectSuperdomain(domain);
            }
            catch
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);
        }
    }

    public class DomainTestWithSuperDomains : DomainTest
    {
        protected override void Populate()
        {
            this.Population.PopulateWithSuperDomains();
        }
    }
}