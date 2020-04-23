import { PushRequestNewObject } from './PushRequestNewObject';
import { PushRequestObject } from './PushRequestObject';

export class PushRequest {
    public newObjects?: PushRequestNewObject[];
    public objects?: PushRequestObject[];

    constructor(fields?: Partial<PushRequest>) {
        Object.assign(this, fields);
    }
}
