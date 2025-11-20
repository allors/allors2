// <copyright file="TemplateCacheEntry.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public class TemplateCacheEntry
    {
        internal TemplateCacheEntry(Template template, object subject)
        {
            this.Revision = template.Media.Revision.Value;
            this.Subject = subject;
        }

        public Guid Revision { get; }

        public object Subject { get; }
    }
}
