export interface Data {
    domains: string[];
    interfaces?: Interface[];
    classes?: Class[];
}

export interface Interface {
    id: string;
    name: string;
    plural: string;
    interfaces?: string[];
    exclusiveRoleTypes?: RoleType[];
    associationTypes?: AssociationType[];
    exclusiveMethodTypes?: MethodType[];
}

export interface Class {
    id: string;
    name: string;
    plural: string;
    interfaces?: string[];
    exclusiveRoleTypes?: RoleType[];
    concreteRoleTypes?: ConcreteRoleType[];
    associationTypes?: AssociationType[];
    exclusiveMethodTypes?: MethodType[];
    concreteMethodTypes?: ConcreteMethodType[];
}

export interface RoleType {
    id: string;
    name: string;
    singular: string;
    objectType: string;
    isUnit: boolean;
    isOne: boolean;
    isDerived: boolean;
    isRequired: boolean;
}

export interface ConcreteRoleType {
    id: string;
    isRequired: boolean;
}

export interface AssociationType {
    name: string;
    id: string;
    objectType: string;
}

export interface MethodType {
    name: string;
    id: string;
}

export interface ConcreteMethodType {
    id: string;
}
