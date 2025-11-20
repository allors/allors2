// <copyright file="MetaLocalisedText.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Meta
{
    public partial class MetaLocalisedText
    {
        internal override void CoreExtend()
        {
            this.Locale.IsRequiredOverride = true;
        }
    }
}
