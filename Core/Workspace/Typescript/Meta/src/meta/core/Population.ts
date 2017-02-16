import { ObjectType } from "./ObjectType";

export class Population {
    readonly domains: string[] = [];

    readonly objectTypeByName: { [email: string]: ObjectType; } = {};

    coreInit() {
        this.domains.push("Core");

        ["Binary", "Boolean", "DateTime", "Decimal", "Float", "Integer", "String", "Unique"]
        .forEach((name) => {
            let unit: ObjectType = { name: name, isUnit: false };
            this.objectTypeByName[unit.name] = unit;
        });
    }
}