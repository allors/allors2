import { InvokeResponse, ISession } from "@allors/base-domain";
export declare class Invoked {
    session: ISession;
    response: InvokeResponse;
    constructor(session: ISession, response: InvokeResponse);
}
