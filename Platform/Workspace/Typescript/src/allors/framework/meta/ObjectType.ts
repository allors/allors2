import { AssociationType } from './AssociationType';
import { ConcreteMethodType } from './ConcreteMethodType';
import { ConcreteRoleType } from './ConcreteRoleType';
import { ExclusiveMethodType } from './ExclusiveMethodType';
import { ExclusiveRoleType } from './ExclusiveRoleType';
import { MetaObject } from './MetaObject';
import { MetaPopulation } from './MetaPopulation';
import { MethodType } from './MethodType';
import { RoleType } from './RoleType';

export enum Kind {
  unit,
  class,
  interface,
}

export class ObjectType implements MetaObject {
  id: string;
  name: string;
  plural: string;
  kind: Kind;

  interfaceByName: { [name: string]: ObjectType; } = {};
  classes: ObjectType[] = [];

  roleTypeByName: { [name: string]: RoleType; } = {};
  exclusiveRoleTypes: ExclusiveRoleType[] = [];
  concreteRoleTypes: ConcreteRoleType[] = [];

  associationTypeByName: { [name: string]: AssociationType; } = {};

  methodTypeByName: { [name: string]: MethodType; } = {};
  exclusiveMethodTypes: ExclusiveMethodType[] = [];
  concreteMethodTypes: ConcreteMethodType[] = [];

  constructor(public metaPopulation: MetaPopulation) {
  }

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

  derive(): void {
    const interfaces: ObjectType[] = [];
    this.addInterfaces(interfaces);

    this.exclusiveRoleTypes.forEach((v) => this.roleTypeByName[v.name] = v);
    this.concreteRoleTypes.forEach((v) => this.roleTypeByName[v.name] = v);

    this.exclusiveMethodTypes.forEach((v) => this.methodTypeByName[v.name] = v);
    this.concreteMethodTypes.forEach((v) => this.methodTypeByName[v.name] = v);

    if (this.isClass) {
      this.classes.push(this);
    }

    interfaces.forEach((v) => {

      if (this.isClass) {
        v.classes.push(this);
      }

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

      Object.keys(v.associationTypeByName).forEach((name) => {
        if (this.associationTypeByName[name]) {
          this.associationTypeByName[name] = v.associationTypeByName[name];
        }
      });
    });
  }

  private addInterfaces(interfaces: ObjectType[]): void {
    Object.keys(this.interfaceByName)
      .map((name) => this.interfaceByName[name])
      .forEach((objectType) => {
        interfaces.push(objectType);
        objectType.addInterfaces(interfaces);
      });
  }
}
