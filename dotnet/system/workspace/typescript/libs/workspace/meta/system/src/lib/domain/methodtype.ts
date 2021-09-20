import { Composite } from './Composite';
import { MetaObject } from './MetaObject';
import { OperandType } from './OperandType';

export interface MethodType extends MetaObject, OperandType {
  objectType: Composite;
}
