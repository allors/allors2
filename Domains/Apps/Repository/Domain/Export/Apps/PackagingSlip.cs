namespace Allors.Repository
{
  using System;

  using Attributes;

  #region Allors
  [Id("66e7dcf3-90bc-4ac6-988f-54015f5bef11")]
  #endregion
  public partial class PackagingSlip : Document
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