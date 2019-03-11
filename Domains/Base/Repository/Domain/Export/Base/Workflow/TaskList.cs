//------------------------------------------------------------------------------------------------- 
// <copyright file="TaskList.cs" company="Allors bvba">
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
    using Attributes;

    #region Allors
    [Id("1c2303a1-f3ce-4084-a1ad-fc25156ac542")]
    #endregion
    public partial class TaskList : Deletable 
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("c3467693-cc18-46e5-a0af-d7ab0cbe9faa")]
        [AssociationId("7976dbaa-9b96-401f-900d-db76fd45f18f")]
        [RoleId("3922d9e8-e518-4459-8b52-0723104456ab")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        #endregion
        public TaskAssignment[] TaskAssignments { get; set; }
        
        #region Allors
        [Id("2e7381c6-3a58-4a64-8808-4f3532254f08")]
        [AssociationId("63efedc3-1157-4ae0-b212-9169cd0ac418")]
        [RoleId("4f83aaac-7ba1-4fdc-9ddf-781559ff3983")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        #endregion
        public TaskAssignment[] OpenTaskAssignments { get; set; }

        #region Allors
        [Id("bc07648e-b80c-42f6-a4dd-113fba962c89")]
        [AssociationId("c3b078cf-27ee-4686-b7ff-ba40a7aba5a7")]
        [RoleId("ef37d700-cfa6-4998-9501-9d09bb9ac1d8")]
        [Derived]
        [Indexed]
        [Workspace]
        #endregion
        public int Count { get; set; }
        
        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void Delete(){}
        #endregion
    }
}