import { SessionObject } from './SessionObject';

export class Method {
    constructor(
        public object: SessionObject,
        public name: string) {
    }
}
