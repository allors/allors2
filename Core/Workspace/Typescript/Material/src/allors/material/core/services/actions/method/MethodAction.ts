import { MatSnackBar } from '@angular/material/snack-bar';
import { Subject, config } from 'rxjs';

import { Action, ActionTarget, Invoked, Context, RefreshService } from '../../../../../angular';
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
    public config?: MethodConfig) {

    this.execute = (target: ActionTarget) => {

      const objects = this.resolve(target);
      const methods = objects.filter((v) => v.canExecute(methodType)).map((v) => (v as any)[methodType.name]);

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
    const objects = this.resolve(target);
    return objects?.find(v => v.canExecute(this.methodType)) == null;
  }

  private resolve(target: ActionTarget): ISessionObject[] {
    let objects = Array.isArray(target) ? target as ISessionObject[] : [target as ISessionObject];

    if (this.config?.path) {
      objects = objects.map(v => {
        for (const roleType of this.config.path) {
          v = v.get(roleType);
        }

        return v;
      });
    }

    return objects;
  }
}
