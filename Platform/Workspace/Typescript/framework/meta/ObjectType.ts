import { AssociationType } from './AssociationType';
import { MetaObject } from './MetaObject';
import { MetaPopulation } from './MetaPopulation';
import { MethodType } from './MethodType';
import { RoleType } from './RoleType';
import * as units from './Units';
import { ids } from '../../meta/generated/ids.g';

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

  interfaces: ObjectType[] = [];
  classes: ObjectType[] = [];

  roleTypeByName: { [name: string]: RoleType; } = {};
  associationTypeByName: { [name: string]: AssociationType; } = {};
  methodTypeByName: { [name: string]: MethodType; } = {};

  constructor(public metaPopulation: MetaPopulation) {
  }

  get isUnit(): boolean {
    return this.kind === Kind.unit;
  }

  get isBinary(): boolean {
    return this.id === ids.Binary;
  }

  get isBoolean(): boolean {
    return this.id === ids.Boolean;
  }

  get isDateTime(): boolean {
    return this.id === ids.DateTime;
  }

  get isDecimal(): boolean {
    return this.id === ids.Decimal;
  }

  get isFloat(): boolean {
    return this.id === ids.Float;
  }

  get isInteger(): boolean {
    return this.id === ids.Integer;
  }

  get isString(): boolean {
    return this.id === ids.String;
  }

  get isUnique(): boolean {
    return this.id === ids.Unique;
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
    this.deriveClasses();
    this.deriveAssociations();
    this.deriveRoles();
    this.deriveMethods();
  }

  private deriveClasses() {
    if (this.isClass) {
      this.classes.push(this);
    }

    this.interfaces.forEach((v) => {

      if (this.isClass) {
        v.classes.push(this);
      }
    });
  }

  private deriveAssociations() {
    this.interfaces.forEach((v) => Object.assign(this.associationTypeByName, v.associationTypeByName));
  }

  private deriveRoles() {
    this.interfaces.forEach((v) => Object.assign(this.roleTypeByName, v.roleTypeByName));

    if (this.isClass) {
      Object.keys(this.roleTypeByName)
        .map((name) => this.roleTypeByName[name])
        .forEach((roleType) => {
          const relationType = roleType.relationType;
          if (relationType.associationType.objectType.isInterface) {
            this.roleTypeByName[roleType.name] = relationType.concreteRoleTypeByClassName[this.name];
          }
        });
    }
  }

  private deriveMethods() {
    this.interfaces.forEach((v) => Object.assign(this.methodTypeByName, v.methodTypeByName));
  }
}
