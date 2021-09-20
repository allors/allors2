// <copyright file="MethodInvocation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Meta
{
    using System;
    using System.Diagnostics;

    public partial class MethodInvocation
    {
        public MethodInvocation(Class @class, MethodType methodType) => this.ConcreteConcreteMethodType = @class.ConcreteMethodTypeByMethodType[methodType];

        public ConcreteMethodType ConcreteConcreteMethodType { get; private set; }

        [DebuggerStepThrough]
        public void Execute(Method method)
        {
            if (method.Executed)
            {
                throw new Exception("Method already executed.");
            }

            method.Executed = true;

            foreach (var action in this.ConcreteConcreteMethodType.Actions)
            {
                // TODO: Add test for deletion
                if (!method.Object.Strategy.IsDeleted)
                {
                    action(method.Object, method);
                }
            }
        }
    }
}
