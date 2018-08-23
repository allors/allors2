import { PropertyType } from './PropertyType';
import { ObjectType } from './ObjectType';

export interface RoleType extends PropertyType {
    id: string;
    name: string;
    singular: string;
    objectType: ObjectType;
    isOne: boolean;
    isMany: boolean;
    isDerived: boolean;
    isRequired: boolean;
}
