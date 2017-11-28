import { SessionObject, Method } from "@allors/framework";
import { ContactMechanism } from './ContactMechanism.g';
export interface ElectronicAddress extends SessionObject, ContactMechanism {
    ElectronicAddressString: string;
    CanExecuteDelete: boolean;
    Delete: Method;
}
