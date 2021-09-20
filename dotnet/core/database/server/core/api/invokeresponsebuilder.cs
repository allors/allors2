// <copyright file="InvokeResponseBuilder.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System;
    using System.Linq;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Protocol.Remote.Invoke;

    public class InvokeResponseBuilder
    {
        private readonly ISession session;
        private readonly IAccessControlLists acls;
        private readonly Invocation[] invocations;
        private readonly bool isolated;
        private readonly bool continueOnError;

        public InvokeResponseBuilder(ISession session, InvokeRequest invokeRequest, IAccessControlLists acls)
        {
            this.session = session;
            this.invocations = invokeRequest.i;
            this.isolated = invokeRequest.o?.i ?? false;
            this.continueOnError = invokeRequest.o?.c ?? false;

            this.acls = acls;
        }

        public InvokeResponse Build()
        {
            var invokeResponse = new InvokeResponse();
            if (this.isolated)
            {
                foreach (var invocation in this.invocations)
                {
                    var error = this.Invoke(invocation, invokeResponse);
                    if (!error)
                    {
                        var validation = this.session.Derive(false);
                        if (validation.HasErrors)
                        {
                            error = true;
                            invokeResponse.AddDerivationErrors(validation);
                        }
                    }

                    if (error)
                    {
                        this.session.Rollback();
                        if (!this.continueOnError)
                        {
                            break;
                        }
                    }
                    else
                    {
                        this.session.Commit();
                    }
                }
            }
            else
            {
                var error = false;
                foreach (var invocation in this.invocations)
                {
                    error = this.Invoke(invocation, invokeResponse);
                    if (error)
                    {
                        break;
                    }
                }

                if (error)
                {
                    this.session.Rollback();
                }
                else
                {
                    this.session.Commit();
                }
            }

            return invokeResponse;
        }

        private bool Invoke(Invocation invocation, InvokeResponse invokeResponse)
        {
            // TODO: M should be a methodTypeId instead of the methodName
            if (invocation.m == null || invocation.i == null || invocation.v == null)
            {
                throw new ArgumentException();
            }

            var obj = this.session.Instantiate(invocation.i);
            if (obj == null)
            {
                invokeResponse.AddMissingError(invocation.i);
                return true;
            }

            var composite = (Composite)obj.Strategy.Class;
            var methodTypes = composite.WorkspaceMethodTypes;
            var methodType = methodTypes.FirstOrDefault(x => x.Id.Equals(Guid.Parse(invocation.m)));

            if (methodType == null)
            {
                throw new Exception("Method " + invocation.m + " not found.");
            }

            if (!invocation.v.Equals(obj.Strategy.ObjectVersion.ToString()))
            {
                invokeResponse.AddVersionError(obj);
                return true;
            }

            var acl = this.acls[obj];
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

                invokeResponse.errorMessage = innerException.Message;
                return true;
            }

            var validation = this.session.Derive(false);
            if (validation.HasErrors)
            {
                invokeResponse.AddDerivationErrors(validation);
                return true;
            }

            return false;
        }
    }
}
