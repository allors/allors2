// <copyright file="AccessClass.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    /// <summary>
    /// I have some tags.
    /// </summary>
    [Id("47C94C46-6F4E-4715-9E4F-28D7EBA88D9B")]
    #endregion
    [Tags("TagA", "TagB", "TagI", "TagX")]
    public partial class Tagged : TaggedInterface
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }
        public bool TagInterface { get; set; }

        #endregion

        #region Allors
        [Id("C7F199F1-CAE3-4C28-A7D1-746BF3BCE6B4")]
        [AssociationId("9A547B58-2538-4A66-83F6-DA45986675DD")]
        [RoleId("23AAC745-4C05-478F-8B52-29089325B7CE")]
        #endregion
        [Tags("TagA")]
        public bool SingleTag { get; set; }

        #region Allors
        [Id("7D0DAE8F-10E2-407D-9C03-90E20C9033B7")]
        [AssociationId("E3631340-66CA-4D0D-8867-EA1578C10E2D")]
        [RoleId("27875131-2978-4E31-A43E-C0D037C8AEB3")]
        #endregion
        [Tags("TagA", "TagB")]
        public bool MultipleTags{ get; set; }
        
        #region Allors

        [Id("B42A9BBF-EEA0-4352-A71D-C70ADA0735A6")]

        #endregion

        [Tags("TagX")]
        public void SingleTagMethod() { }

        #region Allors

        [Id("06485591-EC88-44A2-BFC1-62C00AF9A479")]
        [Tags("TagX", "TagY")]

        #endregion

        public void MultipleTagMethod() { }


        #region inherited methods

        public void OnBuild()
        {
        }

        public void OnPostBuild()
        {
        }

        public void OnInit()
        {
        }

        public void OnPreDerive()
        {
        }

        public void OnDerive()
        {
        }

        public void OnPostDerive()
        {
        }

        #endregion
    }
}
