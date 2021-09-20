// <copyright file="Employment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("6a7e45b2-36b2-4d2e-a29c-0cc13851766f")]
    #endregion
    public partial class Employment : PartyRelationship, Period, Deletable, Object
    {
        #region inherited properties

        public Party[] Parties { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Agreement[] Agreements { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("a243feb0-e5f0-41b4-9b13-a09bb8413fb3")]
        [AssociationId("03bac42d-dcbc-40f3-a130-7b4f3b37f523")]
        [RoleId("1fb50b4b-2a1b-4139-a376-48f1c72c4645")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Person Employee { get; set; }

        #region Allors
        [Id("CED5A46F-0934-41A7-B2B1-8BFFD152C94D")]
        [AssociationId("44EE460A-39DD-47A9-92C7-7F7BE4DFF7B3")]
        [RoleId("01A161A1-CD28-47A2-B6E0-5B7F36A9047A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public InternalOrganisation Employer { get; set; }

        #region Allors
        [Id("8ED07003-CAAC-4A4F-A948-5992CAF8FBBC")]
        [AssociationId("A36CE81A-7A06-4BB5-A89D-F693C8CDF121")]
        [RoleId("B9450239-3C04-4DC1-88F0-2708D5654A2A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PayrollPreference[] PayrollPreferences { get; set; }

        #region Allors
        [Id("c8fd6c79-f909-414e-b9e3-5e911e2e2080")]
        [AssociationId("da451dab-03db-4bc5-8641-93ec74570f4f")]
        [RoleId("0bef74ad-3eb2-494e-846e-6ca3bbfb057b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public EmploymentTerminationReason EmploymentTerminationReason { get; set; }

        #region Allors
        [Id("e79807d4-dcf8-47e2-b510-e8535f1ec436")]
        [AssociationId("6b4896d8-8bf6-4908-acb9-dc2438263fb7")]
        [RoleId("96ff4ce3-5e0b-408e-9641-edf2e06dc508")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public EmploymentTermination EmploymentTermination { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }
        #endregion
    }
}
