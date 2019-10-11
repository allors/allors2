namespace Allors.Meta.Data {
    export interface MetaPopulationData {
        domains: string[];
        interfaces?: ObjectTypeData[];
        classes?: ObjectTypeData[];
        relationTypes?: RelationTypeData[];
        methodTypes?: MethodTypeData[];
    }

    export interface ObjectTypeData {
        id: string;
        name: string;
        plural: string;
        interfaceIds?: string[];
    }

    export interface RelationTypeData {
        id: string;
        associationType: AssociationTypeData;
        roleType: RoleTypeData;
        concreteRoleTypes?: ConcreteRoleTypeData[];
        isDerived?: boolean;
    }

    export interface AssociationTypeData {
        id: string;
        objectTypeId: string;
        name: string;
        isOne: boolean;
    }

    export interface RoleTypeData {
        id: string;
        objectTypeId: string;
        singular: string;
        plural: string;
        isUnit: boolean;
        isOne: boolean;
        isRequired?: boolean;
    }

    export interface ConcreteRoleTypeData {
        objectTypeId: string;
        isRequired: boolean;
    }

    export interface MethodTypeData {
        id: string;
        objectTypeId: string;
        name: string;
    }
}
