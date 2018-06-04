// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestTemplateService.cs" company="Allors bvba">
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

using System;
using System.Threading.Tasks;

namespace Allors.Services
{
    public class ProxyTemplateService : ITemplateService
    {
        public static Func<string, object, Task<string>> Subject = async (viewName, model) => ""; 
        
        public Task<string> Render<TModel>(string viewName, TModel model)
        {
            return Subject(viewName, model);
        }
    }
}
