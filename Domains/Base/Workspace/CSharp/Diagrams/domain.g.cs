namespace Allors.Domain
{
				public interface Deletable 
				{
				}
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
								global::System.String UserName {set;}


								global::System.String UserEmail {set;}


								TaskList TaskList {set;}


								NotificationList NotificationList {set;}

				}
				public interface Task : UniquelyIdentifiable, Deletable 
				{
								WorkItem WorkItem {set;}


								global::System.DateTime DateCreated {set;}


								global::System.DateTime? DateClosed {set;}


								Person Participants {set;}


								Person Performer {set;}

				}
				public interface WorkItem 
				{
								global::System.String WorkItemDescription {set;}

				}
				public interface I1 
				{
								global::System.String I1AllorsString {set;}

				}
				public interface AccessControl : Deletable 
				{
				}
				public interface Counter : UniquelyIdentifiable 
				{
				}
				public interface Login : Deletable 
				{
				}
				public interface Media : UniquelyIdentifiable, Deletable 
				{
								global::System.Guid? Revision {set;}


								global::System.String InDataUri {set;}

				}
				public interface MediaContent : Deletable 
				{
				}
				public interface AutomatedAgent : User 
				{
				}
				public interface Permission : Deletable 
				{
				}
				public interface Person : User, UniquelyIdentifiable, Deletable 
				{
								global::System.String FirstName {set;}


								global::System.String LastName {set;}


								global::System.String MiddleName {set;}


								global::System.DateTime? BirthDate {set;}


								global::System.String FullName {set;}


								global::System.Boolean? IsStudent {set;}


								Media Photo {set;}


								global::System.Decimal? Weight {set;}


								Organisation CycleOne {set;}


								Organisation CycleMany {set;}

				}
				public interface Role : UniquelyIdentifiable 
				{
				}
				public interface SecurityToken : Deletable 
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
				public interface NotificationList : Deletable 
				{
								Notification UnconfirmedNotifications {set;}

				}
				public interface TaskAssignment : Deletable 
				{
								User User {set;}


								Task Task {set;}

				}
				public interface TaskList : Deletable 
				{
								TaskAssignment TaskAssignments {set;}


								TaskAssignment OpenTaskAssignments {set;}


								global::System.Int32? Count {set;}

				}
				public interface C1 : I1 
				{
								global::System.Byte[] C1AllorsBinary {set;}


								global::System.String C1AllorsString {set;}


								C1 C1C1One2Manies {set;}


								C1 C1C1One2One {set;}

				}
				public interface Dependent : Deletable 
				{
				}
				public interface Gender : Enumeration 
				{
				}
				public interface OrderObjectState : ObjectState 
				{
				}
				public interface Organisation : Deletable, UniquelyIdentifiable 
				{
								Person Employees {set;}


								Person Manager {set;}


								global::System.String Name {set;}


								Person Owner {set;}


								Person Shareholders {set;}


								Person CycleOne {set;}


								Person CycleMany {set;}

				}
				public interface UnitSample 
				{
								global::System.Byte[] AllorsBinary {set;}


								global::System.DateTime? AllorsDateTime {set;}


								global::System.Boolean? AllorsBoolean {set;}


								global::System.Double? AllorsDouble {set;}


								global::System.Int32? AllorsInteger {set;}


								global::System.String AllorsString {set;}


								global::System.Guid? AllorsUnique {set;}


								global::System.Decimal? AllorsDecimal {set;}

				}

}