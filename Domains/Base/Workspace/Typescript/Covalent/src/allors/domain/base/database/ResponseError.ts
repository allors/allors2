import { Response } from './Response';

export class ResponseError extends Error {
    constructor(public response: Response) {
        super();

        // Fix for extending builtin objects
        Object.setPrototypeOf(this, ResponseError.prototype);
    }
}
