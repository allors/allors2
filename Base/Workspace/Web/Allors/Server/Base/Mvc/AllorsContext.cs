// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllorsContext.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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

namespace Allors.Web.Mvc
{
    using System;
    using System.Web;

    public partial class AllorsContext : IDisposable
    {
        private const string Key = "AllorsContext";

        private bool disposed;
        private ISession session;

        ~AllorsContext()
        {
            this.Dispose();
        }

        public static AllorsContext Instance
        {
            get
            {
                return (AllorsContext)HttpContext.Current.Items[Key];
            }

            set
            {
                HttpContext.Current.Items[Key] = value;
            }
        }

        public ISession Session
        {
            get
            {
                if (!this.disposed && this.session == null)
                {
                    this.session = Config.Default.CreateSession();
                }

                return this.session;
            }
        }

        public virtual void Dispose()
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                if (disposing)
                {
                    // Free any other managed objects here. 
                    if (this.session != null)
                    {
                        try
                        {
                            this.session.Rollback();
                        }
                        finally
                        {
                            this.session = null;
                        }
                    }
                }

                // Free any unmanaged objects here. 
                
                this.disposed = true;
            }
        }
    }
}