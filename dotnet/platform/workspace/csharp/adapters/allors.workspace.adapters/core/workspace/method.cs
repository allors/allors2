// <copyright file="Method.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Meta;

    public class Method
    {
        public Method(SessionObject @object, IMethodType methodType)
        {
            this.Object = @object;
            this.MethodType = methodType;
        }

        public SessionObject Object { get; }

        public IMethodType MethodType { get; }
    }
}
