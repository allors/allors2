//------------------------------------------------------------------------------------------------- 
// <copyright file="Task.cs" company="Allors bvba">
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

    using Allors.Repository.Attributes;

    #region Allors
    [Id("84eb0e6e-68e1-478c-a35f-6036d45792be")]
    #endregion
    public partial interface Task : AccessControlledObject, UniquelyIdentifiable, Deletable
    {
        #region Allors
        [Id("f247de73-70fe-47e4-a763-22ee9c68a476")]
        [AssociationId("2e1ebe97-52d3-46fc-94c2-3203a13856c7")]
        [RoleId("4ca8997f-9232-4c84-8f37-e977071eb316")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        #endregion
        [Workspace]
        [Required]
        WorkItem WorkItem { get; set; }

        #region Allors
        [Id("8ebd9048-a344-417c-bae7-359ca9a74aa1")]
        [AssociationId("af6cbf34-5f71-498b-a2ec-ef698eeae799")]
        [RoleId("ceba2888-2a6e-4822-881b-1101b48f80f3")]
        [Derived]
        [Indexed]
        #endregion
        [Workspace]
        [Required]
        DateTime DateCreated { get; set; }

        #region Allors
        [Id("5ad0b9f5-669c-4b05-8c97-89b59a227da2")]
        [AssociationId("b3182870-cbe0-4da1-aaeb-804df5a9f869")]
        [RoleId("eacac26b-fea7-49f8-abb6-57d63accd548")]
        [Indexed]
        #endregion
        [Workspace]
        DateTime DateClosed { get; set; }

        #region Allors
        [Id("55375d57-34b0-43d0-9fac-e9788e1b6cd2")]
        [AssociationId("0d421578-35fc-4309-b8b6-cda0c9cf948c")]
        [RoleId("a7c8f58f-358a-4ae9-9299-0ef560190541")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        #endregion
        [Workspace]
        Person[] Participants { get; set; }

        #region Allors
        [Id("ea8abc59-b625-4d25-85bd-dd04bfe55086")]
        [AssociationId("90150444-fc18-47fd-a6fd-7740006e64ca")]
        [RoleId("34320d76-6803-4615-8444-cc3ea8bb0315")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        [Workspace]
        Person Performer { get; set; }

        #region Allors
        [Id("a280bf60-2eb7-488a-abf7-f03c9d9197b5")]
        [AssociationId("33be2d23-16d7-4739-8ef2-42391b0f4bd1")]
        [RoleId("9f88a8cf-84c1-42cc-be52-1d08597e56fa")]
        [Size(-1)]
        #endregion
        [Workspace]
        string Comment { get; set; }
    }
}