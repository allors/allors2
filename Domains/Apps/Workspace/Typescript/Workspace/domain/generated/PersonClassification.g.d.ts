import { SessionObject, Method } from "@allors/framework";
import { PartyClassification } from './PartyClassification.g';
export interface PersonClassification extends SessionObject, PartyClassification {
    CanExecuteDelete: boolean;
    Delete: Method;
}
