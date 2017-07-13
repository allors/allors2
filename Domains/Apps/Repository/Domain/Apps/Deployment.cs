namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("ee23df25-f7d7-4974-b62e-ee3cba56b709")]
    #endregion
    public partial class Deployment : AccessControlledObject, Period 
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        #endregion

        #region Allors
        [Id("212653db-1677-47bd-944c-b5468673ec63")]
        [AssociationId("7543cf10-97dd-4823-b386-f06379e398b2")]
        [RoleId("685a54f0-4e66-4ce3-93a2-f6f45dcf8c8b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Good ProductOffering { get; set; }
        #region Allors
        [Id("c322fbbd-3406-4e73-83ed-033282ab0cfb")]
        [AssociationId("d265b170-3854-4276-9a20-325984097991")]
        [RoleId("501b64c8-4181-45ca-a4f3-075232c8b270")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public DeploymentUsage DeploymentUsage { get; set; }
        #region Allors
        [Id("d588ba7f-7b67-43fd-bb67-b9ff82fcffaf")]
        [AssociationId("bbee5696-6e53-4ea3-8f57-4e018e6bc61d")]
        [RoleId("33c8e0e5-be98-44bb-a9eb-cfbabd8451b2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public SerializedInventoryItem SerializedInventoryItem { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion

    }
}