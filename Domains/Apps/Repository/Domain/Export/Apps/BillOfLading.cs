namespace Allors.Repository
{
  using System;

  using Attributes;

  #region Allors
  [Id("5c5c17d1-2132-403b-8819-e3c1aa7bd6a9")]
  #endregion
  public partial class BillOfLading : Document
  {
    #region inherited properties
    public string Name { get; set; }

    public string Description { get; set; }

    public string Text { get; set; }

    public string DocumentLocation { get; set; }

    public string HtmlContent { get; set; }

      public Permission[] DeniedPermissions { get; set; }

    public SecurityToken[] SecurityTokens { get; set; }

    public string Comment { get; set; }

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