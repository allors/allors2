// Allors generated file. 
// Do not edit this file, changes will be overwritten.
namespace Allors.Domain
{
	public partial class Extender : Allors.IObject , Object
	{
		private readonly IStrategy strategy;

		public Extender(Allors.IStrategy strategy) 
		{
			this.strategy = strategy;
		}

		public Allors.Meta.MetaExtender Meta
		{
			get
			{
				return Allors.Meta.MetaExtender.Instance;
			}
		}

		public long Id
		{
			get { return this.strategy.ObjectId; }
		}

		public IStrategy Strategy
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this.strategy; }
        }

		public static Extender Instantiate (Allors.ISession allorsSession, string allorsObjectId)
		{
			return (Extender) allorsSession.Instantiate(allorsObjectId);		
		}

		public override bool Equals(object obj)
        {
            var typedObj = obj as IObject;
            return typedObj != null &&
                   typedObj.Strategy.ObjectId.Equals(this.Strategy.ObjectId) &&
                   typedObj.Strategy.Session.Database.Id.Equals(this.Strategy.Session.Database.Id);
        }

		public override int GetHashCode()
        {
            return this.Strategy.ObjectId.GetHashCode();
        }



		virtual public global::System.String AllorsString 
		{
			get
			{
				return (global::System.String) Strategy.GetUnitRole(Meta.AllorsString.RelationType);
			}
			set
			{
				Strategy.SetUnitRole(Meta.AllorsString.RelationType, value);
			}
		}

		virtual public bool ExistAllorsString{
			get
			{
				return Strategy.ExistUnitRole(Meta.AllorsString.RelationType);
			}
		}

		virtual public void RemoveAllorsString()
		{
			Strategy.RemoveUnitRole(Meta.AllorsString.RelationType);
		}



		public ObjectOnBuild OnBuild()
		{ 
			var method = new ExtenderOnBuild(this);
            method.Execute();
            return method;
		}

		public ObjectOnBuild OnBuild(System.Action<ObjectOnBuild> action)
		{ 
			var method = new ExtenderOnBuild(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnPostBuild OnPostBuild()
		{ 
			var method = new ExtenderOnPostBuild(this);
            method.Execute();
            return method;
		}

		public ObjectOnPostBuild OnPostBuild(System.Action<ObjectOnPostBuild> action)
		{ 
			var method = new ExtenderOnPostBuild(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnPreDerive OnPreDerive()
		{ 
			var method = new ExtenderOnPreDerive(this);
            method.Execute();
            return method;
		}

		public ObjectOnPreDerive OnPreDerive(System.Action<ObjectOnPreDerive> action)
		{ 
			var method = new ExtenderOnPreDerive(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnDerive OnDerive()
		{ 
			var method = new ExtenderOnDerive(this);
            method.Execute();
            return method;
		}

		public ObjectOnDerive OnDerive(System.Action<ObjectOnDerive> action)
		{ 
			var method = new ExtenderOnDerive(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnPostDerive OnPostDerive()
		{ 
			var method = new ExtenderOnPostDerive(this);
            method.Execute();
            return method;
		}

		public ObjectOnPostDerive OnPostDerive(System.Action<ObjectOnPostDerive> action)
		{ 
			var method = new ExtenderOnPostDerive(this);
            action(method);
            method.Execute();
            return method;
		}
	}

	public partial class ExtenderBuilder : Allors.ObjectBuilder<Extender> , ObjectBuilder, global::System.IDisposable
	{		
		public ExtenderBuilder(Allors.ISession session) : base(session)
	    {
	    }

		protected override void OnBuild(Extender instance)
		{

			instance.AllorsString = this.AllorsString;
		
		
		}


				public global::System.String AllorsString {get; set;}

				/// <exclude/>
				public ExtenderBuilder WithAllorsString(global::System.String value)
		        {
				    if(this.AllorsString!=null){throw new global::System.ArgumentException("One multicplicity");}
		            this.AllorsString = value;
		            return this;
		        }	


	}

	public partial class Extenders : global::Allors.ObjectsBase<Extender>
	{
		public Extenders(Allors.ISession session) : base(session)
		{
		}

		public Allors.Meta.MetaExtender Meta
		{
			get
			{
				return Allors.Meta.MetaExtender.Instance;
			}
		}

		public override Allors.Meta.Composite ObjectType
		{
			get
			{
				return Meta.ObjectType;
			}
		}
	}

}