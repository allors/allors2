namespace Autotest.Html
{
    public class Expansion : Node
    {
        public string SwitchValue { get; set; }

        public ExpansionCase[] Cases { get; set; }
    }
}