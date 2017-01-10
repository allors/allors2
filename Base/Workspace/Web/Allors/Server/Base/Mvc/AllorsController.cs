// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Controller.cs" company="Allors bvba">
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

using System.Linq;

namespace Allors.Web.Mvc
{
    using System;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Allors.Domain;
    using Allors.Meta;

    public abstract partial class AllorsController : System.Web.Mvc.Controller
    {
        private ISession allorsSession;
        private User allorsUser;

        ~AllorsController()
        {
            this.DisposeAllorsSession();
        }

        [Obsolete("Do not use the standard Json helpers to return JSON data to the client.  Use either JsonSuccess or JsonError instead.")]
        protected JsonResult Json<T>(T data)
        {
            throw new InvalidOperationException("Do not use the standard Json helpers to return JSON data to the client.  Use either JsonSuccess or JsonError instead.");
        }

        protected StandardJsonResult JsonValidationError()
        {
            var result = new StandardJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            foreach (var validationError in ModelState.Values.SelectMany(v => v.Errors))
            {
                result.AddError(validationError.ErrorMessage);
            }
            return result;
        }

        protected StandardJsonResult JsonError(string errorMessage)
        {
            var result = new StandardJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            result.AddError(errorMessage);

            return result;
        }

        protected StandardJsonResult<T> JsonSuccess<T>(T data)
        {
            return new StandardJsonResult<T> { Data = data , JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ISession AllorsSession
        {
            get
            {
                return this.allorsSession;
            }

            set
            {
                if (this.allorsSession != null)
                {
                    throw new ArgumentException("AllorsSession is already set");
                }

                this.allorsSession = value;
            }
        }

        public virtual User AllorsUser
        {
            get
            {
                if (this.allorsUser == null && this.User != null)
                {
                    var userName = this.User.Identity.Name;

                    if (!string.IsNullOrWhiteSpace(userName))
                    {
                        this.allorsUser = new Users(this.allorsSession).FindBy(M.User.UserName, userName);
                    }

                    if (this.allorsUser == null)
                    {
                        this.allorsUser = Singleton.Instance(this.AllorsSession).Guest;
                    }
                }

                return this.allorsUser;
            }

            set
            {
                this.allorsUser = value;
            }
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            var locale = this.AllorsUser?.Locale;
            var cultureInfo = locale?.CultureInfo;

            if (cultureInfo != null)
            {
                var originalCurrentCulture = Thread.CurrentThread.CurrentCulture;
                var originalCurrentUICulture = Thread.CurrentThread.CurrentUICulture;

                try
                {
                    Thread.CurrentThread.CurrentCulture = cultureInfo;
                    Thread.CurrentThread.CurrentUICulture = cultureInfo;
                }
                catch
                {
                    Thread.CurrentThread.CurrentCulture = originalCurrentCulture;
                    Thread.CurrentThread.CurrentUICulture = originalCurrentUICulture;
                }
            }

            return base.BeginExecuteCore(callback, state);
        }

        protected override void Initialize(RequestContext requestContext)
        {
            if (this.allorsSession == null)
            {
                this.allorsSession = Config.Default.CreateSession();
            }

            base.Initialize(requestContext);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DisposeAllorsSession();
            }

            base.Dispose(disposing);
        }

        private void DisposeAllorsSession()
        {
            var session = this.allorsSession;
            if (session != null)
            {
                try
                {
                    session.Dispose();
                }
                finally
                {
                    this.allorsSession = null;
                }
            }
        }
    }
}