import { SessionObject, Method } from "@allors/framework";
import { OrganisationClassification } from './OrganisationClassification.g';
export declare class CustomOrganisationClassification extends SessionObject implements OrganisationClassification {
    readonly CanReadName: boolean;
    readonly CanWriteName: boolean;
    Name: string;
    readonly CanExecuteDelete: boolean;
    readonly Delete: Method;
}
