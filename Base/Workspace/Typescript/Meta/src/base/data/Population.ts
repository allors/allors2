export interface Population {
    domains: string[];
    objectTypes?: ObjectType[];
}

export interface ObjectType {
    name: string;
    interfaces: string[];
    roleTypes: RoleType[];
    methodTypes: MethodType[];
}

export interface RoleType {
    name: string;
    objectType: string;
    isUnit: boolean;
    isOne: boolean;
    isRequired: boolean;
}

export interface MethodType {
    name: string;
}


