import { Response } from "./Response";

export interface PullResponse extends Response {
    userSecurityHash: string;
    objects?: string[][];

    namedObjects?: { [id: string]: string; };
    namedCollections?: { [id: string]: string[]; };
    namedValues?: { [id: string]: any; };
}
