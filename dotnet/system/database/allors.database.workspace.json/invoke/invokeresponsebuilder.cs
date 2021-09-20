// <copyright file="InvokeResponseBuilder.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Protocol.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Meta;
    using Allors.Protocol.Json.Api.Invoke;
    using Derivations;
    using Security;

    public class InvokeResponseBuilder
    {
        private readonly ITransaction transaction;
        private readonly Func<IValidation> derive;
        private readonly IAccessControl accessControl;
        private readonly ISet<IClass> allowedClasses;

        public InvokeResponseBuilder(ITransaction transaction, Func<IValidation> derive, IAccessControl accessControl, ISet<IClass> allowedClasses)
        {
            this.transaction = transaction;
            this.derive = derive;
            this.accessControl = accessControl;
            this.allowedClasses = allowedClasses;
        }

        public InvokeResponse Build(InvokeRequest invokeRequest)
        {
            var invocations = invokeRequest.l;
            var isolated = invokeRequest.o?.i ?? false;
            var continueOnError = invokeRequest.o?.c ?? false;

            var invokeResponse = new InvokeResponse();
            if (isolated)
            {
                foreach (var invocation in invocations)
                {
                    var error = this.Invoke(invocation, invokeResponse);
                    if (!error)
                    {
                        var validation = this.derive();
                        if (validation.HasErrors)
                        {
                            error = true;
                            invokeResponse.AddDerivationErrors(validation);
                        }
                    }

                    if (error)
                    {
                        this.transaction.Rollback();
                        if (!continueOnError)
                        {
                            break;
                        }
                    }
                    else
                    {
                        this.transaction.Commit();
                    }
                }
            }
            else
            {
                var error = false;
                foreach (var invocation in invocations)
                {
                    error = this.Invoke(invocation, invokeResponse);

                    if (!error)
                    {
                        var validation = this.derive();
                        if (validation.HasErrors)
                        {
                            error = true;
                            invokeResponse.AddDerivationErrors(validation);
                        }
                    }

                    if (error)
                    {
                        break;
                    }
                }

                if (error)
                {
                    this.transaction.Rollback();
                }
                else
                {
                    this.transaction.Commit();
                }
            }

            return invokeResponse;
        }

        private bool Invoke(Invocation invocation, InvokeResponse invokeResponse)
        {
            if (string.IsNullOrWhiteSpace(invocation.m) || invocation.i == 0 || invocation.v == 0)
            {
                throw new ArgumentException();
            }

            var obj = this.transaction.Instantiate(invocation.i);
            if (obj == null)
            {
                invokeResponse.AddMissingError(invocation.i);
                return true;
            }

            if (this.allowedClasses?.Contains(obj.Strategy.Class) != true)
            {
                invokeResponse.AddAccessError(obj);
                return true;
            }

            var composite = (IComposite)obj.Strategy.Class;

            // TODO: Cache and filter for workspace
            var methodTypes = composite.MethodTypes.Where(v => v.WorkspaceNames.Length > 0);
            var methodType = methodTypes.FirstOrDefault(v => v.Tag.Equals(invocation.m));

            if (methodType == null)
            {
                throw new Exception("Method " + invocation.m + " not found.");
            }

            if (!invocation.v.Equals(obj.Strategy.ObjectVersion))
            {
                invokeResponse.AddVersionError(obj);
                return true;
            }

            var acl = this.accessControl[obj];
            if (!acl.CanExecute(methodType))
            {
                invokeResponse.AddAccessError(obj);
                return true;
            }

            var method = obj.GetType().GetMethod(methodType.Name, new Type[] { });

            try
            {
                method.Invoke(obj, null);
            }
            catch (Exception e)
            {
                var innerException = e;
                while (innerException.InnerException != null)
                {
                    innerException = innerException.InnerException;
                }

                invokeResponse._e = innerException.Message;
                return true;
            }

            var validation = this.derive();
            if (validation.HasErrors)
            {
                invokeResponse.AddDerivationErrors(validation);
                return true;
            }

            return false;
        }
    }
}
