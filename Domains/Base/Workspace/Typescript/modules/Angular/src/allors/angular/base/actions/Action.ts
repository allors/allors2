import { MethodType, ISessionObject } from '../../../framework';
import { ActionTarget } from './ActionTarget';

export interface Action {
  name?: (target: ActionTarget) => string;
  description?: (target: ActionTarget) => string;
  disabled?: (target: ActionTarget) => boolean;
  handler?: (target: ActionTarget) => void;

  method?: MethodType;
}
