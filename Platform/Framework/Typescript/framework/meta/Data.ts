export interface Data {
    domains: string[];
    interfaces?: Interface[];
    classes?: Class[];
    relationTypes?: RelationType[];
    methodTypes?: MethodType[];
}

export interface Interface {
    id: string;
    name: string;
    plural: string;
    interfaces?: string[];
}

export interface Class {
    id: string;
    name: string;
    plural: string;
    interfaces?: string[];
}

export interface RelationType {
    id: string;
    associationType: AssociationType;
    roleType: RoleType;
    isDerived?: boolean;
    concreteRoleTypes?: ConcreteRoleType[];
}

export interface AssociationType {
    id: string;
    objectTypeId: string;
    name: string;
    singular: string;
    isOne: boolean;
}

export interface RoleType {
    id: string;
    objectTypeId: string;
    name: string;
    singular: string;
    isUnit: boolean;
    isOne: boolean;
    isRequired?: boolean;
}

export interface ConcreteRoleType {
    objectTypeId: string;
    isRequired: boolean;
}

export interface MethodType {
    id: string;
    objectTypeId: string;
    name: string;
    concreteMethodTypes?: ConcreteMethodType[];
}

export interface ConcreteMethodType {
    objectTypeId: string;
}
