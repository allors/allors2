namespace Allors.Repository
{
  using Attributes;

  #region Allors
  [Id("1fb8d537-a870-4793-95a1-7742749e16fc")]
  #endregion
  public partial class ProductDrawing : Document
  {
    #region inherited properties
    public string Name { get; set; }

    public string Description { get; set; }

    public string Text { get; set; }

    public string DocumentLocation { get; set; }

    public Media PrintDocument { get; set; }

      public Permission[] DeniedPermissions { get; set; }

    public SecurityToken[] SecurityTokens { get; set; }

    public string Comment { get; set; }

      public LocalisedText[] LocalisedComments { get; set; }

      #endregion

    #region inherited methods


    public void OnBuild() { }

    public void OnPostBuild() { }

    public void OnPreDerive() { }

    public void OnDerive() { }

    public void OnPostDerive() { }





    #endregion
  }
}