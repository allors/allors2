// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvokeResponseBuilder.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Server
{
    using System;
    using System.Linq;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Server.Protocol.Invoke;

    public class InvokeResponseBuilder
    {
        private readonly ISession session;
        private readonly User user;
        private readonly Invocation[] invocations;

        private readonly bool isolated;
        private readonly bool continueOnError;

        public InvokeResponseBuilder(ISession session, User user, InvokeRequest invokeRequest)
        {
            this.session = session;
            this.user = user;
            this.invocations = invokeRequest.I;
            this.isolated = invokeRequest.O?.I ?? false;
            this.continueOnError = invokeRequest.O?.C ?? false;
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
                    var validation = this.session.Derive(false);
                    if (validation.HasErrors)
                    {
                        invokeResponse.AddDerivationErrors(validation);
                    }
                    else
                    {
                        this.session.Commit();
                    }
                }
            }

            return invokeResponse;
        }

        private bool Invoke(Invocation invocation, InvokeResponse invokeResponse)
        {
            if (invocation.M == null || invocation.I == null || invocation.V == null)
            {
                throw new ArgumentException();
            }

            var obj = this.session.Instantiate(invocation.I);
            if (obj == null)
            {
                invokeResponse.AddMissingError(invocation.I);
                return true;
            }

            var composite = (Composite)obj.Strategy.Class;
            var methodTypes = composite.WorkspaceMethodTypes;
            var methodType = methodTypes.FirstOrDefault(x => x.Name.Equals(invocation.M));

            if (methodType == null)
            {
                throw new Exception("Method " + invocation.M + " not found.");
            }

            if (!invocation.V.Equals(obj.Strategy.ObjectVersion.ToString()))
            {
                invokeResponse.AddVersionError(obj);
                return true;
            }

            var acl = new AccessControlList(obj, this.user);
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

                invokeResponse.ErrorMessage = innerException.Message;
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