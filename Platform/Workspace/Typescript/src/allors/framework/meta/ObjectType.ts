import { AssociationType } from "./AssociationType";
import { ConcreteMethodType } from "./ConcreteMethodType";
import { ConcreteRoleType } from "./ConcreteRoleType";
import { ExclusiveMethodType } from "./ExclusiveMethodType";
import { ExclusiveRoleType } from "./ExclusiveRoleType";
import { MetaObject } from "./MetaObject";
import { MetaPopulation } from "./MetaPopulation";
import { MethodType } from "./MethodType";
import { RoleType } from "./RoleType";

export enum Kind {
  unit,
  class,
  interface,
}

export class ObjectType implements MetaObject {
  public id: string;
  public name: string;
  public plural: string;
  public kind: Kind;

  public interfaceByName: { [name: string]: ObjectType; } = {};

  public roleTypeByName: { [name: string]: RoleType; } = {};
  public exclusiveRoleTypes: ExclusiveRoleType[] = [];
  public concreteRoleTypes: ConcreteRoleType[] = [];

  public associationTypeByName: { [name: string]: AssociationType; } = {};

  public methodTypeByName: { [name: string]: MethodType; } = {};
  public exclusiveMethodTypes: ExclusiveMethodType[] = [];
  public concreteMethodTypes: ConcreteMethodType[] = [];

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

  public derive(): void {
    const interfaces: ObjectType[] = [];
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
