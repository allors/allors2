namespace Allors.Domain
{
				public interface Enumeration : UniquelyIdentifiable 
				{
				}
				public interface ObjectState : UniquelyIdentifiable 
				{
								global::System.String Name {set;}

				}
				public interface UniquelyIdentifiable 
				{
								global::System.Guid UniqueId {set;}

				}
				public interface Counter : UniquelyIdentifiable 
				{
				}
				public interface Media : UniquelyIdentifiable 
				{
								global::System.Guid? Revision {set;}


								global::System.String InDataUri {set;}

				}
				public interface Person : UniquelyIdentifiable 
				{
				}
				public interface Role : UniquelyIdentifiable 
				{
				}
				public interface UserGroup : UniquelyIdentifiable 
				{
				}

}