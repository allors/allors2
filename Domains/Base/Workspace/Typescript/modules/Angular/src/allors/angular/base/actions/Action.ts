import { MethodType, ISessionObject } from '../../../framework';
import { ActionTarget } from './ActionTarget';

export interface Action {
  method?: MethodType;
  name?: (target: ActionTarget) => string;
  description?: (target: ActionTarget) => string;
  handler?: (target: ActionTarget) => void;
}
