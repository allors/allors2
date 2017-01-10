//------------------------------------------------------------------------------------------------- 
// <copyright file="C1ClassMethod.cs" company="Allors bvba">
// Copyright 2002-2016 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the Person type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    public partial class C1
    {
        public void CoreSuperinterfaceMethod(S1SuperinterfaceMethod method)
        {
            method.Value += "C1Core";
        }

        public void BaseSuperinterfaceMethod(S1SuperinterfaceMethod method)
        {
            method.Value += "C1Base";
        }

        public void CustomSuperinterfaceMethod(S1SuperinterfaceMethod method)
        {
            method.Value += "C1Custom";
        }
    }

    public static partial class I1Extensions
    {
        public static void CoreSuperinterfaceMethod(this I1 @this, S1SuperinterfaceMethod method)
        {
            method.Value += "I1Core";
        }

        public static void BaseSuperinterfaceMethod(this I1 @this, S1SuperinterfaceMethod method)
        {
            method.Value += "I1Base";
        }

        public static void CustomSuperinterfaceMethod(this I1 @this, S1SuperinterfaceMethod method)
        {
            method.Value += "I1Custom";
        }
    }

    public static partial class S1Extensions
    {
        public static void CoreSuperinterfaceMethod(this S1 @this, S1SuperinterfaceMethod method)
        {
            method.Value += "S1Core";
        }

        public static void BaseSuperinterfaceMethod(this S1 @this, S1SuperinterfaceMethod method)
        {
            method.Value += "S1Base";
        }

        public static void CustomSuperinterfaceMethod(this S1 @this, S1SuperinterfaceMethod method)
        {
            method.Value += "S1Custom";
        }
    }

    public partial class S1SuperinterfaceMethod
    {
        public string Value { get; set; }
    }
}
