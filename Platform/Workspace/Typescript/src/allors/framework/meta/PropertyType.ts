import { MetaObject } from './MetaObject';
import { ObjectType } from './ObjectType';

export interface PropertyType extends MetaObject {
    objectType: ObjectType;
}
