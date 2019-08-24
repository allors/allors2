// <copyright file="Printable.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("61207a42-3199-4249-baa4-9dd11dc0f5b1")]
    #endregion
    public partial interface Printable : Object
    {
        #region Allors
        [Id("079C31BA-0D20-4CD7-921C-A1829E226970")]
        [AssociationId("C98431FE-98EA-44EB-97C4-8D5F2C147424")]
        [RoleId("B3ECE72C-D62C-4F24-805A-34D7FF21DE4F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        PrintDocument PrintDocument { get; set; }

        #region Allors
        [Id("55903F87-8D6B-4D99-9E0D-C3B74064C81F")]
        #endregion
        void Print();
    }
}
