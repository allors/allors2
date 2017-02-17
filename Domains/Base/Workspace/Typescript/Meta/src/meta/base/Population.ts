import { Data } from "./Data";
import { ObjectType, Kind } from "./ObjectType";
import { RoleType } from "./RoleType";
import { MethodType } from "./MethodType";

export class Population {
    readonly domains: string[] = [];

    readonly objectTypeByName: { [email: string]: ObjectType; } = {};

    baseInit(this: Population, data: Data) {

        // Domains
        data.domains.forEach(dataDomain => {
            if (this.domains.indexOf(dataDomain) > -1) {
                this.domains.push(dataDomain);
            }
        });

        // Units
        ["Binary", "Boolean", "DateTime", "Decimal", "Float", "Integer", "String", "Unique"]
        .forEach((name) => {
            let unit: ObjectType = { name: name, kind: Kind.unit };
            this.objectTypeByName[unit.name] = unit;
        });

        // Composites
        data.classes.forEach(dataClass => {
            let metaClass: ObjectType = { name: dataClass.name, kind: Kind.class, interfaces: [], roleTypes: [], methodTypes: [] };
            this.objectTypeByName[metaClass.name] = metaClass;

            dataClass.interfaces.forEach(dataInterface => {
                let metaInterface = this.objectTypeByName[dataInterface];
                if (!metaInterface) {
                    metaInterface = { name: dataInterface, kind: Kind.interface };
                    this.objectTypeByName[dataInterface] = metaInterface;
                }

                metaClass.interfaces.push(metaInterface);
            });

            dataClass.roleTypes.forEach(dataRoleType => {
                let objectType = this.objectTypeByName[dataRoleType.objectType];
                let metaRoleType: RoleType = { name: dataRoleType.name, objectType: objectType, isOne: dataRoleType.isOne, isRequired: dataRoleType.isRequired };

                metaClass.roleTypes.push(metaRoleType);
            });

            dataClass.methodTypes.forEach(dataMethodType => {
                let metaMethodType: MethodType = { name: dataMethodType.name };
                metaClass.methodTypes.push(metaMethodType);
            });
        });

};
}