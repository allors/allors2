// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardJsonResult.cs" company="Allors bvba">
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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    public class StandardJsonResult : JsonResult
    {
        public IList<string> ErrorMessages { get; private set; }

        public StandardJsonResult()
        {
            this.ErrorMessages = new List<string>();
        }

        public void AddError(string errorMessage)
        {
            this.ErrorMessages.Add(errorMessage);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("GET access is not allowed.  Change the JsonRequestBehavior if you need GET access.");
            }

            var response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }

            this.SerializeData(response);
        }

        protected virtual void SerializeData(HttpResponseBase response)
        {
            if (this.ErrorMessages.Any())
            {
                var originalData = this.Data;
                this.Data = new
                {
                    Success = false,
                    OriginalData = originalData,
                    ErrorMessage = string.Join("\n", this.ErrorMessages),
                    ErrorMessages = this.ErrorMessages.ToArray()
                };

                response.StatusCode = 400;
            }

            var serializer = new JsonSerializer { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            serializer.Converters.Add(new StringEnumConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (var streamWriter = new StreamWriter(response.OutputStream))
            {
                using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
                {
                    serializer.Serialize(jsonWriter, this.Data);
                }
            }
        }
    }

    public class StandardJsonResult<T> : StandardJsonResult
    {
        public new T Data
        {
            get { return (T)base.Data; }
            set { base.Data = value; }
        }
    }
}