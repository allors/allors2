// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrandBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.TestPopulation
{
    public static partial class BrandBuilderExtensions
    {
        public static BrandBuilder WithDefaults(this BrandBuilder @this)
        {
            var faker = @this.Session.Faker();

            @this.WithName(string.Join(" ", faker.Lorem.Words(3)));
            @this.WithLogoImage(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(width: 200, height: 56)).Build());
            @this.WithModel(new ModelBuilder(@this.Session).WithName(faker.Lorem.Word()).Build());
            @this.WithModel(new ModelBuilder(@this.Session).WithName(faker.Lorem.Word()).Build());
            @this.WithModel(new ModelBuilder(@this.Session).WithName(faker.Lorem.Word()).Build());

            return @this;
        }
    }
}
