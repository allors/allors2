// <copyright file="InvoiceTermBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain.TestPopulation
{
    public static partial class SalesOrderItemBuilderExtensions
    {
        public static InvoiceTermBuilder WithDefaults(this InvoiceTermBuilder @this)
        {
            var faker = @this.Session.Faker();

            @this.WithTermValue(faker.Lorem.Sentence()).Build();
            @this.WithTermType(faker.Random.ListItem(@this.Session.Extent<InvoiceTermType>())).Build();
            @this.WithDescription(faker.Lorem.Sentence()).Build();

            return @this;
        }
    }
}
