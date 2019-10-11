namespace Allors.Protocol {
    export interface PushResponse extends Response {
        newObjects?: Allors.Protocol.PushResponseNewObject[];
    }
}
