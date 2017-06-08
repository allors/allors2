namespace Allors.Server
{
    using System;

    using Allors.Meta;

    public class PullRequestPath
    {
        /// <summary>
        /// The RoleType.
        /// </summary>
        public string Step { get; set; }

        /// <summary>
        /// The TreeNodes
        /// </summary>
        public PullRequestPath Next { get; set; }

        public void Parse(Path path, IMetaPopulation metaPopulation)
        {
            var propertyType = (PropertyType)metaPopulation.Find(new Guid(this.Step));
            path.PropertyType = propertyType;

            if (this.Next != null)
            {
                path.Next = new Path();
                this.Next.Parse(path.Next, metaPopulation);
            }
        }
    }
}