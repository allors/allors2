// <copyright file="InvokeOptions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    public class InvokeOptions
    {
        public bool Isolated { get; set; } = false;

        public bool ContinueOnError { get; set; } = false;
    }
}
