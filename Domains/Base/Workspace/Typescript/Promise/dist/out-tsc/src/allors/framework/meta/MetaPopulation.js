"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const AssociationType_1 = require("./AssociationType");
const ConcreteMethodType_1 = require("./ConcreteMethodType");
const ConcreteRoleType_1 = require("./ConcreteRoleType");
const ExclusiveMethodType_1 = require("./ExclusiveMethodType");
const ExclusiveRoleType_1 = require("./ExclusiveRoleType");
const ObjectType_1 = require("./ObjectType");
class MetaPopulation {
    constructor(data) {
        this.objectTypeByName = {};
        this.metaObjectById = {};
        const idByTypeName = {
            Binary: "c28e515bcae84d6b95bf062aec8042fc",
            Boolean: "b5ee6cea4E2b498ea5dd24671d896477",
            DateTime: "c4c0934361d3418cade2fe6fd588f128",
            Decimal: "da866d8e2c4041a8ae5b5f6dae0b89c8",
            Float: "ffcabd07f35f4083bef6f6c47970ca5d",
            Integer: "ccd6f13426de4103bff9a37ec3e997a3",
            String: "ad7f5ddcbedb4aaa97acd6693a009ba9",
            Unique: "6dc0a1a888a44614adb492dd3d017c0e",
        };
        // Units
        ["Binary", "Boolean", "DateTime", "Decimal", "Float", "Integer", "String", "Unique"]
            .forEach((name) => {
            const metaUnit = new ObjectType_1.ObjectType();
            metaUnit.id = idByTypeName[name];
            metaUnit.name = name;
            metaUnit.kind = ObjectType_1.Kind.unit;
            this.objectTypeByName[metaUnit.name] = metaUnit;
            this.metaObjectById[metaUnit.id] = metaUnit;
        });
        // Interfaces
        data.interfaces.forEach((dataInterface) => {
            const metaInterface = new ObjectType_1.ObjectType();
            metaInterface.id = dataInterface.id;
            metaInterface.name = dataInterface.name;
            metaInterface.kind = ObjectType_1.Kind.interface;
            this.objectTypeByName[metaInterface.name] = metaInterface;
            this.metaObjectById[metaInterface.id] = metaInterface;
        });
        // Classes
        data.classes.forEach((dataClass) => {
            const metaClass = new ObjectType_1.ObjectType();
            metaClass.id = dataClass.id;
            metaClass.name = dataClass.name;
            metaClass.kind = ObjectType_1.Kind.class;
            this.objectTypeByName[metaClass.name] = metaClass;
            this.metaObjectById[metaClass.id] = metaClass;
        });
        const dataObjectTypes = [].concat(data.interfaces).concat(data.classes);
        dataObjectTypes.forEach((dataObjectType) => {
            const metaObjectType = this.objectTypeByName[dataObjectType.name];
            dataObjectType.interfaces.forEach((dataInterface) => {
                const metaInterface = this.objectTypeByName[dataInterface];
                metaObjectType.interfaceByName[metaInterface.name] = metaInterface;
            });
            if (dataObjectType.exclusiveRoleTypes) {
                dataObjectType.exclusiveRoleTypes.forEach((dataRoleType) => {
                    const objectType = this.objectTypeByName[dataRoleType.objectType];
                    const metaRoleType = new ExclusiveRoleType_1.ExclusiveRoleType();
                    metaRoleType.id = dataRoleType.id;
                    metaRoleType.name = dataRoleType.name;
                    metaRoleType.objectType = objectType;
                    metaRoleType.isOne = dataRoleType.isOne;
                    metaRoleType.isRequired = dataRoleType.isRequired;
                    metaObjectType.exclusiveRoleTypes.push(metaRoleType);
                    this.metaObjectById[metaRoleType.id] = metaRoleType;
                });
            }
            if (dataObjectType.associationTypes) {
                dataObjectType.associationTypes.forEach((dataAssociationType) => {
                    const metaAssociationType = new AssociationType_1.AssociationType();
                    metaAssociationType.id = dataAssociationType.id;
                    metaAssociationType.name = dataAssociationType.name;
                    metaObjectType.associationTypes.push(metaAssociationType);
                    this.metaObjectById[metaAssociationType.id] = metaAssociationType;
                });
            }
            if (dataObjectType.exclusiveMethodTypes) {
                dataObjectType.exclusiveMethodTypes.forEach((dataMethodType) => {
                    const metaMethodType = new ExclusiveMethodType_1.ExclusiveMethodType();
                    metaMethodType.id = dataMethodType.id;
                    metaMethodType.name = dataMethodType.name;
                    metaObjectType.exclusiveMethodTypes.push(metaMethodType);
                    this.metaObjectById[metaMethodType.id] = metaMethodType;
                });
            }
        });
        data.interfaces.forEach((dataInterface) => {
            const metaInterface = this.objectTypeByName[dataInterface.name];
        });
        data.classes.forEach((dataClass) => {
            const metaObjectType = this.objectTypeByName[dataClass.name];
            if (dataClass.concreteRoleTypes) {
                dataClass.concreteRoleTypes.forEach((dataRoleType) => {
                    const metaRoleType = this.metaObjectById[dataRoleType.id];
                    const metaConcreteRoleType = new ConcreteRoleType_1.ConcreteRoleType();
                    metaConcreteRoleType.roleType = metaRoleType;
                    metaConcreteRoleType.isRequired = dataRoleType.isRequired;
                    metaObjectType.concreteRoleTypes.push(metaConcreteRoleType);
                });
            }
            if (dataClass.concreteMethodTypes) {
                dataClass.concreteMethodTypes.forEach((dataMethodType) => {
                    const metaMethodType = this.metaObjectById[dataMethodType.id];
                    const metaConcreteMethodType = new ConcreteMethodType_1.ConcreteMethodType();
                    metaConcreteMethodType.methodType = metaMethodType;
                    metaObjectType.concreteMethodTypes.push(metaConcreteMethodType);
                });
            }
        });
        Object.keys(this.objectTypeByName)
            .map((name) => this.objectTypeByName[name])
            .forEach((objectType) => objectType.derive());
        // MetaDomain
        this.metaDomain = {};
        Object.keys(this.objectTypeByName)
            .forEach((objectTypeName) => {
            const objectType = this.objectTypeByName[objectTypeName];
            const metaObjectType = {
                ObjectType: objectType,
            };
            this.metaDomain[objectTypeName] = metaObjectType;
            Object.keys(objectType.roleTypeByName)
                .forEach((roleTypeName) => {
                const roleType = objectType.roleTypeByName[roleTypeName];
                metaObjectType[roleTypeName] = roleType;
            });
            objectType.associationTypes.forEach((associationType) => {
                metaObjectType[associationType.name] = associationType;
            });
        });
    }
}
exports.MetaPopulation = MetaPopulation;
//# sourceMappingURL=MetaPopulation.js.map