export interface Data {
    domains: string[];
    classes?: Classes[];
}

export interface Classes {
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


