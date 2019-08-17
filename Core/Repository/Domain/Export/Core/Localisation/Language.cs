//-------------------------------------------------------------------------------------------------
// <copyright file="Language.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("4a0eca4b-281f-488d-9c7e-497de882c044")]
    #endregion
    public partial class Language : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("d2a32d9f-21cc-4f9d-b0d3-a9b75da66907")]
        [AssociationId("6c860e73-d12e-4e35-897e-ed9f8fd8eba0")]
        [RoleId("84f904a6-8dcc-4089-bda6-34325ade6367")]
        #endregion
        [Required]
        [Unique]
        [Size(256)]
        [Workspace]
        public string IsoCode { get; set; }

        #region Allors
        [Id("be482902-beb5-4a76-8ad0-c1b1c1c0e5c4")]
        [AssociationId("d3369fa9-afb7-4d5a-b476-3e4d43cce0fd")]
        [RoleId("308d73b0-1b65-40a9-88f1-288848849c51")]
        #endregion
        [Indexed]
        [Required]
        [Unique]
        [Size(256)]
        [Workspace]
        public string Name { get; set; }

        #region Allors
        [Id("f091b264-e6b1-4a57-bbfb-8225cbe8190c")]
        [AssociationId("6650af3b-f537-4c2f-afff-6773552315cd")]
        [RoleId("5e9fcced-727d-42a2-95e6-a0f9d8be4ec7")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public LocalisedText[] LocalisedNames { get; set; }

        #region Allors
        [Id("842CC899-3F37-455A-AE91-51D29D615E69")]
        [AssociationId("F3C7A07E-A2F3-4A96-91EF-D53DDF009DD4")]
        [RoleId("C78E1736-74B4-4574-A365-421DCADF4D4C")]
        #endregion
        [Indexed]
        [Required]
        // [Unique] If Unique is enabled then make sure your database supports the range of unicode characters (e.g. use collation 'Latin1_General_100_CI_AS_SC' in sql server)
        [Size(256)]
        [Workspace]
        public string NativeName { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}
