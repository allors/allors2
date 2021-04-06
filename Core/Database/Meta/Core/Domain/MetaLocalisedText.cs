// <copyright file="MetaLocalisedText.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
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
