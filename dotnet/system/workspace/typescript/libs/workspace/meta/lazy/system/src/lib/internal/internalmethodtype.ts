import { MethodType } from '@allors/workspace/meta/system';
import { InternalMetaObject } from './InternalMetaObject';
import { InternalOperandType } from './InternalOperandType';

export interface InternalMethodType extends InternalMetaObject, InternalOperandType, MethodType {}
