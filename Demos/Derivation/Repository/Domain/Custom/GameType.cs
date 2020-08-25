using Allors.Repository.Attributes;
using System;

namespace Allors.Repository
{
    #region Allors
    [Id("23207fbd-0a09-45db-94e8-d57a9cf84e3a")]
    #endregion

    public class GameType : Enumeration
    {
        #region inherited properties
        public string Name { get; set; }
        public LocalisedText[] LocalisedNames { get; set; }
        public bool IsActive { get; set; }
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public Guid UniqueId { get; set; }
        #endregion

        #region inherited methods
        public void OnBuild()
        {

        }

        public void OnDerive()
        {

        }

        public void OnInit()
        {

        }

        public void OnPostBuild()
        {

        }

        public void OnPostDerive()
        {

        }

        public void OnPreDerive()
        {

        }

        #endregion inherited methods
    }
}