// <copyright file="IUnitOfMeasure.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("b7215af5-97d6-42b0-9f6f-c1fccb2bc695")]
    #endregion
    public partial interface IUnitOfMeasure : Enumeration
    {
        #region Allors
        [Id("650F5B49-B4DA-4A34-938C-DF5EE2446298")]
        [AssociationId("83EB2BFE-0E39-4616-8545-69A96E85E8EA")]
        [RoleId("EBAFD17D-1876-4DDA-BEC3-E64BCD9E0E60")]
        #endregion
        [Indexed]
        [Workspace]
        string Abbreviation { get; set; }

        #region Allors
        [Id("4725FD6A-61FE-409C-8706-3A99548D9266")]
        [AssociationId("D29B84C0-BE9C-4898-A850-D190EDD07ACE")]
        [RoleId("C2CF0DCB-30AB-4BCC-BDB2-E2DBFB4C8C43")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        LocalisedText[] LocalisedAbbreviations { get; set; }

        #region Allors
        [Id("22d65b11-5d96-4632-9e95-72e30b885942")]
        [AssociationId("873998c2-8c2e-415a-a3c3-6406b21febd8")]
        [RoleId("0543bd39-be9a-49cb-ae23-5df243ee7ea5")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("65c75f72-3bb4-415c-8aa7-b291d96dd157")]
        [AssociationId("9225dd82-fdb4-451f-a1cf-000fa37268f1")]
        [RoleId("d202f3f6-2f04-4b2e-8c66-d630be77d76d")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]

        UnitOfMeasureConversion[] UnitOfMeasureConversions { get; set; }

        #region Allors
        [Id("D35B0EDF-4196-4FE9-8DAA-8B93AEE3B70D")]
        [AssociationId("F3249A01-7C98-4991-B361-078EA6D1DDD8")]
        [RoleId("5530284E-3249-4FD0-979B-C28A2443EF13")]
        #endregion
        [Workspace]
        string Symbol { get; set; }
    }
}
