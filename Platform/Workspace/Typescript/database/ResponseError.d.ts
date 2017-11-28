import { Response } from "./Response";
export declare class ResponseError extends Error {
    response: Response;
    constructor(response: Response);
}
