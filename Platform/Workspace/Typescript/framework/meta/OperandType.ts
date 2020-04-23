import { MetaObject } from './MetaObject';
import { ObjectType } from './ObjectType';

export interface OperandType extends MetaObject {
    objectType: ObjectType;
}
