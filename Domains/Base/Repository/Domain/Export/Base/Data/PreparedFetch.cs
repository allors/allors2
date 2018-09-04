//------------------------------------------------------------------------------------------------- 
// <copyright file="PreparedFetch.cs" company="Allors bvba">
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

namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("02C7569C-8F54-4F8D-AC09-1BACD9528F1F")]
    #endregion
    public partial class PreparedFetch : UniquelyIdentifiable, Deletable
    {
        #region inherited properties

        public Guid UniqueId { get; set; }


        #endregion

        #region Allors
        [Id("A8CFE3FC-EC2E-4407-B3C5-681603AA2B67")]
        [AssociationId("7B64288F-DF7F-4553-AB9E-552A118A0886")]
        [RoleId("292C9040-7F35-4A4B-86CE-5D79133CC5C6")]
        #endregion
        [Size(256)]
        public string Name { get; set; }

        #region Allors
        [Id("B5A89EE5-960F-4ABC-A43D-19438264E019")]
        [AssociationId("20E9768B-76B5-47B5-95B3-556C4E0E9EF2")]
        [RoleId("D05D37C2-5109-4C3B-BAF0-42BB7A10FB40")]
        #endregion
        [Size(-1)]
        public string Description { get; set; }

        #region Allors
        [Id("B26239E3-FA65-43F1-AAFF-0058DCCB462A")]
        [AssociationId("CF0621C6-1445-4B9E-A1D5-C859BCA09836")]
        [RoleId("D49131D7-6111-4455-B96D-1D5609995172")]
        #endregion
        [Size(-1)]
        public string Content { get; set; }

        #region inherited methods


        public void OnBuild()
        {
        }

        public void OnPostBuild()
        {
        }

        public void OnPreDerive()
        {
        }

        public void OnDerive()
        {
        }

        public void OnPostDerive()
        {
        }


        public void Delete()
        {
        }

        #endregion
    }
}