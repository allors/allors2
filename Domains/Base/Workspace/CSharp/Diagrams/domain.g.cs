namespace Allors.Domain
{
				public interface Deletable 
				{
				}
				public interface Enumeration : UniquelyIdentifiable 
				{
								LocalisedText LocalisedNames {set;}


								global::System.String Name {set;}


								global::System.Boolean IsActive {set;}

				}
				public interface Localised 
				{
								Locale Locale {set;}

				}
				public interface ObjectState : UniquelyIdentifiable 
				{
								global::System.String Name {set;}

				}
				public interface UniquelyIdentifiable 
				{
								global::System.Guid UniqueId {set;}

				}
				public interface User : Localised 
				{
								global::System.Boolean? UserEmailConfirmed {set;}


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
				public interface LocalisedText : Localised 
				{
								global::System.String Text {set;}

				}
				public interface AccessControl : Deletable 
				{
				}
				public interface Counter : UniquelyIdentifiable 
				{
				}
				public interface Country 
				{
								Currency Currency {set;}


								global::System.String IsoCode {set;}


								global::System.String Name {set;}


								LocalisedText LocalisedNames {set;}

				}
				public interface Currency 
				{
								global::System.String IsoCode {set;}


								global::System.String Name {set;}


								LocalisedText LocalisedNames {set;}

				}
				public interface Language 
				{
								global::System.String IsoCode {set;}


								global::System.String Name {set;}


								global::System.String NativeName {set;}


								LocalisedText LocalisedNames {set;}

				}
				public interface Locale 
				{
								global::System.String Name {set;}


								Language Language {set;}


								Country Country {set;}

				}
				public interface Login : Deletable 
				{
				}
				public interface Media : UniquelyIdentifiable, Deletable 
				{
								global::System.Guid? Revision {set;}


								global::System.Byte[] InData {set;}


								global::System.String InDataUri {set;}


								global::System.String FileName {set;}


								global::System.String Type {set;}

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
				public interface Singleton 
				{
								Locale DefaultLocale {set;}


								Locale Locales {set;}


								User Guest {set;}

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