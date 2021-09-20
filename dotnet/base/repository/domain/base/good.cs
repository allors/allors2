// <copyright file="Good.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("7D9E6DE4-E73D-42BE-B658-729309129A53")]
    #endregion
    public partial interface Good : Product
    {
        #region Allors
        [Id("4e8eceff-aec2-44f8-9820-4e417ed904c1")]
        [AssociationId("30f4ec83-5854-4a53-a594-ba1247d02b2f")]
        [RoleId("80361383-e1fc-4256-9b69-7cd43469d0de")]
        #endregion
        [Size(256)]
        [Workspace]
        string BarCode { get; set; }

        #region Allors
        [Id("DDECD426-40C7-4D17-A225-2C46B47F0C89")]
        [AssociationId("B2F5E3CA-E4BF-43B8-B812-D9479A74DF6E")]
        [RoleId("3570A014-1A46-4C61-8D7F-14D07BF1F5AA")]
        #endregion
        [Workspace]
        public decimal ReplacementValue { get; set; }

        #region Allors
        [Id("E25F1487-6F08-4DC5-9838-BAE4FF990ADA")]
        [AssociationId("26F70AEA-7F70-48C4-BDCA-41DC58E3BDB3")]
        [RoleId("F386A2DB-5DBE-4BD6-921B-024A9C80105C")]
        #endregion
        [Workspace]
        public int LifeTime { get; set; }

        #region Allors
        [Id("D96B2474-B8AE-40F4-9D86-4CA09E2B6965")]
        [AssociationId("AA3AA882-D022-42A1-8E1A-4C7901358EE8")]
        [RoleId("19E428F3-9C68-4C59-BD19-68EA17688F04")]
        #endregion
        [Workspace]
        public int DepreciationYears { get; set; }

        #region Allors
        [Id("acbe2dc6-63ad-4910-9752-4cab83e24afb")]
        [AssociationId("70d193cf-8985-4c25-84a5-31f4e2fd2a34")]
        [RoleId("73361510-c5a2-4c4f-afe5-94d2b9eaeea3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        Product[] ProductSubstitutions { get; set; }

        #region Allors
        [Id("e1ee15a9-f173-4d81-a11d-82abff076fb4")]
        [AssociationId("20928aed-02cc-4ea1-9640-cd31cb54ba13")]
        [RoleId("e1c65763-9c2d-4111-bca1-638a69490e99")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        Product[] ProductIncompatibilities { get; set; }
    }
}
