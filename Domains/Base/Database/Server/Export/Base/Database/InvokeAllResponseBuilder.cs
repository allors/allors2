// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvokeAllResponseBuilder.cs" company="Allors bvba">
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

    public class InvokeAllResponseBuilder
    {
        private readonly ISession session;
        private readonly InvokeRequest[] invokeRequests;
        private readonly User user;

        public InvokeAllResponseBuilder(ISession session, User user, InvokeRequest[] invokeRequests)
        {
            this.session = session;
            this.user = user;
            this.invokeRequests = invokeRequests;
        }

        public InvokeResponse Build()
        {
            var invokeResponse = new InvokeResponse();

            foreach (var invokeRequest in this.invokeRequests)
            {
                if (invokeRequest.M == null || invokeRequest.I == null || invokeRequest.V == null)
                {
                    throw new ArgumentException();
                }

                var obj = this.session.Instantiate(invokeRequest.I);
                var composite = (Composite)obj.Strategy.Class;
                var methodTypes = composite.WorkspaceMethodTypes;
                var methodType = methodTypes.FirstOrDefault(x => x.Name.Equals(invokeRequest.M));

                if (methodType == null)
                {
                    throw new Exception("Method " + invokeRequest.M + " not found.");
                }


                if (!invokeRequest.V.Equals(obj.Strategy.ObjectVersion.ToString()))
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
                        if (validation.HasErrors)
                        {
                            invokeResponse.AddDerivationErrors(validation);
                        }
                    }
                    else
                    {
                        invokeResponse.AddAccessError(obj);
                    }
                }

                if (invokeResponse.HasErrors)
                {
                    break;
                }
            }


            if (!invokeResponse.HasErrors)
            {
                this.session.Commit();
            }    

            return invokeResponse;
        }
    }
}