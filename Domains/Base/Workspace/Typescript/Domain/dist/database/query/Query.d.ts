import { ObjectType, ObjectTyped } from "@allors/base-meta";
import { Page } from "./Page";
import { Predicate } from "./Predicate";
import { Sort } from "./Sort";
import { TreeNode } from "./TreeNode";
export declare class Query {
    name: string;
    objectType: ObjectType | ObjectTyped;
    predicate: Predicate;
    union: Query[];
    intersect: Query[];
    except: Query[];
    include: TreeNode[];
    sort: Sort[];
    page: Page;
    constructor(fields?: Partial<Query>);
    toJSON(): any;
}
