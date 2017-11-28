import { SessionObject, Method } from "@allors/framework";
export interface Deletable extends SessionObject {
    CanExecuteDelete: boolean;
    Delete: Method;
}
