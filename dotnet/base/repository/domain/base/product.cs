// <copyright file="Product.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("56b79619-d04a-4924-96e8-e3e7be9faa09")]
    #endregion
    public partial interface Product : UnifiedProduct
    {
        #region Allors
        [Id("05a2e95a-e5f1-45bc-a8ca-4ebfad3290b5")]
        [AssociationId("1674a9e0-00de-45fa-bde4-63a716a31557")]
        [RoleId("594503f3-c081-46b3-9695-92b921c15a6b")]
        #endregion
        [Workspace]
        DateTime SupportDiscontinuationDate { get; set; }

        #region Allors
        [Id("0b283eb9-2972-47ae-80d8-1a7aa8f77673")]
        [AssociationId("aa3ccdc9-7286-4a82-912a-dd2e53c7410b")]
        [RoleId("487e408f-d55b-4273-bbe9-b0291069ae42")]
        #endregion
        [Workspace]
        DateTime SalesDiscontinuationDate { get; set; }

        #region Allors
        [Id("28f34f5d-c98c-45f8-9534-ce9191587ac8")]
        [AssociationId("7c676669-52b3-4665-8212-e2e14dde5cf9")]
        [RoleId("5931ff6f-0972-4e9b-9dc3-dd072ed935a3")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]
        PriceComponent[] VirtualProductPriceComponents { get; set; }

        #region Allors
        [Id("345aaf52-424a-4573-b77b-64708665822f")]
        [AssociationId("be3a7b3a-bf77-407e-895a-3609bbf05e24")]
        [RoleId("be85293b-25b0-4856-b9cf-19fe7f0e6a3d")]
        #endregion
        [Size(256)]
        string IntrastatCode { get; set; }

        #region Allors
        [Id("4632101d-09d6-4a89-8bba-e02ac791f9ad")]
        [AssociationId("3aed43b7-3bad-44f9-a2d9-8f865de71156")]
        [RoleId("de3785d8-0143-4339-bf49-310c13de385a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Product ProductComplement { get; set; }

        #region Allors
        [Id("60bd113a-d6b9-4de9-bbff-2b5094ec4803")]
        [AssociationId("b5198a54-72bc-4972-aded-b8eaf0f304a0")]
        [RoleId("1c2134b2-d7ce-469a-a6e4-7e2cc741e07c")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        Product[] Variants { get; set; }

        #region Allors
        [Id("74fc9be0-8677-463c-b3b6-f0e7bb7478ba")]
        [AssociationId("23a3e0bb-a2f9-48d5-b57b-40376e68b0ba")]
        [RoleId("c977306e-8738-4e30-88c1-3c545fdb4e93")]
        #endregion
        [Workspace]
        DateTime IntroductionDate { get; set; }

        #region Allors
        [Id("c018edeb-54e0-43d5-9bbd-bf68df1364de")]
        [AssociationId("2ad88d44-a7f6-41f7-bcf7-fee094f20e22")]
        [RoleId("cd7f09d5-8c4b-46b7-98d1-108f5e910cc3")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        [Indexed]
        EstimatedProductCost[] EstimatedProductCosts { get; set; }

        #region Allors
        [Id("e6f084e9-e6fe-49b8-940e-cda85e1dc1e0")]
        [AssociationId("7eb974af-86a6-4d26-a07f-7dd01b80d3ac")]
        [RoleId("3918335f-7cde-4fd2-b168-fb422ab5ee1a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        Product[] ProductObsolescences { get; set; }

        #region Allors
        [Id("29d3d43b-6332-4a13-830b-44ab828c357b")]
        [AssociationId("c9d0fd8f-f178-4444-b6c2-f3a8a34a20b4")]
        [RoleId("f685b66a-b6b3-477e-99f2-58da0db5da89")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        VatRegime VatRegime { get; set; }

        #region Allors
        [Id("f2abc02c-67a1-42b7-83f5-195841e58a6a")]
        [AssociationId("dae3b48d-0dde-4c71-bbd3-4f7743d20a9f")]
        [RoleId("fe8dd3c4-0540-49d9-a18a-905fe0259ca1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        PriceComponent[] BasePrices { get; set; }
    }
}
