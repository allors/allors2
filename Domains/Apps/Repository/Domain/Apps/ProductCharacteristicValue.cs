namespace Allors.Repository
{
  using System;

  using Attributes;

  #region Allors
  [Id("C3A647C2-1073-4D8B-99EB-AE5293AADB6B")]
  #endregion
  [Synced]
  public partial class ProductCharacteristicValue : AccessControlledObject, Deletable, Localised
  {
    #region inherited properties

    public Locale Locale { get; set; }

    public Permission[] DeniedPermissions { get; set; }

    public SecurityToken[] SecurityTokens { get; set; }

    #endregion

    #region Allors
    [Id("73A04D99-CD9F-41F7-AA1C-B4CD80AF60AD")]
    [AssociationId("61DC8595-D02C-485C-9EC8-09470B33EF9E")]
    [RoleId("216DA9A4-BF58-44E9-B315-A9ED222122C8")]
    #endregion
    [Multiplicity(Multiplicity.ManyToOne)]
    [Required]
    [Workspace]
    [Synced]
    public ProductCharacteristic ProductCharacteristic { get; set; }

    #region Allors
    [Id("E68E6931-F10C-4F04-A23E-B2BC82AC6D5C")]
    [AssociationId("3757E6C2-789B-4711-A366-F018212A2109")]
    [RoleId("0BC3A4E1-C3EB-4312-A699-D28EB778EA05")]
    #endregion
    [Multiplicity(Multiplicity.OneToOne)]
    [Size(-1)]
    [Workspace]
    public string AssignedValue { get; set; }

    #region Allors
    [Id("0675493A-FB2F-4EBC-9C6C-DCF29F2C79A3")]
    [AssociationId("419E2294-5E95-47A8-9183-A1BD676F29F3")]
    [RoleId("EE4767C5-1315-4201-9104-3F214024A1AC")]
        #endregion
    [Derived]
    [Size(-1)]
    [Workspace]
    public string DerivedValue { get; set; }

    #region inherited methods

    public void OnBuild()
    {
      throw new NotImplementedException();
    }

    public void OnPostBuild()
    {
      throw new NotImplementedException();
    }

    public void OnPreDerive()
    {
      throw new NotImplementedException();
    }

    public void OnDerive()
    {
      throw new NotImplementedException();
    }

    public void OnPostDerive()
    {
      throw new NotImplementedException();
    }

    public void Delete()
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}