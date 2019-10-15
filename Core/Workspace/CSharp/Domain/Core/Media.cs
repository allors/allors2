// <copyright file="PurchaseInvoiceStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Domain
{
    using System.Globalization;

    public partial class Media
    {
        public string Source => "/Media/DownloadWithRevision/" + this.UniqueId + "?revision=" + this.Revision;
    }
}
