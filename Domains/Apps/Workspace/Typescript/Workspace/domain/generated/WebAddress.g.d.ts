import { SessionObject, Method } from "@allors/framework";
import { ElectronicAddress } from './ElectronicAddress.g';
import { ContactMechanismType } from './ContactMechanismType.g';
import { User } from './User.g';
export declare class WebAddress extends SessionObject implements ElectronicAddress {
    readonly CanReadElectronicAddressString: boolean;
    readonly CanWriteElectronicAddressString: boolean;
    ElectronicAddressString: string;
    readonly CanReadDescription: boolean;
    readonly CanWriteDescription: boolean;
    Description: string;
    readonly CanReadContactMechanismType: boolean;
    readonly CanWriteContactMechanismType: boolean;
    ContactMechanismType: ContactMechanismType;
    readonly CanReadCreatedBy: boolean;
    readonly CanWriteCreatedBy: boolean;
    CreatedBy: User;
    readonly CanReadLastModifiedBy: boolean;
    readonly CanWriteLastModifiedBy: boolean;
    LastModifiedBy: User;
    readonly CanReadCreationDate: boolean;
    readonly CanWriteCreationDate: boolean;
    CreationDate: Date;
    readonly CanReadLastModifiedDate: boolean;
    readonly CanWriteLastModifiedDate: boolean;
    LastModifiedDate: Date;
    readonly CanExecuteDelete: boolean;
    readonly Delete: Method;
}
