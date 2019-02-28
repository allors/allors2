//------------------------------------------------------------------------------------------------- 
// <copyright file="PreparedExtent.cs" company="Allors bvba">
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
    [Id("645A4F92-F1F1-41C7-BA76-53A1CC4D2A61")]
    #endregion
    public partial class PreparedExtent : UniquelyIdentifiable, Deletable
    {
        #region inherited properties

        public Guid UniqueId { get; set; }


        #endregion

        #region Allors
        [Id("CEADE44E-AA67-4E77-83FC-2C6E141A89F6")]
        [AssociationId("A0D69E1C-AC7A-4D20-91A7-0D8EB9422CFC")]
        [RoleId("A0CA7D9E-C05A-4B50-8EEA-53407BF78A3C")]
        #endregion
        [Size(256)]
        public string Name { get; set; }

        #region Allors
        [Id("03B7FB15-970F-453D-B6AC-A50654775E5E")]
        [AssociationId("9652ADA4-CCCC-471E-8A8E-FAF9D6596CDD")]
        [RoleId("55409DA7-AA4D-4132-A736-3D910F769129")]
        #endregion
        [Size(-1)]
        public string Description { get; set; }

        #region Allors
        [Id("712367B5-85ED-4623-9AC9-C082A32D8889")]
        [AssociationId("18852DD1-0F3F-4147-9298-6D5D47578CDA")]
        [RoleId("04AD0310-A894-4BCC-B30E-E7B80C6861BA")]
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

        public void OnInit()
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