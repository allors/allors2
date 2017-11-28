import { Predicate } from "./Predicate";
export declare class Not implements Predicate {
    predicate: Predicate;
    constructor(fields?: Partial<Not>);
    toJSON(): any;
}
