namespace Allors.Domain
{
		public interface Object 
		{
		}
		public interface AccessControlledObject  : Object 
		{
						Permission DeniedPermissions {set;}

						SecurityToken SecurityTokens {set;}

		}
		public interface Deletable  : Object 
		{
		}
		public interface Enumeration  : AccessControlledObject, UniquelyIdentifiable 
		{
						LocalisedText LocalisedNames {set;}

						global::System.String Name {set;}

						global::System.Boolean IsActive {set;}

		}
		public interface Localised  : Object 
		{
						Locale Locale {set;}

		}
		public interface ObjectState  : UniquelyIdentifiable 
		{
						Permission DeniedPermissions {set;}

						global::System.String Name {set;}

		}
		public interface SecurityTokenOwner  : Object 
		{
						SecurityToken OwnerSecurityToken {set;}

						AccessControl OwnerAccessControl {set;}

		}
		public interface Transitional  : AccessControlledObject 
		{
						ObjectState PreviousObjectState {set;}

						ObjectState LastObjectState {set;}

		}
		public interface UniquelyIdentifiable  : Object 
		{
						global::System.Guid UniqueId {set;}

		}
		public interface User  : SecurityTokenOwner, AccessControlledObject, Localised 
		{
						global::System.Boolean? UserEmailConfirmed {set;}

						global::System.String UserName {set;}

						global::System.String UserEmail {set;}

						global::System.String UserPasswordHash {set;}

						TaskList TaskList {set;}

						NotificationList NotificationList {set;}

		}
		public interface ApproveTask  : Task 
		{
						Notification RejectionNotification {set;}

		}
		public interface EmailSource  : Object 
		{
						EmailMessage EmailMessage {set;}

		}
		public interface Task  : AccessControlledObject, UniquelyIdentifiable, Deletable 
		{
						WorkItem WorkItem {set;}

						global::System.DateTime DateCreated {set;}

						global::System.DateTime? DateClosed {set;}

						Person Participants {set;}

						Person Performer {set;}

						global::System.String Comment {set;}

		}
		public interface WorkItem  : Object 
		{
						global::System.String WorkItemDescription {set;}

		}
		public interface LocalisedText  : AccessControlledObject, Localised 
		{
						global::System.String Text {set;}

		}
		public interface AccessControl  : Deletable, AccessControlledObject 
		{
						UserGroup SubjectGroups {set;}

						User Subjects {set;}

						Role Role {set;}

						Permission EffectivePermissions {set;}

						User EffectiveUsers {set;}

		}
		public interface Counter  : UniquelyIdentifiable 
		{
						global::System.Int32 Value {set;}

		}
		public interface Country  : AccessControlledObject 
		{
						Currency Currency {set;}

						global::System.String IsoCode {set;}

						global::System.String Name {set;}

						LocalisedText LocalisedNames {set;}

		}
		public interface Currency  : AccessControlledObject 
		{
						global::System.String IsoCode {set;}

						global::System.String Name {set;}

						LocalisedText LocalisedNames {set;}

		}
		public interface Language  : AccessControlledObject 
		{
						global::System.String IsoCode {set;}

						global::System.String Name {set;}

						global::System.String NativeName {set;}

						LocalisedText LocalisedNames {set;}

		}
		public interface Locale  : AccessControlledObject 
		{
						global::System.String Name {set;}

						Language Language {set;}

						Country Country {set;}

		}
		public interface Login  : Deletable 
		{
						global::System.String Key {set;}

						global::System.String Provider {set;}

						User User {set;}

		}
		public interface Media  : UniquelyIdentifiable, AccessControlledObject, Deletable 
		{
						global::System.Guid? Revision {set;}

						MediaContent MediaContent {set;}

						global::System.Byte[] InData {set;}

						global::System.String InDataUri {set;}

		}
		public interface MediaContent  : AccessControlledObject, Deletable 
		{
						global::System.String Type {set;}

						global::System.Byte[] Data {set;}

		}
		public interface AutomatedAgent  : User 
		{
						global::System.String Name {set;}

						global::System.String Description {set;}

		}
		public interface Permission  : Deletable, AccessControlledObject 
		{
						global::System.Guid OperandTypePointer {set;}

						global::System.Guid ConcreteClassPointer {set;}

						global::System.Int32 OperationEnum {set;}

		}
		public interface Person  : User, UniquelyIdentifiable 
		{
						global::System.String FirstName {set;}

						global::System.String LastName {set;}

						global::System.String MiddleName {set;}

		}
		public interface Role  : AccessControlledObject, UniquelyIdentifiable 
		{
						Permission Permissions {set;}

						global::System.String Name {set;}

		}
		public interface SecurityToken  : Deletable 
		{
						AccessControl AccessControls {set;}

		}
		public interface Singleton  : AccessControlledObject 
		{
						Locale DefaultLocale {set;}

						Locale Locales {set;}

						User Guest {set;}

						SecurityToken InitialSecurityToken {set;}

						SecurityToken DefaultSecurityToken {set;}

						AccessControl CreatorsAccessControl {set;}

						AccessControl GuestAccessControl {set;}

						AccessControl AdministratorsAccessControl {set;}

		}
		public interface UserGroup  : UniquelyIdentifiable, AccessControlledObject 
		{
						User Members {set;}

						global::System.String Name {set;}

		}
		public interface EmailMessage  : Object 
		{
						global::System.DateTime DateCreated {set;}

						global::System.DateTime? DateSending {set;}

						global::System.DateTime? DateSent {set;}

						User Sender {set;}

						User Recipients {set;}

						global::System.String Subject {set;}

						global::System.String Body {set;}

		}
		public interface Notification  : AccessControlledObject 
		{
						UniquelyIdentifiable Target {set;}

						global::System.Boolean Confirmed {set;}

						global::System.String Title {set;}

						global::System.String Description {set;}

						global::System.DateTime DateCreated {set;}

		}
		public interface NotificationList  : AccessControlledObject, Deletable 
		{
						Notification Notifications {set;}

						Notification UnconfirmedNotifications {set;}

						Notification ConfirmedNotifications {set;}

		}
		public interface TaskAssignment  : AccessControlledObject, Deletable 
		{
						User User {set;}

						Notification Notification {set;}

						Task Task {set;}

		}
		public interface TaskList  : Deletable 
		{
						TaskAssignment TaskAssignments {set;}

						TaskAssignment OpenTaskAssignments {set;}

						global::System.Int32? Count {set;}

		}
}