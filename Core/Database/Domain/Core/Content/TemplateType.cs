// <copyright file="TemplateType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class TemplateType
    {
        public bool IsOpenDocumentTemplate => this.UniqueId.Equals(TemplateTypes.OpenDocumentTypeId);
    }
}
