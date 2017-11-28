import { SessionObject } from "@allors/framework";
export interface Period extends SessionObject {
    FromDate: Date;
    ThroughDate: Date;
}
