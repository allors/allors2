// ------------------------------------------------------------------------------------------------
// <copyright file="MediaController.cs" company="Allors bvba">
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
// ------------------------------------------------------------------------------------------------

namespace Allors.Web.Media
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    public abstract partial class BaseMediaController : Controller
    {
        public virtual ActionResult Display(string id, string revision)
        {
            Guid uniqueId;
            if (Guid.TryParse(id, out uniqueId))
            {
                using (var session = Config.Default.CreateSession())
                {
                    var mediaContent = new Medias(session).FindBy(M.Media.UniqueId, uniqueId)?.MediaContent;

                    if (mediaContent != null)
                    {
                        this.Response.Cache.SetCacheability(HttpCacheability.Public);
                        this.Response.Cache.SetExpires(DateTime.Now.AddYears(1));
                        this.Response.Cache.SetValidUntilExpires(true); 

                        return this.File(mediaContent.Data, mediaContent.Type);
                    }
                }
            }

            return this.HttpNotFound("Media with unique id " + uniqueId + " not found.");
        }
    }
}