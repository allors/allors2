// <copyright file="RequestTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using System.Linq;
    using Allors.Domain.Derivations.Default;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;
    using Bogus.DataSets;
    using Xunit;

    public class RequestTests : DomainTest
    {
        [Fact]
        public void GivenIssuerWithoutRequestNumberPrefix_WhenDeriving_ThenSortableRequestNumberIsSet()
        {
            this.InternalOrganisation.RemoveRequestNumberPrefix();
            this.Session.Derive();

            var party = new PersonBuilder(this.Session).WithLastName("party").Build();

            var request = new RequestForQuoteBuilder(this.Session)
                .WithOriginator(party)
                .WithFullfillContactMechanism(new WebAddressBuilder(this.Session).WithElectronicAddressString("test").Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(int.Parse(request.RequestNumber), request.SortableRequestNumber);
        }

        [Fact]
        public void GivenIssuerWithRequestNumberPrefix_WhenDeriving_ThenSortableRequestNumberIsSet()
        {
            this.InternalOrganisation.RequestSequence = new RequestSequences(this.Session).EnforcedSequence;
            this.InternalOrganisation.RequestNumberPrefix = "prefix-";
            this.Session.Derive();

            var party = new PersonBuilder(this.Session).WithLastName("party").Build();

            var request = new RequestForQuoteBuilder(this.Session)
                .WithOriginator(party)
                .WithFullfillContactMechanism(new WebAddressBuilder(this.Session).WithElectronicAddressString("test").Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(int.Parse(request.RequestNumber.Split('-')[1]), request.SortableRequestNumber);
        }

        [Fact]
        public void GivenIssuerWithParametrizedRequestNumberPrefix_WhenDeriving_ThenSortableRequestNumberIsSet()
        {
            this.InternalOrganisation.RequestSequence = new RequestSequences(this.Session).EnforcedSequence;
            this.InternalOrganisation.RequestNumberPrefix = "prefix-{year}-";
            this.Session.Derive();

            var party = new PersonBuilder(this.Session).WithLastName("party").Build();

            var request = new RequestForQuoteBuilder(this.Session)
                .WithOriginator(party)
                .WithFullfillContactMechanism(new WebAddressBuilder(this.Session).WithElectronicAddressString("test").Build())
                .WithRequestDate(this.Session.Now().Date)
                .Build();

            this.Session.Derive();

            var number = int.Parse(request.RequestNumber.Split('-').Last()).ToString("000000");
            Assert.Equal(int.Parse(string.Concat(this.Session.Now().Date.Year.ToString(), number)), request.SortableRequestNumber);
        }
    }
}
