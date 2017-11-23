namespace Allors.Workspace.Meta
{
	using Allors.Meta;

    public partial class MetaCustom
    {
        public static MetaCustom Instance { get; internal set;}

		public readonly Domain Domain;

        internal MetaCustom(MetaPopulation metaPopulation)
        {
			this.Domain = new Domain(metaPopulation, new System.Guid("af96e2b7-3bb5-4cd1-b02c-39a67c99a11a"))
			{
				Name = "Custom"
			};
        }
    }

    public partial class MetaBase
    {
        public static MetaBase Instance { get; internal set;}

		public readonly Domain Domain;

        internal MetaBase(MetaPopulation metaPopulation)
        {
			this.Domain = new Domain(metaPopulation, new System.Guid("770538dd-7b19-4694-bdce-cf04dcf9cf62"))
			{
				Name = "Base"
			};
        }
    }

    public partial class MetaCore
    {
        public static MetaCore Instance { get; internal set;}

		public readonly Domain Domain;

        internal MetaCore(MetaPopulation metaPopulation)
        {
			this.Domain = new Domain(metaPopulation, new System.Guid("fc9bad0e-d70d-4f93-bca7-0c7944ef1abb"))
			{
				Name = "Core"
			};
        }
    }
}