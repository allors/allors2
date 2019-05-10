// <copyright file="Pipe.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Autotest.Angular
{
    public partial class Pipe
    {
        public override string ToString()
        {
            return this.Reference.ToString();
        }
    }
}