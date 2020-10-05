// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailAddressBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.TestPopulation
{
    public static partial class EmailAddressBuilderExtensions
    {
        public static EmailAddressBuilder WithDefaults(this EmailAddressBuilder @this)
        {
            var faker = @this.Session.Faker();

            @this.WithElectronicAddressString(faker.Internet.Email());
            @this.WithDescription(faker.Lorem.Sentence(5));

            return @this;
        }
    }
}
