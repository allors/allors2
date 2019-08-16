//-------------------------------------------------------------------------------------------------
// <copyright file="SuperinterfaceMethod.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Person type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    public partial class C1
    {
        public void CoreSuperinterfaceMethod(S1SuperinterfaceMethod method) => method.Value += "C1Core";

        public void CustomSuperinterfaceMethod(S1SuperinterfaceMethod method) => method.Value += "C1Custom";
    }

    public static partial class I1Extensions
    {
        public static void CoreSuperinterfaceMethod(this I1 @this, S1SuperinterfaceMethod method) => method.Value += "I1Core";

        public static void CustomSuperinterfaceMethod(this I1 @this, S1SuperinterfaceMethod method) => method.Value += "I1Custom";
    }

    public static partial class S1Extensions
    {
        public static void CoreSuperinterfaceMethod(this S1 @this, S1SuperinterfaceMethod method) => method.Value += "S1Core";

        public static void CustomSuperinterfaceMethod(this S1 @this, S1SuperinterfaceMethod method) => method.Value += "S1Custom";
    }

    public partial class S1SuperinterfaceMethod
    {
        public string Value { get; set; }
    }
}
