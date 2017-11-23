namespace Allors.Meta
{
    public partial class MetaBinary : MetaUnit
	{
	    public static MetaBinary Instance { get; internal set;}

		public override Unit Unit { get; }

		internal MetaBinary(MetaPopulation metaPopulation)
        {
			this.Unit = new Unit(metaPopulation, UnitIds.Binary)
			{
				UnitTag = UnitTags.Binary,
				SingularName = UnitNames.Binary
			};
        }
	}

    public partial class MetaBoolean : MetaUnit
	{
	    public static MetaBoolean Instance { get; internal set;}

		public override Unit Unit { get; }

		internal MetaBoolean(MetaPopulation metaPopulation)
        {
			this.Unit = new Unit(metaPopulation, UnitIds.Boolean)
			{
				UnitTag = UnitTags.Boolean,
				SingularName = UnitNames.Boolean
			};
        }
	}

    public partial class MetaDateTime : MetaUnit
	{
	    public static MetaDateTime Instance { get; internal set;}

		public override Unit Unit { get; }

		internal MetaDateTime(MetaPopulation metaPopulation)
        {
			this.Unit = new Unit(metaPopulation, UnitIds.DateTime)
			{
				UnitTag = UnitTags.DateTime,
				SingularName = UnitNames.DateTime
			};
        }
	}

    public partial class MetaDecimal : MetaUnit
	{
	    public static MetaDecimal Instance { get; internal set;}

		public override Unit Unit { get; }

		internal MetaDecimal(MetaPopulation metaPopulation)
        {
			this.Unit = new Unit(metaPopulation, UnitIds.Decimal)
			{
				UnitTag = UnitTags.Decimal,
				SingularName = UnitNames.Decimal
			};
        }
	}

    public partial class MetaFloat : MetaUnit
	{
	    public static MetaFloat Instance { get; internal set;}

		public override Unit Unit { get; }

		internal MetaFloat(MetaPopulation metaPopulation)
        {
			this.Unit = new Unit(metaPopulation, UnitIds.Float)
			{
				UnitTag = UnitTags.Float,
				SingularName = UnitNames.Float
			};
        }
	}

    public partial class MetaInteger : MetaUnit
	{
	    public static MetaInteger Instance { get; internal set;}

		public override Unit Unit { get; }

		internal MetaInteger(MetaPopulation metaPopulation)
        {
			this.Unit = new Unit(metaPopulation, UnitIds.Integer)
			{
				UnitTag = UnitTags.Integer,
				SingularName = UnitNames.Integer
			};
        }
	}

    public partial class MetaString : MetaUnit
	{
	    public static MetaString Instance { get; internal set;}

		public override Unit Unit { get; }

		internal MetaString(MetaPopulation metaPopulation)
        {
			this.Unit = new Unit(metaPopulation, UnitIds.String)
			{
				UnitTag = UnitTags.String,
				SingularName = UnitNames.String
			};
        }
	}

    public partial class MetaUnique : MetaUnit
	{
	    public static MetaUnique Instance { get; internal set;}

		public override Unit Unit { get; }

		internal MetaUnique(MetaPopulation metaPopulation)
        {
			this.Unit = new Unit(metaPopulation, UnitIds.Unique)
			{
				UnitTag = UnitTags.Unique,
				SingularName = UnitNames.Unique
			};
        }
	}
}