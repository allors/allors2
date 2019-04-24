namespace Autocomplete
{
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public partial class Model
    {
        public void LoadMenu(FileInfo fileInfo)
        {
            using (var file = File.OpenText(fileInfo.FullName))
            using (var reader = new JsonTextReader(file))
            {
                var jsonArray = (JArray)JToken.ReadFrom(reader);

                this.Menu = jsonArray?.Cast<JObject>()
                    .Select(v =>
                        {
                            var child = new MenuItem();
                            child.Load(v);
                            return child;
                        }).ToArray();
            }
        }
    }
}
