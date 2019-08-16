// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemplateType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    public partial class TemplateType
    {
        public bool IsOpenDocumentTemplate => this.UniqueId.Equals(TemplateTypes.OpenDocumentTypeId);
    }
}
