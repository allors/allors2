// <copyright file="PurchaseInvoiceStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Domain
{
    using System;

    public partial class PurchaseInvoiceStates
    {
        public static readonly Guid CreatedId = new Guid("102F4080-1D12-4090-9196-F42C094C38CA");
        public static readonly Guid AwaitingApprovalId = new Guid("FE3A30A9-0174-4534-A11E-E772112E9760");
        public static readonly Guid InProcessId = new Guid("C6501188-7145-4abd-85FC-BEF746C74E9E");
        public static readonly Guid ReceivedId = new Guid("FC9EC85B-2419-4c97-92F6-45F5C6D3DF61");
        public static readonly Guid PartiallyPaidId = new Guid("9D917078-7ACD-4F04-AE6B-24E33755E9B1");
        public static readonly Guid NotPaidId = new Guid("3D811CFE-3EC0-4975-80B8-012A42B2B3E2");
        public static readonly Guid PaidId = new Guid("2982C8BE-657E-4594-BCAF-98997AFEA9F8");
        public static readonly Guid CancelledId = new Guid("60650051-F1F1-4dd6-90C8-5E744093D2EE");
        public static readonly Guid RejectedId = new Guid("26E27DDC-0782-4C29-B4BE-FF1E7AEE788A");
    }
}
