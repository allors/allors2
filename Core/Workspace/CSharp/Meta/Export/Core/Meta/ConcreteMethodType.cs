//------------------------------------------------------------------------------------------------- 
// <copyright file="ConcreteRoleType.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
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
// <summary>Defines the IObjectType type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Workspace.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public sealed partial class ConcreteMethodType
    {
        private readonly Class @class;

        private readonly MethodType methodType;

        private IList<Action<object, object>> actions;

        public ConcreteMethodType(Class @class, MethodType methodType)
        {
            this.@class = @class;
            this.methodType = methodType;
        }
        
        public MethodType MethodType
        {
            get
            {
                return this.methodType;
            }
        }

        public Class Class
        {
            get
            {
                return this.@class;
            }
        }

        public IList<Action<object, object>> Actions
        {
            get
            {
                return this.actions;
            }

            set
            {
                this.actions = value;
            }
        }

        public void Bind(List<Domain> sortedDomains, MethodInfo[] extensionMethods, Dictionary<Type, Dictionary<MethodInfo, Action<object, object>>> actionByMethodInfoByType)
        {
            this.actions = new List<Action<object, object>>();

            var interfaces = new List<Interface>(this.Class.Supertypes);
            
            interfaces.Sort(
                (a, b) =>
                {
                    if (a.Supertypes.Contains(b))
                    {
                        return 1;
                    }

                    if (a.Subtypes.Contains(b))
                    {
                        return -1;
                    }

                    return string.Compare(a.Name, b.Name, StringComparison.Ordinal);
                });

            // Interface
            foreach (var @interface in interfaces)
            {
                foreach (var domain in sortedDomains)
                {
                    var methodName = domain.Name + this.methodType.Name;
                    var extensionMethodInfos = GetExtensionMethods(extensionMethods, @interface.ClrType, methodName);
                    if (extensionMethodInfos.Length > 1)
                    {
                        throw new Exception("Interface " + @interface + " has 2 extension methods for " + methodName);
                    }

                    if (extensionMethodInfos.Length == 1)
                    {
                        var methodInfo = extensionMethodInfos[0];

                        Dictionary<MethodInfo, Action<object, object>> actionByMethodInfo;
                        if (!actionByMethodInfoByType.TryGetValue(this.Class.ClrType, out actionByMethodInfo))
                        {
                            actionByMethodInfo = new Dictionary<MethodInfo, Action<object, object>>();
                            actionByMethodInfoByType[this.Class.ClrType] = actionByMethodInfo;
                        }

                        Action<object, object> action;
                        if (!actionByMethodInfo.TryGetValue(methodInfo, out action))
                        {
                            var o = Expression.Parameter(typeof(object));
                            var castO = Expression.Convert(o, methodInfo.GetParameters()[0].ParameterType);

                            var p = Expression.Parameter(typeof(object));
                            var castP = Expression.Convert(p, methodInfo.GetParameters()[1].ParameterType);

                            Expression call = Expression.Call(methodInfo, new Expression[] { castO, castP });

                            action = Expression.Lambda<Action<object, object>>(call, o, p).Compile();
                            actionByMethodInfo[methodInfo] = action;
                        }

                        this.actions.Add(action);
                    }
                }
            }

            // Class
            {
                foreach (var domain in sortedDomains)
                {
                    var methodName = domain.Name + this.methodType.Name;

                    var methodInfo = this.Class.ClrType.GetTypeInfo().GetDeclaredMethod(methodName);
                    if (methodInfo != null)
                    {
                        var o = Expression.Parameter(typeof(object));
                        var castO = Expression.Convert(o, this.Class.ClrType);

                        var p = Expression.Parameter(typeof(object));
                        var castP = Expression.Convert(p, methodInfo.GetParameters()[0].ParameterType);

                        Expression call = Expression.Call(castO, methodInfo, castP);

                        var action = Expression.Lambda<Action<object, object>>(call, o, p).Compile();
                        this.actions.Add(action);
                    }
                }
            }
        }

        private static MethodInfo[] GetExtensionMethods(MethodInfo[] extensionMethods, Type @interface, string methodName)
        {
            var query = from method in extensionMethods
                        where method.Name.Equals(methodName)
                        where method.GetParameters()[0].ParameterType == @interface
                        select method;
            return query.ToArray();
        }
    }
}