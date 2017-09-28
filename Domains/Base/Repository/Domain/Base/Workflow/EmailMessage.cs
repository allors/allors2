//------------------------------------------------------------------------------------------------- 
// <copyright file="EmailMessage.cs" company="Allors bvba">
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
    [Id("ab20998b-62b1-4064-a7b9-cc9416edf77a")]
    #endregion
    public partial class EmailMessage : Object 
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("5de25d18-3c36-418f-9c85-55a480d58bc5")]
        [AssociationId("1b4eb236-b359-40ff-ba67-2e1623844f78")]
        [RoleId("57dc2c2a-949b-497b-880f-b1df13e0e12f")]
        [Indexed]
        #endregion
        [Derived]
        [Required]
        public DateTime DateCreated { get; set; }

        #region Allors
        [Id("c297ff40-e2ad-46af-94fc-c61af6e6a6d6")]
        [AssociationId("366767a9-d82d-408d-9c06-7256724aa578")]
        [RoleId("29b77e2c-9590-4da9-a616-f67e84187644")]
        [Indexed]
        #endregion
        public DateTime DateSending { get; set; }

        #region Allors
        [Id("cc36e90a-dcda-4289-b84f-c947c97847b0")]
        [AssociationId("9b3d2505-103a-4801-9f16-f1f7ca924f57")]
        [RoleId("ae7bedca-c966-4cd5-9a8a-b99f3fc5e0bc")]
        [Indexed]
        #endregion
        public DateTime DateSent { get; set; }

        #region Allors
        [Id("e16da480-35ab-4383-940a-5298d0b33b9c")]
        [AssociationId("5be8bb0f-cead-44f6-813b-1125882618b7")]
        [RoleId("4cca6d37-fffe-4e78-962c-f4474551e09e")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public User Sender { get; set; }
        
        #region Allors
        [Id("d115bcfb-55e5-4ed8-8a21-f8e4dd5f903d")]
        [AssociationId("55c3f9b5-1a80-419d-93cc-6c19925e350e")]
        [RoleId("8e8749da-4411-4dfa-bd78-856f37e1a4ba")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Required]
        public User[] Recipients { get; set; }

        #region Allors
        [Id("CD9C9D1E-3393-46B4-AD61-7AC03019EE08")]
        [AssociationId("EC809FF4-98BB-4DFA-9D18-1D321A2BC871")]
        [RoleId("6846A2B4-DFC4-436E-81E2-C504DD020546")]
        #endregion
        [Indexed]
        [Size(256)]
        public string RecipientEmailAddress { get; set; }

        #region Allors
        [Id("5666ebec-8205-4e5f-b0df-cacfa1af99ce")]
        [AssociationId("1adc0465-9b6b-4050-9b0a-e7fe441ccbd5")]
        [RoleId("f19705f3-5323-4360-8602-acee1be80c50")]
        #endregion
        [Size(1024)]
        [Required]
        public string Subject { get; set; }

        #region Allors
        [Id("25be1f1c-ea8b-471e-ad09-b618927dc15a")]
        [AssociationId("0b9ec5be-fe85-407c-8a35-434ede55bd3b")]
        [RoleId("b331b4dd-7bfa-479d-91f2-9376955207ef")]
        #endregion
        [Size(-1)]
        [Required]
        public string Body { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}
        #endregion
    }
}