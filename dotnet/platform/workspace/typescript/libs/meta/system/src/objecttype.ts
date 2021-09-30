import { AssociationType } from './AssociationType';
import { MetaObject } from './MetaObject';
import { MetaPopulation } from './MetaPopulation';
import { MethodType } from './MethodType';
import { RoleType } from './RoleType';

import { unitIds } from './unitIds';

export enum Kind {
  unit,
  class,
  interface,
}

export class ObjectType implements MetaObject {

  interfaces: ObjectType[];
  subtypes: ObjectType[];
  classes: ObjectType[];

  exclusiveRoleTypes: RoleType[];
  exclusiveAssociationTypes: AssociationType[];
  exclusiveMethodTypes: MethodType[];

  roleTypes: RoleType[];
  associationTypes: AssociationType[];
  methodTypes: MethodType[];

  roleTypeByName: Map<string, RoleType>;
  associationTypeByName: Map<string, AssociationType>;
  methodTypeByName: Map<string, MethodType>;

  constructor(
    public metaPopulation: MetaPopulation,
    public id: string,
    public name: string,
    public plural: string,
    public kind: Kind
  ) {
    this.interfaces = [];
    this.subtypes = [];
    this.classes = [];

    this.exclusiveRoleTypes = [];
    this.exclusiveAssociationTypes = [];
    this.exclusiveMethodTypes = [];

    this.roleTypes = [];
    this.associationTypes = [];
    this.methodTypes = [];

    this.roleTypeByName = new Map();
    this.associationTypeByName = new Map();
    this.methodTypeByName = new Map();
  }

  get isUnit(): boolean {
    return this.kind === Kind.unit;
  }

  get isBinary(): boolean {
    return this.id === unitIds.Binary;
  }

  get isBoolean(): boolean {
    return this.id === unitIds.Boolean;
  }

  get isDateTime(): boolean {
    return this.id === unitIds.DateTime;
  }

  get isDecimal(): boolean {
    return this.id === unitIds.Decimal;
  }

  get isFloat(): boolean {
    return this.id === unitIds.Float;
  }

  get isInteger(): boolean {
    return this.id === unitIds.Integer;
  }

  get isString(): boolean {
    return this.id === unitIds.String;
  }

  get isUnique(): boolean {
    return this.id === unitIds.Unique;
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
}
