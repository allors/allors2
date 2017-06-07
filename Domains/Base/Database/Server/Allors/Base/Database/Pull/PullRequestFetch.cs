namespace Allors.Server
{
    using Allors.Meta;

    public class PullRequestFetch
    {
        /// <summary>
        /// The name of the fetch
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// The id of the object
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The additional objects to include from this relations.
        /// </summary>
        public PullRequestTreeNode[] Include { get; set; }

        public void Parse<T>(ISession session, out T @object, out Tree include)
            where T : IObject
        {
            @object = (T)session.Instantiate(this.Id);
            var composite = @object.Strategy.Class;
            include = new Tree(composite);
            if (this.Include != null)
            {
                foreach (var treeNode in this.Include)
                {
                    treeNode.Parse(include);
                }
            }
            
        }
    }
}