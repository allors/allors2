// <copyright file="Position.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("04540476-602f-456a-b300-54166b65c8b1")]
    #endregion
    public partial class Position : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("137841cd-fa69-4704-a6e3-cd710c51af43")]
        [AssociationId("834d2485-8aac-4e7e-86ad-0b7c5c21b368")]
        [RoleId("7f6522ca-1f5b-4c97-99b4-1f4ac6670d8e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Organisation Organisation { get; set; }
        #region Allors
        [Id("2806ca00-0b79-45e5-835e-b11f45b05f15")]
        [AssociationId("144fcdd3-d66c-4ad3-9c68-a6f8c96afdc5")]
        [RoleId("cc54251d-b913-41f7-ba48-982e5829c0f0")]
        #endregion

        public bool Temporary { get; set; }
        #region Allors
        [Id("39298cc2-0869-4dc9-b0ff-bea8269ba958")]
        [AssociationId("7ca00aff-ad0b-4195-902b-39b3d5cc2c25")]
        [RoleId("949968c0-dc95-44d3-a8f0-65829a884c3b")]
        #endregion

        public DateTime EstimatedThroughDate { get; set; }
        #region Allors
        [Id("6ede43f7-87a5-429c-8fc0-6441ca8753f1")]
        [AssociationId("c2100e41-9586-485c-8110-693de5479a9e")]
        [RoleId("5f2fa20d-f4c8-468e-b9bb-d9a3cd777b70")]
        #endregion

        public DateTime EstimatedFromDate { get; set; }
        #region Allors
        [Id("8166d3b6-cc9d-486a-9321-5cd97ff49ddc")]
        [AssociationId("2c5ea5b2-9bea-4181-8c4e-ae903e93c8f8")]
        [RoleId("03b86a68-6063-4299-aa94-ed3f1850f115")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public PositionType PositionType { get; set; }
        #region Allors
        [Id("bf81174e-1105-4313-8d42-4a7b03bfc308")]
        [AssociationId("679e6db2-ffd5-47db-a601-624d9f852057")]
        [RoleId("a345fe4c-caa6-4d40-a168-e97d315bc37d")]
        #endregion

        public bool Fulltime { get; set; }
        #region Allors
        [Id("cb040fe9-8cdb-4e3a-9a32-e6700f1a8867")]
        [AssociationId("0ee703ac-5647-4402-aee8-bfc1eaad2b7c")]
        [RoleId("ec97642c-97dc-423d-b379-e3dce90d0d0d")]
        #endregion

        public bool Salary { get; set; }
        #region Allors
        [Id("db94dd2c-5f39-4f64-ad6d-ce80bf7a4c22")]
        [AssociationId("1e391ccb-da94-4b69-8dc7-b0659eaaf201")]
        [RoleId("c3123ffe-f6d2-4d46-87be-77e184ec8adb")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public PositionStatus PositionStatus { get; set; }
        #region Allors
        [Id("e1f8d2a3-83a7-4357-9451-858c314dbefc")]
        [AssociationId("5d53246b-9497-476e-b68a-e8e5bea2c851")]
        [RoleId("a026d5da-a2dd-4443-956f-2c6d8c73a894")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public BudgetItem ApprovedBudgetItem { get; set; }
        #region Allors
        [Id("ec8beecc-9e28-4103-94d3-249aed76c934")]
        [AssociationId("c68b7794-0379-4542-8f1b-24311e2358a4")]
        [RoleId("3a72f3f0-0476-4629-9831-ed43ebaa8cf5")]
        #endregion
        [Required]

        public DateTime ActualFromDate { get; set; }
        #region Allors
        [Id("fc328a1a-4f62-42de-96b2-a61c612a1602")]
        [AssociationId("f815e446-05a5-4fa3-b3b4-4c7a94b7ca1f")]
        [RoleId("c8cb5709-08d5-4b3b-9598-15289ba9d689")]
        #endregion

        public DateTime ActualThroughDate { get; set; }

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
