using Autotest.Typescript;

namespace Autotest.Angular
{
    public class Directive
    {
        public Reference Reference { get; set; }
        
        public bool IsComponent { get; set; }
        public string Selector { get; set; }
        public Template Template { get; set; }
        public Class Type { get; set; }
    }
}