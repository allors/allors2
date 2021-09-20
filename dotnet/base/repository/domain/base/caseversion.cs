// <copyright file="CaseVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("B15B38A6-0A6C-4AB3-81CA-AF44647F90C1")]
    #endregion
    public partial class CaseVersion : Version
    {
        #region inherited properties

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public User LastModifiedBy { get; set; }

        #endregion

        #region Allors
        [Id("00E6F8D6-4D49-4DE3-8B96-30E170250831")]
        [AssociationId("C3405AC3-B82D-4217-A436-03005F80A7FB")]
        [RoleId("1BA0F445-8295-4267-9463-84D9824B5D53")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Required]
        [Workspace]
        public CaseState CaseState { get; set; }

        #region Allors
        [Id("51760570-2925-4CC2-94A4-8D16A237A932")]
        [AssociationId("68C99559-BF4A-4FE6-92E5-518E07B509C9")]
        [RoleId("D40A08D9-0AA1-47A9-A489-0354FEF334BC")]
        #endregion
        public DateTime StartDate { get; set; }

        #region Allors
        [Id("6BF14869-E4B3-443C-8A04-761FEE122E44")]
        [AssociationId("44256AA2-99B7-4B8B-9FD0-701D1B26732E")]
        [RoleId("59B58EB6-F659-4A55-990E-02FAC180AD20")]
        #endregion
        [Required]
        [Size(-1)]
        public string Description { get; set; }

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
