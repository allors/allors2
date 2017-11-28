import { SessionObject } from "@allors/framework";
import { User } from './User.g';
export interface Auditable extends SessionObject {
    CreatedBy: User;
    LastModifiedBy: User;
    CreationDate: Date;
    LastModifiedDate: Date;
}
