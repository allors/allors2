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
        /// The path
        /// </summary>
        public PullRequestPath Path { get; set; }

        /// <summary>
        /// The additional objects to include from this relations.
        /// </summary>
        public PullRequestTreeNode[] Include { get; set; }

        public void Parse<T>(ISession session, out T @object, out Path path, out Tree include)
            where T : IObject
        {
            @object = (T)session.Instantiate(this.Id);

            path = null;
            if (this.Path != null)
            {
                path = new Path();
                this.Path.Parse(path, session.Database.MetaPopulation);
            }

            include = null;
            if (this.Include != null)
            {
                if (path == null)
                {
                    var composite = @object.Strategy.Class;
                    include = new Tree(composite);
                }
                else
                {
                    include = new Tree((IComposite)path.End.GetObjectType());
                }

                var metaPopulation = (MetaPopulation)@object.Strategy.Session.Database.MetaPopulation;
                foreach (var i in this.Include)
                {
                    i.Parse(metaPopulation, out TreeNode treeNode);
                    include.Nodes.Add(treeNode);
                }
            }            
        }
    }
}