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
    interfaceIds?: string[];
}

export interface Class {
    id: string;
    name: string;
    plural: string;
    interfaceIds?: string[];
}

export interface RelationType {
    id: string;
    associationType: AssociationType;
    roleType: RoleType;
    concreteRoleTypes?: ConcreteRoleType[];
    isDerived?: boolean;
}

export interface AssociationType {
    id: string;
    objectTypeId: string;
    name: string;
    isOne: boolean;
}

export interface RoleType {
    id: string;
    objectTypeId: string;
    singular: string;
    plural: string;
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
}
