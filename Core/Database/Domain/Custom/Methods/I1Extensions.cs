// <copyright file="I1Extensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Person type.</summary>

namespace Allors.Domain
{
    public static partial class I1Extensions
    {
        public static void CoreInterfaceMethod(this I1 @this, I1InterfaceMethod method) => method.Value += "I1Core";

        public static void CustomInterfaceMethod(this I1 @this, I1InterfaceMethod method) => method.Value += "I1Custom";
    }
}
