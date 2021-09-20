import { PropertyType } from '@allors/workspace/meta/system';
import { InternalOperandType } from './InternalOperandType';

export interface InternalPropertyType extends InternalOperandType, PropertyType {}
