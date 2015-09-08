namespace Allors.Meta
{
    public partial class MetaPopulation
    {
        private void Extend()
        {
            foreach (var composite in this.Composites)
            {
                composite.BaseExtend();
            }

            foreach (var composite in this.Composites)
            {
                composite.AppsExtend();
            }
        }
    }
}