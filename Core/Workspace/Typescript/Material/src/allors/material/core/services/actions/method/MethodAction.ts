import { MatSnackBar } from '@angular/material/snack-bar';
import { Subject } from 'rxjs';

import { Action, ActionTarget, Invoked, Context, RefreshService} from '../../../../../angular';
import { MethodType, ISessionObject } from '../../../../../framework';

import { MethodConfig } from './MethodConfig';
import { SaveService } from '../../save';

export class MethodAction implements Action {

  name = 'method';

  constructor(
    refreshService: RefreshService,
    snackBar: MatSnackBar,
    context: Context,
    saveService: SaveService,
    public methodType: MethodType,
    public config: MethodConfig) {

    this.execute = (target: ActionTarget) => {

      const objects = Array.isArray(target) ? target as ISessionObject[] : [target as ISessionObject];
      const methods = objects.filter((v) => v.canExecute(methodType)).map((v) => v[methodType.name]);

      if (methods.length > 0) {
        context.invoke(methods)
          .subscribe((invoked: Invoked) => {
            snackBar.open('Successfully executed ' + methodType.name + '.', 'close', { duration: 5000 });
            refreshService.refresh();
            this.result.next(true);
          },
          saveService.errorHandler);
      }
    };
  }

  result = new Subject<boolean>();

  execute: (target: ActionTarget) => void;

  displayName = () => (this.config && this.config.name) || this.methodType.name;
  description = () => (this.config && this.config.description) || this.methodType.name;
  disabled = (target: ActionTarget) => {
    if (Array.isArray(target)) {
      return target.length > 0 ? target.find(v => v[`CanExecute${this.methodType.name}`]) === undefined : true;
    } else {
      return !target[`CanExecute${this.methodType.name}`];
    }
  }
}
