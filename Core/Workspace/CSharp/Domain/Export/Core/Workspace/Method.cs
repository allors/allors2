// <copyright file="Method.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    public class Method
    {
        public Method(SessionObject @object, string name)
        {
            this.Object = @object;
            this.Name = name;
        }

        public SessionObject Object { get; }

        public string Name { get; }
    }
}
