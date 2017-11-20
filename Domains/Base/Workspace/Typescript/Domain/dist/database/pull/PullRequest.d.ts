import { Fetch } from "../query/Fetch";
import { Query } from "../query/Query";
export declare class PullRequest {
    query: Query[];
    fetch: Fetch[];
    constructor(fields?: Partial<PullRequest>);
    toJSON(): {
        f: Fetch[];
        q: Query[];
    };
}
