namespace Allors.Workspace.Data
{
    using System.Collections.Generic;

    public class PushRequestRole
    {
        public string t { get; set; }
        public object s { get; set; }
        public string[] a { get; set; }
        public string[] r { get; set; }
    }

    public class PushRequestObject
    {
        public string i { get; set; }
        public string v { get; set; }

        public PushRequestRole[] roles { get; set; }
    }

    public class PushRequestNewObject
    {
        public string ni { get; set; }
        public string t { get; set; }
        public PushRequestRole[] roles { get; set; }
    }

    public class PushRequest
    {
        public List<PushRequestNewObject> newObjects { get; set; }
        public List<PushRequestObject> objects { get; set; }
    }
}