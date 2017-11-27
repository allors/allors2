// Allors generated file. 
// Do not edit this file, changes will be overwritten.
namespace Allors.Domain
{
	public partial class Second : Allors.IObject , Object
	{
		private readonly IStrategy strategy;

		public Second(Allors.IStrategy strategy) 
		{
			this.strategy = strategy;
		}

		public Allors.Meta.MetaSecond Meta
		{
			get
			{
				return Allors.Meta.MetaSecond.Instance;
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

		public static Second Instantiate (Allors.ISession allorsSession, string allorsObjectId)
		{
			return (Second) allorsSession.Instantiate(allorsObjectId);		
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



		virtual public Third Third
		{ 
			get
			{
				return (Third) Strategy.GetCompositeRole(Meta.Third.RelationType);
			}
			set
			{
				Strategy.SetCompositeRole(Meta.Third.RelationType, value);
			}
		}

		virtual public bool ExistThird
		{
			get
			{
				return Strategy.ExistCompositeRole(Meta.Third.RelationType);
			}
		}

		virtual public void RemoveThird()
		{
			Strategy.RemoveCompositeRole(Meta.Third.RelationType);
		}


		virtual public global::System.Boolean? IsDerived 
		{
			get
			{
				return (global::System.Boolean?) Strategy.GetUnitRole(Meta.IsDerived.RelationType);
			}
			set
			{
				Strategy.SetUnitRole(Meta.IsDerived.RelationType, value);
			}
		}

		virtual public bool ExistIsDerived{
			get
			{
				return Strategy.ExistUnitRole(Meta.IsDerived.RelationType);
			}
		}

		virtual public void RemoveIsDerived()
		{
			Strategy.RemoveUnitRole(Meta.IsDerived.RelationType);
		}



		virtual public First FirstWhereSecond
		{ 
			get
			{
				return (First) Strategy.GetCompositeAssociation(Meta.FirstWhereSecond.RelationType);
			}
		} 

		virtual public bool ExistFirstWhereSecond
		{
			get
			{
				return Strategy.ExistCompositeAssociation(Meta.FirstWhereSecond.RelationType);
			}
		}



		public ObjectOnBuild OnBuild()
		{ 
			var method = new SecondOnBuild(this);
            method.Execute();
            return method;
		}

		public ObjectOnBuild OnBuild(System.Action<ObjectOnBuild> action)
		{ 
			var method = new SecondOnBuild(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnPostBuild OnPostBuild()
		{ 
			var method = new SecondOnPostBuild(this);
            method.Execute();
            return method;
		}

		public ObjectOnPostBuild OnPostBuild(System.Action<ObjectOnPostBuild> action)
		{ 
			var method = new SecondOnPostBuild(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnPreDerive OnPreDerive()
		{ 
			var method = new SecondOnPreDerive(this);
            method.Execute();
            return method;
		}

		public ObjectOnPreDerive OnPreDerive(System.Action<ObjectOnPreDerive> action)
		{ 
			var method = new SecondOnPreDerive(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnDerive OnDerive()
		{ 
			var method = new SecondOnDerive(this);
            method.Execute();
            return method;
		}

		public ObjectOnDerive OnDerive(System.Action<ObjectOnDerive> action)
		{ 
			var method = new SecondOnDerive(this);
            action(method);
            method.Execute();
            return method;
		}

		public ObjectOnPostDerive OnPostDerive()
		{ 
			var method = new SecondOnPostDerive(this);
            method.Execute();
            return method;
		}

		public ObjectOnPostDerive OnPostDerive(System.Action<ObjectOnPostDerive> action)
		{ 
			var method = new SecondOnPostDerive(this);
            action(method);
            method.Execute();
            return method;
		}
	}

	public partial class SecondBuilder : Allors.ObjectBuilder<Second> , ObjectBuilder, global::System.IDisposable
	{		
		public SecondBuilder(Allors.ISession session) : base(session)
	    {
	    }

		protected override void OnBuild(Second instance)
		{
			

			if(this.IsDerived.HasValue)
			{
				instance.IsDerived = this.IsDerived.Value;
			}			
		
		

			instance.Third = this.Third;

		
		}


				public Third Third {get; set;}

				/// <exclude/>
				public SecondBuilder WithThird(Third value)
		        {
		            if(this.Third!=null){throw new global::System.ArgumentException("One multicplicity");}
					this.Third = value;
		            return this;
		        }		

				
				public global::System.Boolean? IsDerived {get; set;}

				/// <exclude/>
				public SecondBuilder WithIsDerived(global::System.Boolean? value)
		        {
				    if(this.IsDerived!=null){throw new global::System.ArgumentException("One multicplicity");}
		            this.IsDerived = value;
		            return this;
		        }	


	}

	public partial class Seconds : global::Allors.ObjectsBase<Second>
	{
		public Seconds(Allors.ISession session) : base(session)
		{
		}

		public Allors.Meta.MetaSecond Meta
		{
			get
			{
				return Allors.Meta.MetaSecond.Instance;
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