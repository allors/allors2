// <copyright file="ConcreteMethodType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public sealed partial class ConcreteMethodType
    {
        public ConcreteMethodType(Class @class, MethodType methodType)
        {
            this.Class = @class;
            this.MethodType = methodType;
        }

        public MethodType MethodType { get; private set; }

        public Class Class { get; private set; }

        public IList<Action<object, object>> Actions { get; private set; }

        public void Bind(List<Domain> sortedDomains, MethodInfo[] extensionMethods, Dictionary<Type, Dictionary<MethodInfo, Action<object, object>>> actionByMethodInfoByType)
        {
            this.Actions = new List<Action<object, object>>();

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
                    var methodName = domain.Name + this.MethodType.Name;
                    var extensionMethodInfos = GetExtensionMethods(extensionMethods, @interface.ClrType, methodName);
                    if (extensionMethodInfos.Length > 1)
                    {
                        throw new Exception("Interface " + @interface + " has 2 extension methods for " + methodName);
                    }

                    if (extensionMethodInfos.Length == 1)
                    {
                        var methodInfo = extensionMethodInfos[0];

                        if (!actionByMethodInfoByType.TryGetValue(this.Class.ClrType, out var actionByMethodInfo))
                        {
                            actionByMethodInfo = new Dictionary<MethodInfo, Action<object, object>>();
                            actionByMethodInfoByType[this.Class.ClrType] = actionByMethodInfo;
                        }

                        if (!actionByMethodInfo.TryGetValue(methodInfo, out var action))
                        {
                            var o = Expression.Parameter(typeof(object));
                            var castO = Expression.Convert(o, methodInfo.GetParameters()[0].ParameterType);

                            var p = Expression.Parameter(typeof(object));
                            var castP = Expression.Convert(p, methodInfo.GetParameters()[1].ParameterType);

                            Expression call = Expression.Call(methodInfo, new Expression[] { castO, castP });

                            action = Expression.Lambda<Action<object, object>>(call, o, p).Compile();
                            actionByMethodInfo[methodInfo] = action;
                        }

                        this.Actions.Add(action);
                    }
                }
            }

            // Class
            {
                foreach (var domain in sortedDomains)
                {
                    var methodName = domain.Name + this.MethodType.Name;

                    var methodInfo = this.Class.ClrType.GetTypeInfo().GetDeclaredMethod(methodName);
                    if (methodInfo != null)
                    {
                        var o = Expression.Parameter(typeof(object));
                        var castO = Expression.Convert(o, this.Class.ClrType);

                        var p = Expression.Parameter(typeof(object));
                        var castP = Expression.Convert(p, methodInfo.GetParameters()[0].ParameterType);

                        Expression call = Expression.Call(castO, methodInfo, castP);

                        var action = Expression.Lambda<Action<object, object>>(call, o, p).Compile();
                        this.Actions.Add(action);
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
