import { Origin } from '@allors/workspace/meta/system';
import { ObjectType } from './ObjectType';
import { OperandType } from './OperandType';

export interface PropertyType extends OperandType {
  origin: Origin;

  singularName: string;

  pluralName: string;

  objectType: ObjectType;

  isOne: boolean;

  isMany: boolean;

  // get(strategy: IStrategy, ofType: IComposite): object;
}
