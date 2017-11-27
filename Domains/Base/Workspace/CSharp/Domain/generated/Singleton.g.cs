// Allors generated file. 
// Do not edit this file, changes will be overwritten.
namespace Allors.Workspace.Domain
{
	public partial class Singleton : SessionObject 
	{
		public Singleton(Session session)
		: base(session)
		{
		}

		public Allors.Workspace.Meta.MetaSingleton Meta
		{
			get
			{
				return Allors.Workspace.Meta.MetaSingleton.Instance;
			}
		}

		public static Singleton Instantiate (Session allorsSession, long allorsObjectId)
		{
			return (Singleton) allorsSession.Get(allorsObjectId);		
		}

		public override bool Equals(object obj)
        {
            var that = obj as SessionObject;
		    if (that == null)
		    {
		        return false;
		    }

		    return this.Id.Equals(that.Id);
        }

		public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }


		public bool CanReadDefaultLocale 
		{
			get
			{
				return this.CanRead(this.Meta.DefaultLocale);
			}
		}

		public bool CanWriteDefaultLocale 
		{
			get
			{
				return this.CanWrite(this.Meta.DefaultLocale);
			}
		}



		virtual public Locale DefaultLocale
		{ 
			get
			{
				return (Locale) this.Get(Meta.DefaultLocale);
			}
			set
			{
				this.Set(Meta.DefaultLocale, value);
			}
		}

		virtual public bool ExistDefaultLocale
		{
			get
			{
				return this.Exist(Meta.DefaultLocale);
			}
		}

		virtual public void RemoveDefaultLocale()
		{
			this.Set(Meta.DefaultLocale, null);
		}
		public bool CanReadLocales 
		{
			get
			{
				return this.CanRead(this.Meta.Locales);
			}
		}

		public bool CanWriteLocales 
		{
			get
			{
				return this.CanWrite(this.Meta.Locales);
			}
		}



		virtual public Locale[] Locales
		{ 
			get
			{
				return (Locale[])this.Get(Meta.Locales);
			}
			set
			{
				this.Set(Meta.Locales, value);
			}
		}

		virtual public void AddLocale (Locale value)
		{
			this.Add(Meta.Locales, value);
		}

		virtual public void RemoveLocale (Locale value)
		{
			this.Remove(Meta.Locales, value);
		}

		virtual public bool ExistLocales
		{
			get
			{
				return this.Exist(Meta.Locales);
			}
		}

		virtual public void RemoveLocales()
		{
			this.Set(Meta.Locales, null);
		}
		public bool CanReadGuest 
		{
			get
			{
				return this.CanRead(this.Meta.Guest);
			}
		}

		public bool CanWriteGuest 
		{
			get
			{
				return this.CanWrite(this.Meta.Guest);
			}
		}



		virtual public User Guest
		{ 
			get
			{
				return (User) this.Get(Meta.Guest);
			}
			set
			{
				this.Set(Meta.Guest, value);
			}
		}

		virtual public bool ExistGuest
		{
			get
			{
				return this.Exist(Meta.Guest);
			}
		}

		virtual public void RemoveGuest()
		{
			this.Set(Meta.Guest, null);
		}


	}
}