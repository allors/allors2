import { ObjectTyped, RoleType, AssociationType, Data } from '@allors/framework';
export interface MetaDomain {
    Version: MetaVersion;
    Deletable: MetaDeletable;
    Enumeration: MetaEnumeration;
    Localised: MetaLocalised;
    ObjectState: MetaObjectState;
    UniquelyIdentifiable: MetaUniquelyIdentifiable;
    User: MetaUser;
    Task: MetaTask;
    WorkItem: MetaWorkItem;
    I1: MetaI1;
    LocalisedText: MetaLocalisedText;
    AccessControl: MetaAccessControl;
    Counter: MetaCounter;
    Country: MetaCountry;
    Currency: MetaCurrency;
    Language: MetaLanguage;
    Locale: MetaLocale;
    Login: MetaLogin;
    Media: MetaMedia;
    MediaContent: MetaMediaContent;
    AutomatedAgent: MetaAutomatedAgent;
    Permission: MetaPermission;
    Person: MetaPerson;
    Role: MetaRole;
    SecurityToken: MetaSecurityToken;
    Singleton: MetaSingleton;
    UserGroup: MetaUserGroup;
    Notification: MetaNotification;
    NotificationList: MetaNotificationList;
    TaskAssignment: MetaTaskAssignment;
    TaskList: MetaTaskList;
    C1: MetaC1;
    Dependent: MetaDependent;
    Gender: MetaGender;
    Order: MetaOrder;
    OrderLine: MetaOrderLine;
    OrderLineVersion: MetaOrderLineVersion;
    PaymentState: MetaPaymentState;
    ShipmentState: MetaShipmentState;
    OrderState: MetaOrderState;
    OrderVersion: MetaOrderVersion;
    Organisation: MetaOrganisation;
    UnitSample: MetaUnitSample;
}
export interface MetaVersion extends ObjectTyped {
    DerivationTimeStamp: RoleType;
}
export interface MetaDeletable extends ObjectTyped {
}
export interface MetaEnumeration extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
}
export interface MetaLocalised extends ObjectTyped {
    Locale: RoleType;
}
export interface MetaObjectState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
}
export interface MetaUniquelyIdentifiable extends ObjectTyped {
    UniqueId: RoleType;
}
export interface MetaUser extends ObjectTyped {
    UserName: RoleType;
    NormalizedUserName: RoleType;
    UserEmail: RoleType;
    UserEmailConfirmed: RoleType;
    TaskList: RoleType;
    NotificationList: RoleType;
    Locale: RoleType;
    SingletonWhereGuest: AssociationType;
    TaskAssignmentsWhereUser: AssociationType;
}
export interface MetaTask extends ObjectTyped {
    WorkItem: RoleType;
    DateCreated: RoleType;
    DateClosed: RoleType;
    Participants: RoleType;
    Performer: RoleType;
    UniqueId: RoleType;
    TaskAssignmentsWhereTask: AssociationType;
}
export interface MetaWorkItem extends ObjectTyped {
    WorkItemDescription: RoleType;
    TasksWhereWorkItem: AssociationType;
}
export interface MetaI1 extends ObjectTyped {
    I1AllorsString: RoleType;
}
export interface MetaLocalisedText extends ObjectTyped {
    Text: RoleType;
    Locale: RoleType;
    CountryWhereLocalisedName: AssociationType;
    CurrencyWhereLocalisedName: AssociationType;
    LanguageWhereLocalisedName: AssociationType;
    EnumerationWhereLocalisedName: AssociationType;
}
export interface MetaAccessControl extends ObjectTyped {
}
export interface MetaCounter extends ObjectTyped {
    UniqueId: RoleType;
}
export interface MetaCountry extends ObjectTyped {
    Currency: RoleType;
    IsoCode: RoleType;
    Name: RoleType;
    LocalisedNames: RoleType;
    LocalesWhereCountry: AssociationType;
}
export interface MetaCurrency extends ObjectTyped {
    IsoCode: RoleType;
    Name: RoleType;
    LocalisedNames: RoleType;
    CountriesWhereCurrency: AssociationType;
}
export interface MetaLanguage extends ObjectTyped {
    IsoCode: RoleType;
    Name: RoleType;
    NativeName: RoleType;
    LocalisedNames: RoleType;
    LocalesWhereLanguage: AssociationType;
}
export interface MetaLocale extends ObjectTyped {
    Name: RoleType;
    Language: RoleType;
    Country: RoleType;
    SingletonsWhereDefaultLocale: AssociationType;
    SingletonWhereLocale: AssociationType;
    LocalisedsWhereLocale: AssociationType;
}
export interface MetaLogin extends ObjectTyped {
}
export interface MetaMedia extends ObjectTyped {
    Revision: RoleType;
    MediaContent: RoleType;
    InData: RoleType;
    InDataUri: RoleType;
    FileName: RoleType;
    Type: RoleType;
    UniqueId: RoleType;
    PeopleWherePhoto: AssociationType;
}
export interface MetaMediaContent extends ObjectTyped {
    Type: RoleType;
    Data: RoleType;
    MediaWhereMediaContent: AssociationType;
}
export interface MetaAutomatedAgent extends ObjectTyped {
    UserName: RoleType;
    NormalizedUserName: RoleType;
    UserEmail: RoleType;
    UserEmailConfirmed: RoleType;
    TaskList: RoleType;
    NotificationList: RoleType;
    Locale: RoleType;
    SingletonWhereGuest: AssociationType;
    TaskAssignmentsWhereUser: AssociationType;
}
export interface MetaPermission extends ObjectTyped {
}
export interface MetaPerson extends ObjectTyped {
    FirstName: RoleType;
    LastName: RoleType;
    MiddleName: RoleType;
    BirthDate: RoleType;
    FullName: RoleType;
    IsStudent: RoleType;
    Photo: RoleType;
    Weight: RoleType;
    CycleOne: RoleType;
    CycleMany: RoleType;
    UserName: RoleType;
    NormalizedUserName: RoleType;
    UserEmail: RoleType;
    UserEmailConfirmed: RoleType;
    TaskList: RoleType;
    NotificationList: RoleType;
    Locale: RoleType;
    UniqueId: RoleType;
    OrganisationWhereEmployee: AssociationType;
    OrganisationWhereManager: AssociationType;
    OrganisationsWhereOwner: AssociationType;
    OrganisationsWhereShareholder: AssociationType;
    OrganisationsWhereCycleOne: AssociationType;
    OrganisationsWhereCycleMany: AssociationType;
    TasksWhereParticipant: AssociationType;
    TasksWherePerformer: AssociationType;
    SingletonWhereGuest: AssociationType;
    TaskAssignmentsWhereUser: AssociationType;
}
export interface MetaRole extends ObjectTyped {
    UniqueId: RoleType;
}
export interface MetaSecurityToken extends ObjectTyped {
}
export interface MetaSingleton extends ObjectTyped {
    DefaultLocale: RoleType;
    Locales: RoleType;
    Guest: RoleType;
}
export interface MetaUserGroup extends ObjectTyped {
    UniqueId: RoleType;
}
export interface MetaNotification extends ObjectTyped {
    Confirmed: RoleType;
    Title: RoleType;
    Description: RoleType;
    DateCreated: RoleType;
    NotificationListWhereUnconfirmedNotification: AssociationType;
}
export interface MetaNotificationList extends ObjectTyped {
    UnconfirmedNotifications: RoleType;
    UserWhereNotificationList: AssociationType;
}
export interface MetaTaskAssignment extends ObjectTyped {
    User: RoleType;
    Task: RoleType;
    TaskListWhereTaskAssignment: AssociationType;
    TaskListWhereOpenTaskAssignment: AssociationType;
}
export interface MetaTaskList extends ObjectTyped {
    TaskAssignments: RoleType;
    OpenTaskAssignments: RoleType;
    Count: RoleType;
    UserWhereTaskList: AssociationType;
}
export interface MetaC1 extends ObjectTyped {
    C1AllorsBinary: RoleType;
    C1AllorsString: RoleType;
    C1C1One2Manies: RoleType;
    C1C1One2One: RoleType;
    I1AllorsString: RoleType;
    C1WhereC1C1One2Many: AssociationType;
    C1WhereC1C1One2One: AssociationType;
}
export interface MetaDependent extends ObjectTyped {
}
export interface MetaGender extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
}
export interface MetaOrder extends ObjectTyped {
    CurrentVersion: RoleType;
    AllVersions: RoleType;
}
export interface MetaOrderLine extends ObjectTyped {
    CurrentVersion: RoleType;
    AllVersions: RoleType;
}
export interface MetaOrderLineVersion extends ObjectTyped {
    DerivationTimeStamp: RoleType;
    OrderLineWhereCurrentVersion: AssociationType;
    OrderLineWhereAllVersion: AssociationType;
}
export interface MetaPaymentState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
}
export interface MetaShipmentState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
}
export interface MetaOrderState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
}
export interface MetaOrderVersion extends ObjectTyped {
    DerivationTimeStamp: RoleType;
    OrderWhereCurrentVersion: AssociationType;
    OrderWhereAllVersion: AssociationType;
}
export interface MetaOrganisation extends ObjectTyped {
    Employees: RoleType;
    Manager: RoleType;
    Name: RoleType;
    Owner: RoleType;
    Shareholders: RoleType;
    CycleOne: RoleType;
    CycleMany: RoleType;
    UniqueId: RoleType;
    PeopleWhereCycleOne: AssociationType;
    PeopleWhereCycleMany: AssociationType;
}
export interface MetaUnitSample extends ObjectTyped {
    AllorsBinary: RoleType;
    AllorsDateTime: RoleType;
    AllorsBoolean: RoleType;
    AllorsDouble: RoleType;
    AllorsInteger: RoleType;
    AllorsString: RoleType;
    AllorsUnique: RoleType;
    AllorsDecimal: RoleType;
}
export declare let data: Data;
