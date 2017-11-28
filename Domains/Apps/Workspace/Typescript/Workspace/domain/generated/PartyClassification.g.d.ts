import { SessionObject, Method } from "@allors/framework";
export interface PartyClassification extends SessionObject {
    Name: string;
    CanExecuteDelete: boolean;
    Delete: Method;
}
