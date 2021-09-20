import { ObjectType } from './ObjectType';

export interface Unit extends ObjectType {
  isBinary: boolean;

  isBoolean: boolean;

  isDecimal: boolean;

  isDateTime: boolean;

  isFloat: boolean;

  isInteger: boolean;

  isString: boolean;

  isUnique: boolean;
}
