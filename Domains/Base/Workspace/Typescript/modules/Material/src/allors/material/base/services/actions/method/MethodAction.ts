import { MatSnackBar } from '@angular/material';
import { Subject } from 'rxjs';

import { Deletable } from '../../../../../domain';
import { Action, ActionTarget, Invoked, Context, RefreshService} from '../../../../../angular';
import { MethodType, ISessionObject } from '../../../../../framework';

import { MethodConfig } from './MethodConfig';

export class DeleteAction implements Action {

  constructor(
    refreshService: RefreshService,
    snackBar: MatSnackBar,
    context: Context,
    public methodType: MethodType,
    public config: MethodConfig) {

    this.execute = (target: ActionTarget) => {

      const objects = Array.isArray(target) ? target as ISessionObject[] : [target as ISessionObject];
      const methods = objects.filter((v) => v.canExecute(methodType.name)).map((v) => v[methodType.name]);

      if (methods.length > 0) {
        context.invoke(methods)
          .subscribe((invoked: Invoked) => {
            snackBar.open('Successfully executed ' + this.name + '.', 'close', { duration: 5000 });
            refreshService.refresh();
            this.result.next(true);
          });
      }
    };
  }

  result = new Subject<boolean>();

  execute: (target: ActionTarget) => void;

  name = () => (this.config && this.config.name) || this.methodType.name;
  description = () => (this.config && this.config.description) || this.methodType.name;
  disabled = (target: ActionTarget) => {
    if (Array.isArray(target)) {
      return target.length > 0 ? !(target[0] as Deletable).CanExecuteDelete : true;
    } else {
      return !(target as Deletable).CanExecuteDelete;
    }
  }
}
