import { Predicate } from "./Predicate";
export declare class Or implements Predicate {
    predicates: Predicate[];
    constructor(fields?: Partial<Or>);
    toJSON(): any;
}
