namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("93e3b3df-b227-479a-9b05-ec10190e7d51")]
    #endregion
    public partial class HazardousMaterialsDocument : Document
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

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Print() { }

        #endregion
    }
}