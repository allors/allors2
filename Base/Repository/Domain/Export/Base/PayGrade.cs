namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("028de4a4-12d4-422f-8d82-4f1edaa471ae")]
    #endregion
    public partial class PayGrade : Commentable, Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        #endregion

        #region Allors
        [Id("88ba9ad4-e7de-42d9-89d7-9292d34d308b")]
        [AssociationId("36e42e9c-a623-493f-a29b-a34cdf485612")]
        [RoleId("64944205-252c-49d7-8a59-771b4a8a4318")]
        #endregion
        [Required]
        [Size(256)]

        public string Name { get; set; }
        #region Allors
        [Id("f7e52596-8814-48ff-a703-d80255110c5f")]
        [AssociationId("7ff0bc91-cc37-468d-b5ed-ae2de433acc8")]
        [RoleId("dc165e1f-88d2-4fb3-af0d-10d229f93528")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]

        public SalaryStep[] SalarySteps { get; set; }


        #region inherited methods


        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion

    }
}
