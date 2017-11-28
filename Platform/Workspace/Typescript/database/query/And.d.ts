import { Predicate } from "./Predicate";
export declare class And implements Predicate {
    predicates: Predicate[];
    constructor(fields?: Partial<And>);
    toJSON(): any;
}
