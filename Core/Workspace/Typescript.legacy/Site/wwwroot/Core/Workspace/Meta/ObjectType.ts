namespace Allors.Meta {
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

    constructor(public metaPopulation: MetaPopulation) {
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
  }

}
