namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("4629c7ed-e9a4-4f31-bb46-e3f2920bd768")]
    #endregion
    public partial class PerformanceNote : AccessControlledObject, Commentable 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        #endregion

        #region Allors
        [Id("1b8f0ada-bb5c-4226-8e35-5f1c40b06fc8")]
        [AssociationId("e4ae1691-22f8-4304-8e04-73ae41420b43")]
        [RoleId("1d396f6f-279d-4b83-9d95-6ece6089f6a0")]
        #endregion
        [Required]
        [Size(-1)]

        public string Description { get; set; }
        #region Allors
        [Id("2f6ed687-4200-4a27-bfb2-922d9ce2e38f")]
        [AssociationId("5f2b047e-2cb0-4d2a-9cce-77846ad35f45")]
        [RoleId("f21bbf2d-0780-4bbf-92e6-2c6676b4893d")]
        #endregion

        public DateTime CommunicationDate { get; set; }
        #region Allors
        [Id("5bf234d2-8486-47b2-a770-eca36b44bb67")]
        [AssociationId("cc9f9a6f-54fc-4786-9d83-2769d8d921ce")]
        [RoleId("0467f9fa-17e9-4fdc-b74a-39d074e55b16")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Person GivenByManager { get; set; }
        #region Allors
        [Id("a8cd7bf6-6bea-44ad-9e89-1bd63ffca459")]
        [AssociationId("c4a4e475-613b-4e38-bb79-b5bd12f73332")]
        [RoleId("06b721ea-20ec-4b18-bd5c-d6d3e86610bd")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Person Employee { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion

    }
}