import { Workspace } from '@allors/workspace/domain';
import { extendPerson } from './custom/Person';

export { Auditable } from './generated/Auditable.g';
export { Deletable } from './generated/Deletable.g';
export { Enumeration } from './generated/Enumeration.g';
export { UniquelyIdentifiable } from './generated/UniquelyIdentifiable.g';
export { Version } from './generated/Version.g';
export { Printable } from './generated/Printable.g';
export { Localised } from './generated/Localised.g';
export { ApproveTask } from './generated/ApproveTask.g';
export { ObjectState } from './generated/ObjectState.g';
export { Task } from './generated/Task.g';
export { User } from './generated/User.g';
export { WorkItem } from './generated/WorkItem.g';
export { Address } from './generated/Address.g';
export { Addressable } from './generated/Addressable.g';
export { I1 } from './generated/I1.g';
export { I12 } from './generated/I12.g';
export { I2 } from './generated/I2.g';
export { S1 } from './generated/S1.g';

export { Counter } from './generated/Counter.g';
export { Singleton } from './generated/Singleton.g';
export { Media } from './generated/Media.g';
export { MediaContent } from './generated/MediaContent.g';
export { PrintDocument } from './generated/PrintDocument.g';
export { Template } from './generated/Template.g';
export { TemplateType } from './generated/TemplateType.g';
export { PreparedExtent } from './generated/PreparedExtent.g';
export { PreparedFetch } from './generated/PreparedFetch.g';
export { Country } from './generated/Country.g';
export { Currency } from './generated/Currency.g';
export { Language } from './generated/Language.g';
export { Locale } from './generated/Locale.g';
export { LocalisedMedia } from './generated/LocalisedMedia.g';
export { LocalisedText } from './generated/LocalisedText.g';
export { AccessControl } from './generated/AccessControl.g';
export { IdentityClaim } from './generated/IdentityClaim.g';
export { Login } from './generated/Login.g';
export { Permission } from './generated/Permission.g';
export { Role } from './generated/Role.g';
export { SecurityToken } from './generated/SecurityToken.g';
export { AutomatedAgent } from './generated/AutomatedAgent.g';
export { Notification } from './generated/Notification.g';
export { NotificationList } from './generated/NotificationList.g';
export { Person } from './generated/Person.g';
export { TaskAssignment } from './generated/TaskAssignment.g';
export { UserGroup } from './generated/UserGroup.g';
export { C1 } from './generated/C1.g';
export { C2 } from './generated/C2.g';
export { Data } from './generated/Data.g';
export { Dependent } from './generated/Dependent.g';
export { Gender } from './generated/Gender.g';
export { HomeAddress } from './generated/HomeAddress.g';
export { MailboxAddress } from './generated/MailboxAddress.g';
export { MediaTyped } from './generated/MediaTyped.g';
export { Order } from './generated/Order.g';
export { OrderLine } from './generated/OrderLine.g';
export { OrderLineVersion } from './generated/OrderLineVersion.g';
export { OrderState } from './generated/OrderState.g';
export { OrderVersion } from './generated/OrderVersion.g';
export { Organisation } from './generated/Organisation.g';
export { Page } from './generated/Page.g';
export { PaymentState } from './generated/PaymentState.g';
export { ShipmentState } from './generated/ShipmentState.g';
export { UnitSample } from './generated/UnitSample.g';

export function extend(workspace: Workspace) {
  extendPerson(workspace);
}
