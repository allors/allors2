import * as data from "./data/Population";
import { ObjectType } from "./ObjectType";

export class Population {
    readonly objectTypeByName: Map<string, ObjectType>;

    constructor(dataPopulation: data.Population) {
        this.objectTypeByName = new Map();

        ["Binary", "Boolean", "DateTime", "Decimal", "Float", "Integer", "String", "Unique"]
        .forEach((name) => {
            let unit = new ObjectType(name);
            this.objectTypeByName.set(unit.name, unit);
        });
    }
}