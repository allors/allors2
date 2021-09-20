// <copyright file="Method.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Meta
{
    public abstract partial class Method
    {
        protected Method(IObject @object)
        {
            this.Executed = false;
            this.Object = @object;
        }

        public abstract MethodInvocation MethodInvocation { get; }

        public IObject Object { get; private set; }

        public bool Executed { get; set; }

        public bool StopPropagation { get; set; }

        public virtual void Execute() => this.MethodInvocation.Execute(this);
    }
}
