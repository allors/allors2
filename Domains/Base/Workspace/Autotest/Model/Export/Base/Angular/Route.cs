using Newtonsoft.Json.Linq;

namespace Autotest.Angular
{
    public class Route
    {
    public string Path { get; set; }
    public string PathMatch{ get; set; }
    public Directive Component{ get; set; }
    public string RedirectTo{ get; set; }
    public string Outlet{ get; set; }
    public JToken Data{ get; set; }
    public Route[] Children{ get; set; }
}
}