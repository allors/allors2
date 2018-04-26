namespace Allors.Domain
{
				public interface I1 : I12, S1 
				{
								global::System.String I1AllorsString {set;}

				}
				public interface I12 
				{
								global::System.String Name {set;}

				}
				public interface I2 : I12 
				{
				}
				public interface S1 
				{
				}
				public interface Deletable 
				{
				}
				public interface Enumeration : UniquelyIdentifiable 
				{
								global::System.String Name {set;}


								LocalisedText LocalisedNames {set;}


								global::System.Boolean IsActive {set;}

				}
				public interface UniquelyIdentifiable 
				{
								global::System.Guid UniqueId {set;}

				}
				public interface Version 
				{
								global::System.DateTime? DerivationTimeStamp {set;}

				}
				public interface Localised 
				{
								Locale Locale {set;}

				}
				public interface ObjectState : UniquelyIdentifiable 
				{
								global::System.String Name {set;}

				}
				public interface Task : UniquelyIdentifiable, Deletable 
				{
								WorkItem WorkItem {set;}


								global::System.DateTime DateCreated {set;}


								global::System.DateTime? DateClosed {set;}


								Person Participants {set;}


								Person Performer {set;}

				}
				public interface User 
				{
								global::System.String UserName {set;}


								global::System.String NormalizedUserName {set;}


								global::System.String UserEmail {set;}


								global::System.Boolean? UserEmailConfirmed {set;}


								TaskList TaskList {set;}


								NotificationList NotificationList {set;}

				}
				public interface WorkItem 
				{
								global::System.String WorkItemDescription {set;}

				}
				public interface C1 : I1 
				{
								global::System.Byte[] C1AllorsBinary {set;}


								global::System.String C1AllorsString {set;}


								C1 C1C1One2Manies {set;}


								C1 C1C1One2One {set;}

				}
				public interface C2 : I2 
				{
								S1 S1One2One {set;}

				}
				public interface Dependent : Deletable 
				{
				}
				public interface Gender : Enumeration 
				{
				}
				public interface Order 
				{
								OrderVersion CurrentVersion {set;}


								OrderVersion AllVersions {set;}

				}
				public interface OrderLine 
				{
								OrderLineVersion CurrentVersion {set;}


								OrderLineVersion AllVersions {set;}

				}
				public interface OrderLineVersion : Version 
				{
				}
				public interface OrderState : ObjectState 
				{
				}
				public interface OrderVersion : Version 
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
				public interface PaymentState : ObjectState 
				{
				}
				public interface Person : Deletable, User, UniquelyIdentifiable 
				{
								global::System.DateTime? BirthDate {set;}


								global::System.String FullName {set;}


								global::System.Boolean? IsStudent {set;}


								Media Photo {set;}


								Media Pictures {set;}


								global::System.Decimal? Weight {set;}


								Organisation CycleOne {set;}


								Organisation CycleMany {set;}


								global::System.String FirstName {set;}


								global::System.String LastName {set;}


								global::System.String MiddleName {set;}

				}
				public interface ShipmentState : ObjectState 
				{
				}
				public interface Singleton 
				{
								Locale DefaultLocale {set;}


								Locale AdditionalLocales {set;}


								User Guest {set;}

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
				public interface Counter : UniquelyIdentifiable 
				{
				}
				public interface Media : UniquelyIdentifiable, Deletable 
				{
								global::System.Guid? Revision {set;}


								MediaContent MediaContent {set;}


								global::System.Byte[] InData {set;}


								global::System.String InDataUri {set;}


								global::System.String FileName {set;}


								global::System.String Type {set;}

				}
				public interface MediaContent : Deletable 
				{
								global::System.String Type {set;}


								global::System.Byte[] Data {set;}

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


								LocalisedText LocalisedNames {set;}


								global::System.String NativeName {set;}

				}
				public interface Locale 
				{
								global::System.String Name {set;}


								Language Language {set;}


								Country Country {set;}

				}
				public interface LocalisedText : Localised, Deletable 
				{
								global::System.String Text {set;}

				}
				public interface AccessControl : Deletable 
				{
				}
				public interface Login : Deletable 
				{
				}
				public interface Permission : Deletable 
				{
				}
				public interface Role : UniquelyIdentifiable 
				{
				}
				public interface SecurityToken : Deletable 
				{
				}
				public interface AutomatedAgent : User 
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
				public interface UserGroup : UniquelyIdentifiable 
				{
				}

}