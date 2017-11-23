import { SessionObject, Method } from "@allors/base-domain";
export interface Deletable extends SessionObject {
    CanExecuteDelete: boolean;
    Delete: Method;
}
