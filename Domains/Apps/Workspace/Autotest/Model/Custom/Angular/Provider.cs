// <copyright file="Provider.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Angular
{
    public partial class Provider
    {
        public override string ToString()
        {
            return this.TokenIdentifier?.ToString() ?? this.TokenValue;
        }
    }
}