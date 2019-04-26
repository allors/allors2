namespace Autotest.Angular
{
    using Newtonsoft.Json.Linq;

    public class Pipe
    {
        public Project Project { get; set; }

        public JToken Json { get; set; }

        public Reference Reference { get; set; }

        public void BaseLoad()
        {
        }
    }
}