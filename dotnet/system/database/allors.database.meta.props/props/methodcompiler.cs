// <copyright file="MethodInvocation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Meta
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public partial class MethodCompiler
    {
        private readonly Dictionary<Type, MethodInfo[]> extensionMethodsByInterface;
        private readonly List<IDomainBase> sortedDomains;
        private readonly ConcurrentDictionary<Type, Dictionary<MethodInfo, Action<object, object>>> actionByMethodInfoByType;

        public MethodCompiler(IMetaPopulationBase metaPopulation, Dictionary<Type, MethodInfo[]> extensionMethodsByInterface)
        {
            this.extensionMethodsByInterface = extensionMethodsByInterface;
            this.sortedDomains = new List<IDomainBase>(metaPopulation.Domains);
            this.sortedDomains.Sort((a, b) => a.Superdomains.Contains(b) ? -1 : 1);

            this.actionByMethodInfoByType = new ConcurrentDictionary<Type, Dictionary<MethodInfo, Action<object, object>>>();
        }

        public Action<object, object>[] Compile(IClass @class, IMethodType methodType)
        {
            var actions = new List<Action<object, object>>();

            var interfaces = new List<IInterface>(@class.Supertypes);

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
                foreach (var domain in this.sortedDomains)
                {
                    var methodName = domain.Name + methodType.Name;
                    var extensionMethodInfos = this.GetExtensionMethods(@interface.ClrType, methodName);
                    if (extensionMethodInfos.Length > 1)
                    {
                        throw new Exception("Interface " + @interface + " has 2 extension methods for " + methodName);
                    }

                    if (extensionMethodInfos.Length == 1)
                    {
                        var methodInfo = extensionMethodInfos[0];

                        if (!this.actionByMethodInfoByType.TryGetValue(@class.ClrType, out var actionByMethodInfo))
                        {
                            actionByMethodInfo = new Dictionary<MethodInfo, Action<object, object>>();
                            this.actionByMethodInfoByType[@class.ClrType] = actionByMethodInfo;
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

                        actions.Add(action);
                    }
                }
            }

            // Class
            {
                foreach (var domain in this.sortedDomains)
                {
                    var methodName = domain.Name + methodType.Name;

                    var methodInfo = @class.ClrType.GetTypeInfo().GetDeclaredMethod(methodName);
                    if (methodInfo != null)
                    {
                        var o = Expression.Parameter(typeof(object));
                        var castO = Expression.Convert(o, @class.ClrType);

                        var p = Expression.Parameter(typeof(object));
                        var castP = Expression.Convert(p, methodInfo.GetParameters()[0].ParameterType);

                        Expression call = Expression.Call(castO, methodInfo, castP);

                        var action = Expression.Lambda<Action<object, object>>(call, o, p).Compile();
                        actions.Add(action);
                    }
                }
            }

            return actions.ToArray();
        }

        private MethodInfo[] GetExtensionMethods(Type @interface, string methodName) => !this.extensionMethodsByInterface.TryGetValue(@interface, out var extensionMethods) ?
            Array.Empty<MethodInfo>() :
            extensionMethods.Where(method => method.Name.Equals(methodName)).ToArray();
    }
}
