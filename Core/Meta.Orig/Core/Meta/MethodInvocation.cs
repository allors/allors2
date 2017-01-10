//------------------------------------------------------------------------------------------------- 
// <copyright file="MethodInvocation.cs" company="Allors bvba">
// Copyright 2002-2013 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta
{
    using System;

    public partial class MethodInvocation
    {
        private readonly ConcreteMethodType concreteMethodType;

        public MethodInvocation(Class @class, MethodType methodType)
        {
            this.concreteMethodType = @class.ConcreteMethodTypeByMethodType[methodType];
        }

        public ConcreteMethodType ConcreteConcreteMethodType
        {
            get
            {
                return this.concreteMethodType;
            }
        }

        public void Execute(Method method)
        {
            if (method.Executed)
            {
                throw new Exception("Method already executed.");
            }

            method.Executed = true;

            foreach (var action in this.concreteMethodType.Actions)
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