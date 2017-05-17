namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("d89d0bd1-2670-4168-bf8c-41f5a71bd225")]
    #endregion
    public partial interface EmailSource :  Object 
    {
        #region Allors
        [Id("00523f74-d16b-4355-8b37-6917072c75b9")]
        [AssociationId("4dcdedea-647b-454f-955c-3973be50270f")]
        [RoleId("0960e5de-a50d-41b0-8979-0adbf49652d2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        EmailMessage EmailMessage { get; set; }
    }
}