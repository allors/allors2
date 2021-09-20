// <copyright file="BudgetVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("7FBECB76-27B6-44E3-BD08-FCBB6998B525")]
    #endregion
    public partial interface BudgetVersion : Version
    {
        #region Allors
        [Id("E119A204-C1A7-44FD-BEE0-7ADC93E72C44")]
        [AssociationId("4727E326-523D-4F8D-932F-5AC50B1742B8")]
        [RoleId("38061C21-697E-417B-A0F5-1D54FE7A2B89")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        BudgetState BudgetState { get; set; }

        #region Allors
        [Id("90AA323B-3DB8-483B-AA70-1667F66B92E0")]
        [AssociationId("BF0529C2-5610-49AE-B9FF-518B8178E7F3")]
        [RoleId("08DD3819-0C74-48CC-8758-A60F501AFEFA")]
        #endregion
        [Required]
        [Workspace]
        DateTime FromDate { get; set; }

        #region Allors
        [Id("0161AEB3-9955-4D04-B57B-FEFE4EE65A10")]
        [AssociationId("5E81545A-ECCE-443A-9D7F-F22D0D789BE2")]
        [RoleId("48DA3CA0-B2A9-457F-825B-B4A4E33E45C0")]
        #endregion
        [Workspace]
        DateTime ThroughDate { get; set; }

        #region Allors
        [Id("A29FBF93-FBDF-4E54-9044-F26452BB096B")]
        [AssociationId("B966A108-A0E3-4874-8315-94C7CAFDDA51")]
        [RoleId("B844C96D-6BD0-40D8-A37E-C448FE07450F")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Comment { get; set; }

        #region Allors
        [Id("D77F2590-CDE3-45CB-8B69-75263FABEF84")]
        [AssociationId("C8B81B65-E65A-4927-BB20-42494B49EE9C")]
        [RoleId("63E6E824-F455-474F-A49A-D173C0E6F2D2")]
        #endregion
        [Required]
        [Size(-1)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("7A2BC4B1-AD88-44E7-9962-B349BA6BA9D8")]
        [AssociationId("E421EA94-E0D9-46E6-8CFA-F5F8C3F349AC")]
        [RoleId("9A1FCEE7-5291-4CE3-BAF4-3E848484315B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        BudgetRevision[] BudgetRevisions { get; set; }

        #region Allors
        [Id("02C70BA0-971A-417B-A9E1-E82DDE76173F")]
        [AssociationId("3DDF0072-E997-4676-8692-68E8039369C6")]
        [RoleId("2B723333-5DCA-40BE-9740-4F08330F01C9")]
        #endregion
        [Size(256)]
        [Workspace]
        string BudgetNumber { get; set; }

        #region Allors
        [Id("C3BF33B5-5D22-4B58-81E0-81A3FE88AAB1")]
        [AssociationId("D00A8D2D-5223-4EA6-84B4-CA01B518116C")]
        [RoleId("29B9E3B0-828B-4279-ABBF-E5688F9DE1FD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        BudgetReview[] BudgetReviews { get; set; }

        #region Allors
        [Id("B264FB7F-A71B-47D2-A16D-3FBBC1DE6EC8")]
        [AssociationId("40109592-0C5D-42F3-B72F-18F6557A96AE")]
        [RoleId("E0314BB9-CBA1-4B45-A15B-A29757558EEE")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        BudgetItem[] BudgetItems { get; set; }
    }
}
