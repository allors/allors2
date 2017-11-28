import { SessionObject, Method } from "@allors/framework";
import { Auditable } from './Auditable.g';
import { Deletable } from './Deletable.g';
import { ContactMechanismType } from './ContactMechanismType.g';
export interface ContactMechanism extends SessionObject, Auditable, Deletable {
    Description: string;
    ContactMechanismType: ContactMechanismType;
    CanExecuteDelete: boolean;
    Delete: Method;
}
