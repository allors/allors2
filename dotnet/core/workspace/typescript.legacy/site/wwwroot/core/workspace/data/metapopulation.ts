namespace Allors.Data {
  export class RoleType {
    name: string;
    objectType: string;
    isUnit: boolean;
    isOne: boolean;
    isRequired: boolean;
  }

  export class MethodType {
    name: string;
  }

  export class ObjectType {
    id: string;
    name: string;
    interfaces: string[];
    roleTypes: RoleType[];
    methodTypes: MethodType[];
  }

  export class MetaPopulation {
    domains: string[];
    classes: ObjectType[];
  }
}
