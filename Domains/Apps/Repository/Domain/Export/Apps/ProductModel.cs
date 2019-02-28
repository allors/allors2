namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("99ea8125-7d86-4cb6-b453-27752c434fc7")]
    #endregion
    public partial class ProductModel : Document
    {
        #region inherited properties
        public string Name { get; set; }

        public string Description { get; set; }

        public string Text { get; set; }

        public string DocumentLocation { get; set; }

        public PrintDocument PrintDocument { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        #endregion

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
            throw new System.NotImplementedException();
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Print() { }
        #endregion
    }
}