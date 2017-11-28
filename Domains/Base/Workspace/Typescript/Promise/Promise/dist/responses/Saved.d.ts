import { ISession, PushResponse } from "@allors/base-domain";
export declare class Saved {
    session: ISession;
    response: PushResponse;
    constructor(session: ISession, response: PushResponse);
}
