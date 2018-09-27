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
        private readonly InvokeRequest invokeRequest;
        private readonly User user;

        public InvokeResponseBuilder(ISession session, User user, InvokeRequest invokeRequest)
        {
            this.session = session;
            this.user = user;
            this.invokeRequest = invokeRequest;
        }

        public InvokeResponse Build()
        {
            if (this.invokeRequest.M == null || this.invokeRequest.I == null || this.invokeRequest.V == null)
            {
                throw new ArgumentException();
            }

            var obj = this.session.Instantiate(this.invokeRequest.I);
            var composite = (Composite)obj.Strategy.Class;
            var methodTypes = composite.WorkspaceMethodTypes;
            var methodType = methodTypes.FirstOrDefault(x => x.Name.Equals(this.invokeRequest.M));

            if (methodType == null)
            {
                throw new Exception("Method " + this.invokeRequest.M + " not found.");   
            }
            
            var invokeResponse = new InvokeResponse();

            if (!this.invokeRequest.V.Equals(obj.Strategy.ObjectVersion.ToString()))
            {
                invokeResponse.AddVersionError(obj);
            }
            else
            {
                var acl = new AccessControlList(obj, this.user);
                if (acl.CanExecute(methodType))
                {
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
                    }

                    var validation = this.session.Derive(false);
                    if (!validation.HasErrors)
                    {
                        this.session.Commit();
                    }
                    else
                    {
                        invokeResponse.AddDerivationErrors(validation);
                    }
                }
                else
                {
                    invokeResponse.AddAccessError(obj);
                }
            }

            return invokeResponse;
        }
    }
}