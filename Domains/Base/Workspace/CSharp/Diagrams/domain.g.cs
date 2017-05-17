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
				public interface User 
				{
								TaskList TaskList {set;}


								NotificationList NotificationList {set;}

				}
				public interface ApproveTask : Task 
				{
				}
				public interface Task : UniquelyIdentifiable 
				{
								WorkItem WorkItem {set;}


								global::System.DateTime DateCreated {set;}


								global::System.DateTime? DateClosed {set;}


								Person Participants {set;}


								Person Performer {set;}


								global::System.String Comment {set;}

				}
				public interface WorkItem 
				{
								global::System.String WorkItemDescription {set;}

				}
				public interface Counter : UniquelyIdentifiable 
				{
				}
				public interface Media : UniquelyIdentifiable 
				{
								global::System.Guid? Revision {set;}


								global::System.String InDataUri {set;}

				}
				public interface AutomatedAgent : User 
				{
				}
				public interface Person : User, UniquelyIdentifiable 
				{
				}
				public interface Role : UniquelyIdentifiable 
				{
				}
				public interface UserGroup : UniquelyIdentifiable 
				{
				}
				public interface Notification 
				{
								global::System.Boolean Confirmed {set;}


								global::System.String Title {set;}


								global::System.String Description {set;}


								global::System.DateTime DateCreated {set;}

				}
				public interface NotificationList 
				{
								Notification UnconfirmedNotifications {set;}

				}
				public interface TaskAssignment 
				{
								User User {set;}


								Task Task {set;}

				}
				public interface TaskList 
				{
								TaskAssignment TaskAssignments {set;}


								TaskAssignment OpenTaskAssignments {set;}


								global::System.Int32? Count {set;}

				}

}