// <copyright file="MediaContents.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;
    using System.IO;
    using HeyRed.Mime;
    using Meta;

    public partial class MediaContent
    {
        public void CoreOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistData || this.Data.Length == 0)
            {
                derivation.Validation.AddError(this, Meta.Data, "Empty data");
            }
        }
    }
}
