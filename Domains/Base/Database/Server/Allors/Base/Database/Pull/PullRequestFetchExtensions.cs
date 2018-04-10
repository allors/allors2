namespace Allors.Server
{
    using Allors.Meta;

    public static class PullRequestFetchExtensions
    {
        public static void Parse<T>(this PullRequestFetch @this, ISession session, out T @object, out Path path, out Tree include)
            where T : IObject
        {
            @object = (T)session.Instantiate(@this.Id);

            path = null;
            if (@this.Path != null)
            {
                path = new Path();
                @this.Path.Parse(path, session.Database.MetaPopulation);
            }

            include = null;
            if (@this.Include != null && @object != null)
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
                foreach (var i in @this.Include)
                {
                    i.Parse(metaPopulation, out TreeNode treeNode);
                    include.Nodes.Add(treeNode);
                }
            }            
        }
    }
}