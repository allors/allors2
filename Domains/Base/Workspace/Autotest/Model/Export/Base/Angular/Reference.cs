namespace Autotest.Angular
{
    using Newtonsoft.Json.Linq;

    public class Reference
    {
        public Reference(JToken json)
        {
            this.Name = json["name"]?.Value<string>();
            this.Path = json["path"]?.Value<string>();

            this.Id = $"{this.Name} | {this.Path}";
        }

        public string Id { get; }

        public string Name { get; }

        public string Path { get; }
    }
}