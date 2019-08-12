namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("4a4a0548-b75f-4a79-89aa-f5c242121f11")]
    #endregion
    [Plural("ServiceEntries")]
    public partial interface ServiceEntry : Commentable, Period, Deletable, Object
    {
        #region Allors
        [Id("74fc8f9b-62f3-4921-bce1-ca10eed33204")]
        [AssociationId("987c6fb3-b512-4797-933d-28424500649e")]
        [RoleId("1bbf98fb-fb84-45e7-b3f3-c6d5bb9b155c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        EngagementItem EngagementItem { get; set; }

        #region Allors
        [Id("9b04b715-376f-4c39-b78b-f92af6b4ffc1")]
        [AssociationId("2c25dc8f-c253-471e-87fb-fe6934cf2b15")]
        [RoleId("b80138a0-0a0b-4a3a-8fbb-5bca2dc8c84c")]
        #endregion
        [Required]
        [Workspace]
        bool IsBillable { get; set; }

        #region Allors
        [Id("a6ae42bd-babf-44e1-bdc0-cc403e56e43e")]
        [AssociationId("47acb5ae-b805-494e-9a44-10e2ddccec80")]
        [RoleId("04df18b1-b92d-437d-a666-852c85e64330")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("b9bb6409-c6b9-4a4b-9d46-02c62b4b3304")]
        [AssociationId("c4b7a55c-d0d9-429f-9577-d32de5b6f0cd")]
        [RoleId("f624973f-1a6a-4cd6-930f-ecfb4d3772ec")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        WorkEffort WorkEffort { get; set; }
    }
}