// Allors generated file. 
// Do not edit this file, changes will be overwritten.
namespace Allors.Workspace.Domain
{
	public partial interface UniquelyIdentifiable : ISessionObject 
	{


		global::System.Guid UniqueId 
		{
			get;
			set;
		}

		bool ExistUniqueId{get;}

		void RemoveUniqueId();


	}
}