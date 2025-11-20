// <copyright file="C1.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Person type.</summary>

namespace Allors.Domain
{
    public partial class C1
    {
        public void CoreClassMethod(C1ClassMethod method) => method.Value += "C1Core";

        public void CustomClassMethod(C1ClassMethod method) => method.Value += "C1Custom";

        public void CoreInterfaceMethod(I1InterfaceMethod method) => method.Value += "C1Core";

        public void CustomInterfaceMethod(I1InterfaceMethod method) => method.Value += "C1Custom";

        public void CoreSuperinterfaceMethod(S1SuperinterfaceMethod method) => method.Value += "C1Core";

        public void CustomSuperinterfaceMethod(S1SuperinterfaceMethod method) => method.Value += "C1Custom";
    }
}
