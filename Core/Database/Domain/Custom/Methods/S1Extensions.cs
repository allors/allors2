// <copyright file="S1Extensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Person type.</summary>

namespace Allors.Domain
{
    public static partial class S1Extensions
    {
        public static void CoreSuperinterfaceMethod(this S1 @this, S1SuperinterfaceMethod method) => method.Value += "S1Core";

        public static void CustomSuperinterfaceMethod(this S1 @this, S1SuperinterfaceMethod method) => method.Value += "S1Custom";
    }
}
