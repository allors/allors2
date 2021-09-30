// <copyright file="PayHistory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("208a5af6-8dd8-4a48-acb2-2ecb89e8d322")]
    #endregion
    public partial class PayHistory : Period, Object
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        #endregion

        #region Allors
        [Id("6d26369b-eea2-4712-a7d1-56884a3cc715")]
        [AssociationId("6e23ddf7-9766-4f56-bd4f-587bb6f00e00")]
        [RoleId("9d1f6129-281c-413d-ba78-fdb99c84a8b2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public TimeFrequency Frequency { get; set; }

        #region Allors
        [Id("b3f1071f-7e71-4ef1-aa9b-545ad694f44c")]
        [AssociationId("717107b5-fafc-4cca-b85d-364d819a7529")]
        [RoleId("3f7535b3-76dc-47c8-9668-895596bafc16")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public SalaryStep SalaryStep { get; set; }

        #region Allors
        [Id("b7ef1bf8-b16b-400e-903e-d0a7454572a0")]
        [AssociationId("9717c46c-8c64-477a-916a-98594dd21039")]
        [RoleId("fcae3d2d-fe78-4501-8c8e-bda78822c6f2")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal Amount { get; set; }

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
