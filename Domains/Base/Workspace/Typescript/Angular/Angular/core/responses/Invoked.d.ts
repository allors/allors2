import { InvokeResponse, ISession } from "@allors/framework";
export declare class Invoked {
    session: ISession;
    response: InvokeResponse;
    constructor(session: ISession, response: InvokeResponse);
}
