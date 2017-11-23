import { SessionObject } from "@allors/base-domain";
export interface UniquelyIdentifiable extends SessionObject {
    UniqueId: string;
}
