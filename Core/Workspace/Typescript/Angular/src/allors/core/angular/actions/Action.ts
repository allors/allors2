import { Observable } from 'rxjs';

import { ActionTarget, ActionResult } from '@allors/angular/actions';

export interface Action {
  name: string;
  displayName: (target: ActionTarget) => string;
  description: (target: ActionTarget) => string;
  disabled: (target: ActionTarget) => boolean;
  execute: (target: ActionTarget) => void;

  result: Observable<ActionResult>;
}
