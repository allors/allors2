namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("fdd52472-e863-4e91-bb01-1dada2acc8f6")]
    #endregion
    public partial interface Commentable : Object 
    {
        #region Allors
        [Id("d800f9a2-fadd-45f1-8731-4dac177c6b1b")]
        [AssociationId("d3aadbc5-e488-4346-ac34-9e14cb8d2350")]
        [RoleId("8b41d441-cd12-49d0-823c-b8a3163baadc")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Comment { get; set; }

        #region Allors
        [Id("88CDC491-70DB-4B5C-B628-B18806D1FDBD")]
        [AssociationId("396F14D8-7FA5-4E0C-A58B-E3988B733367")]
        [RoleId("16562E16-4FE1-4437-8BEF-FB8909C44480")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        LocalisedText[] LocalisedComments { get; set; }
    }
}