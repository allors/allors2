using Newtonsoft.Json.Linq;

namespace Autotest.Angular
{
    public class Provider
    {
        public Reference TokenIdentifier { get; set; }
        public string TokenValue { get; set; }

        public Reference UseClass{ get; set; }
        public JToken UseExisting{ get; set; }
        public Reference UseFactory{ get; set; }
        public bool Multi { get; set; }
    }
}