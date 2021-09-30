// <copyright file="WorkTaskBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>

namespace Allors.Domain.TestPopulation
{
    using System.Collections.Generic;
    using System.Linq;

    public static partial class WorkTaskBuilderExtensions
    {
        public static WorkTaskBuilder WithScheduledWorkForExternalCustomer(this WorkTaskBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var customer = internalOrganisation.ActiveCustomers.Where(v => v.GetType().Name == typeof(Organisation).Name).FirstOrDefault();

            @this.WithTakenBy(internalOrganisation);
            @this.WithExecutedBy(internalOrganisation);
            @this.WithCustomer(customer);
            @this.WithFacility(faker.Random.ListItem(internalOrganisation.FacilitiesWhereOwner));
            @this.WithContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithName(string.Join(" ", faker.Lorem.Words(3)));
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithWorkDone(faker.Lorem.Sentence());
            @this.WithPriority(faker.Random.ListItem(@this.Session.Extent<Priority>()));
            @this.WithWorkEffortPurpose(faker.Random.ListItem(@this.Session.Extent<WorkEffortPurpose>()));
            @this.WithScheduledStart(@this.Session.Now().AddDays(7));
            @this.WithScheduledCompletion(@this.Session.Now().AddDays(10));
            @this.WithEstimatedHours(faker.Random.Int(7, 30));
            @this.WithElectronicDocument(new MediaBuilder(@this.Session).WithInFileName("doc1.en.pdf").WithInData(faker.Random.Bytes(1000)).Build());

            return @this;
        }

        public static WorkTaskBuilder WithScheduledInternalWork(this WorkTaskBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var otherInternalOrganization = @this.Session.Extent<Organisation>().Except(new List<Organisation> { internalOrganisation }).FirstOrDefault();

            @this.WithTakenBy(internalOrganisation);
            @this.WithExecutedBy(internalOrganisation);
            @this.WithCustomer(otherInternalOrganization);
            @this.WithFacility(faker.Random.ListItem(internalOrganisation.FacilitiesWhereOwner));
            @this.WithName(string.Join(" ", faker.Lorem.Words(3)));
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithWorkDone(faker.Lorem.Sentence());
            @this.WithPriority(faker.Random.ListItem(@this.Session.Extent<Priority>()));
            @this.WithWorkEffortPurpose(faker.Random.ListItem(@this.Session.Extent<WorkEffortPurpose>()));
            @this.WithScheduledStart(@this.Session.Now().AddDays(7));
            @this.WithScheduledCompletion(@this.Session.Now().AddDays(10));
            @this.WithEstimatedHours(faker.Random.Int(7, 30));
            @this.WithElectronicDocument(new MediaBuilder(@this.Session).WithInFileName("doc1.en.pdf").WithInData(faker.Random.Bytes(1000)).Build());

            return @this;
        }

        // TODO: WithScheduledWorkForSubcontractedWork
        // TODO: WithWorkStartedForExternalCustomer
        // TODO: WithWorkStartedForInternalWork
        // TODO: WithWorkStartedForSubcontractedWork
    }
}
