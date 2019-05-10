// <copyright file="Attribute.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Html
{
    public partial class Attribute
    {
        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(this.Value) ? this.Name : $"{this.Name}: {this.Value}";
        }
    }
}