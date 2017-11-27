import { Data } from "./Data";
import { MetaObject } from "./MetaObject";
import { ObjectType } from "./ObjectType";
export declare class MetaPopulation {
    readonly objectTypeByName: {
        [name: string]: ObjectType;
    };
    readonly metaObjectById: {
        [id: string]: MetaObject;
    };
    readonly metaDomain: any;
    constructor(data: Data);
}
