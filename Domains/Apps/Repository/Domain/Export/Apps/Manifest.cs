namespace Allors.Repository
{
  using System;

  using Attributes;

  #region Allors
  [Id("efb6f7a2-edec-40dd-a03a-d4e15abc438d")]
  #endregion
  public partial class Manifest : Document
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