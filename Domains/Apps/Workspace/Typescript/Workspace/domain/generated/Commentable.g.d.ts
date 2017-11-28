import { SessionObject } from "@allors/framework";
export interface Commentable extends SessionObject {
    Comment: string;
}
