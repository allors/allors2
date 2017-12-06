"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Kind;
(function (Kind) {
    Kind[Kind["unit"] = 0] = "unit";
    Kind[Kind["class"] = 1] = "class";
    Kind[Kind["interface"] = 2] = "interface";
})(Kind = exports.Kind || (exports.Kind = {}));
class ObjectType {
    constructor() {
        this.interfaceByName = {};
        this.roleTypeByName = {};
        this.exclusiveRoleTypes = [];
        this.concreteRoleTypes = [];
        this.associationTypes = [];
        this.methodTypeByName = {};
        this.exclusiveMethodTypes = [];
        this.concreteMethodTypes = [];
    }
    get isUnit() {
        return this.kind === Kind.unit;
    }
    get isComposite() {
        return this.kind !== Kind.unit;
    }
    get isInterface() {
        return this.kind === Kind.interface;
    }
    get isClass() {
        return this.kind === Kind.class;
    }
    derive() {
        const interfaces = [];
        this.addInterfaces(interfaces);
        this.exclusiveRoleTypes.forEach((v) => this.roleTypeByName[v.name] = v);
        this.concreteRoleTypes.forEach((v) => this.roleTypeByName[v.name] = v);
        this.exclusiveMethodTypes.forEach((v) => this.methodTypeByName[v.name] = v);
        this.concreteMethodTypes.forEach((v) => this.methodTypeByName[v.name] = v);
        interfaces.forEach((v) => {
            v.exclusiveRoleTypes.forEach((roleType) => {
                if (!this.roleTypeByName[roleType.name]) {
                    this.roleTypeByName[roleType.name] = roleType;
                }
            });
            v.exclusiveMethodTypes.forEach((methodType) => {
                if (!this.methodTypeByName[methodType.name]) {
                    this.methodTypeByName[methodType.name] = methodType;
                }
            });
            v.associationTypes.forEach((associationType) => {
                if (this.associationTypes.indexOf(associationType) < 0) {
                    this.associationTypes.push(associationType);
                }
            });
        });
    }
    addInterfaces(interfaces) {
        Object.keys(this.interfaceByName)
            .map((name) => this.interfaceByName[name])
            .forEach((objectType) => {
            interfaces.push(objectType);
            objectType.addInterfaces(interfaces);
        });
    }
}
exports.ObjectType = ObjectType;
//# sourceMappingURL=ObjectType.js.map