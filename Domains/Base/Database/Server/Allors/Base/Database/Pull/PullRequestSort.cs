namespace Allors.Server
{
    using System;

    using Allors.Meta;

    public class PullRequestSort
    {
        /// <summary>
        /// The RoleType.
        /// </summary>
        public string RT { get; set; }

        /// <summary>
        /// The TreeNodes
        /// </summary>
        public string D { get; set; }

        public Sort Parse(MetaPopulation metaPopulation)
        {
            var roleType = (RoleType)metaPopulation.Find(new Guid(this.RT));

            return new Sort
                       {
                           RoleType = roleType,
                           Direction = "Desc".Equals(this.D) ? SortDirection.Descending : SortDirection.Ascending
                       };
        }
    }
}