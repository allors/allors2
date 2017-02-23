//------------------------------------------------------------------------------------------------- 
// <copyright file="SyncedAttribute.cs" company="Allors bvba">
// Copyright 2002-2016 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the Extent type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Repository.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class SyncedAttribute : RepositoryAttribute
    {
        public SyncedAttribute(bool value = true)
        {
            this.Value = value;
        }
        
        public bool Value { get; set; }
    }
}