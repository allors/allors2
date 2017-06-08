import { Object } from './Object';
import { RoleType, ExclusiveRoleType, ConcreteRoleType } from './RoleType';
import { MethodType, ExclusiveMethodType, ConcreteMethodType } from './MethodType';

export enum Kind {
  unit,
  class,
  interface
}

export class ObjectType implements Object {
  id: string;
  name: string;
  kind: Kind;

  interfaceByName: { [name: string]: ObjectType; } = {};

  roleTypeByName: { [name: string]: RoleType; } = {};
  exclusiveRoleTypes: ExclusiveRoleType[] = [];
  concreteRoleTypes: ConcreteRoleType[] = [];

  methodTypeByName: { [name: string]: MethodType; } = {};
  exclusiveMethodTypes: ExclusiveMethodType[] = [];
  concreteMethodTypes: ConcreteMethodType[] = [];

  get isUnit(): boolean {
    return this.kind === Kind.unit;
  }

  get isComposite(): boolean {
    return this.kind !== Kind.unit;
  }

  get isInterface(): boolean {
    return this.kind === Kind.interface;
  }

  get isClass(): boolean {
    return this.kind === Kind.class;
  }

  derive() {
    const interfaces: ObjectType[] = [];
    this.addInterfaces(interfaces);

    this.exclusiveRoleTypes.forEach(v => this.roleTypeByName[v.name] = v);
    this.concreteRoleTypes.forEach(v => this.roleTypeByName[v.name] = v);

    this.exclusiveMethodTypes.forEach(v => this.methodTypeByName[v.name] = v);
    this.concreteMethodTypes.forEach(v => this.methodTypeByName[v.name] = v);

    interfaces.forEach(v => {
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
    });
  }

  private addInterfaces(interfaces: ObjectType[]) {
    Object.keys(this.interfaceByName)
      .map((name) => this.interfaceByName[name])
      .forEach((objectType) => {
        interfaces.push(objectType);
        objectType.addInterfaces(interfaces);
      });
  }
}
