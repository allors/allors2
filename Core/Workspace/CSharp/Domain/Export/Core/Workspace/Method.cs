namespace Allors.Workspace
{
    public class Method
    {
        public Method(SessionObject @object, string name)
        {
            this.Object = @object;
            this.Name = name;
        }

        public SessionObject Object { get; }

        public string Name { get; }
    }
}
