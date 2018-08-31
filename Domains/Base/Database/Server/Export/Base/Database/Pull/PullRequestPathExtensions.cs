// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PullRequestPathExtensions.cs" company="Allors bvba">
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

    using Allors.Data;
    using Allors.Meta;
    using Allors.Server.Protocol.Pull;

    public static class PullRequestPathExtensions
    {
        public static void Parse(this PullPath @this, Path path, IMetaPopulation metaPopulation)
        {
            var propertyType = (IPropertyType)metaPopulation.Find(new Guid(@this.Step));
            path.PropertyType = propertyType;

            if (@this.Next != null)
            {
                path.Next = new Path();
                @this.Next.Parse(path.Next, metaPopulation);
            }
        }
    }
}