// <copyright file="TimeSheet.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("02E8DA1B-0551-411E-BDFA-52F053EC7D4A")]
    #endregion
    public partial class TimeSheet : Deletable, DelegatedAccessControlledObject
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }
        #endregion

        /// <summary>
        /// Gets or sets the Person for whom this TimeSheet records TimeEntries.
        /// </summary>
        #region Allors
        [Id("6623E301-C36F-4F57-A5B7-B52A3C10E846")]
        [AssociationId("D39519EB-4AEF-4525-9BD4-2ABA38A30989")]
        [RoleId("B14E5FAC-B660-419C-B75B-866E140F1F50")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Person Worker { get; set; }

        #region Allors
        [Id("7A05E667-4A9F-45DF-BAC9-6A4C9F5CC885")]
        [AssociationId("F7340A0B-E03D-4F1A-9FE6-275649B76DA6")]
        [RoleId("F0735226-8665-4F8A-8ECB-7BA8B406DC32")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public TimeEntry[] TimeEntries { get; set; }

        #region inherited methods
        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        public void DelegateAccess() { }

        public void Delete() { }
        #endregion
    }
}
