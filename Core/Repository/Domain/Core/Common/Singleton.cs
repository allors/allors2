// <copyright file="Singleton.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
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
        [Id("615AC72B-B3DF-4057-9B7C-C8528341F5FE")]
        [AssociationId("5848B9B7-5DAC-41C0-9655-79EA24A814F6")]
        [RoleId("56DDB161-B92F-4D73-8CDE-61D4BB275DA1")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Derived]
        [Workspace]
        public Locale[] Locales { get; set; }

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
