namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("A56696EB-3806-40F3-AFE9-5067100A1F00")]
    #endregion
    public partial interface IPhoneCommunication : ICommunicationEvent 
    {
        #region Allors
        [Id("50a8225e-7094-4572-8074-a5df4a50b0bd")]
        [AssociationId("5fb6405b-2d06-425d-9e42-cb6638a2e308")]
        [RoleId("209e3d12-b5cf-49c9-a39c-15f14690ec69")]
        #endregion
        [Workspace]
        bool LeftVoiceMail { get; set; }

        #region Allors
        [Id("53df1269-a6f0-49a4-bd2f-af4aff75565a")]
        [AssociationId("32e719bd-39c7-4fc3-bff2-e0091cebd0b7")]
        [RoleId("5bbb6e8a-7c82-492e-b497-3579007f9294")]
        #endregion
        [Required]
        [Workspace]
        bool IncomingCall { get; set; }

        #region Allors
        [Id("5e3c675b-b329-47a4-9d53-b0e95837a23b")]
        [AssociationId("16fa813c-15d6-4bfb-a7b3-c295efe47a1c")]
        [RoleId("f9320b55-230d-4f10-9a1b-6960137326b7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Required]
        [Workspace]
        Party[] Receivers { get; set; }

        #region Allors
        [Id("7a37ab85-222a-4d13-b832-b222faefcf39")]
        [AssociationId("79c04646-6f62-4867-9f89-f2ce1876e981")]
        [RoleId("507e6ff3-3baa-4c77-b41b-4d1893443dc2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Required]
        [Workspace]
        Party[] Callers { get; set; }
    }
}