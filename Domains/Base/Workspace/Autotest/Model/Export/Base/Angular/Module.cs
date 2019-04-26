namespace Autotest.Angular
{
    public class Module
    {
        public Project Project { get; set; }

        public Reference Reference { get; set; }
        
        public Directive[] BootstrapComponents{ get; set; }
        public Directive[] DeclaredDirectives{ get; set; }
        public Directive[] ExportedDirectives{ get; set; }
        
        public Pipe[] ExportedPipes{ get; set; }
        public Pipe[] DeclaredPipes{ get; set; }
        
        public Module[] ImportedModules{ get; set; }
        public Module[] ExportedModules{ get; set; }
        
        public Route[] Routes{ get; set; }
    }
}