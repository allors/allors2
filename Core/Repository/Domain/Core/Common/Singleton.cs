// <copyright file="Singleton.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("313b97a5-328c-4600-9dd2-b5bc146fb13b")]
    #endregion
    public partial class Singleton : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("9c1634ab-be99-4504-8690-ed4b39fec5bc")]
        [AssociationId("45a4205d-7c02-40d4-8d97-6d7d59e05def")]
        [RoleId("1e051b37-cf30-43ed-a623-dd2928d6d0a3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        public Locale DefaultLocale { get; set; }

        #region Allors
        [Id("9e5a3413-ed33-474f-adf2-149ad5a80719")]
        [AssociationId("33d5d8b9-3472-48d8-ab1a-83d00d9cb691")]
        [RoleId("e75a8956-4d02-49ba-b0cf-747b7a9f350d")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public Locale[] AdditionalLocales { get; set; }

        #region Allors
        [Id("f16652b0-b712-43d7-8d4e-34a22487514d")]
        [AssociationId("c92466b5-55ba-496a-8880-2821f32f8f8e")]
        [RoleId("3a12d798-40c3-40e0-ba9f-9d01b1e39e89")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        [Indexed]
        public AutomatedAgent Guest { get; set; }

        #region Allors
        [Id("F5DB3956-2715-487B-B872-CEF97F70566B")]
        [AssociationId("2702B7E9-EDB0-4DF5-968F-1F68AEF6DCA0")]
        [RoleId("D60B0381-24B3-4EF0-97D7-CF0BFB952830")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        [Indexed]
        public AutomatedAgent Scheduler { get; set; }

        #region Allors
        [Id("6A6E0852-C984-47B8-939D-8E0B0B042B9D")]
        [AssociationId("E783AFBE-EF70-4AC1-8C0A-5DFE6FEDFBE0")]
        [RoleId("BCF431F6-10CD-4F33-873D-0B2F1A1EA09D")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public SecurityToken InitialSecurityToken { get; set; }

        #region Allors
        [Id("f579494b-e550-4be6-9d93-84618ac78704")]
        [AssociationId("33f17e75-99cc-417e-99f3-c29080f08f0a")]
        [RoleId("ca9e3469-583c-4950-ba2c-1bc3a0fc3e96")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public SecurityToken DefaultSecurityToken { get; set; }

        #region Allors
        [Id("B2166062-84DA-449D-B34F-983A0C81BC31")]
        [AssociationId("22096B27-ED3C-4640-BB60-EB7338A779FB")]
        [RoleId("1E931D15-5137-4C6D-91ED-9CC5C3C95BEF")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Media LogoImage { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}
