import { MatSnackBar } from '@angular/material';
import { Subject } from 'rxjs';

import { Deletable } from '../../../../../domain';
import { Action, ActionTarget, Invoked, Context, RefreshService} from '../../../../../angular';
import { AllorsMaterialDialogService } from '../../dialog';


export class DeleteAction implements Action {

  constructor(
    refreshService: RefreshService,
    dialogService: AllorsMaterialDialogService,
    snackBar: MatSnackBar,
    context: Context) {
    this.execute = (target: ActionTarget) => {

      const deletables = Array.isArray(target) ? target as Deletable[] : [target as Deletable];
      const methods = deletables.filter((v) => v.CanExecuteDelete).map((v) => v.Delete);

      if (methods.length > 0) {
        dialogService
          .confirm(
            methods.length === 1 ?
              { message: `Are you sure you want to delete this ${methods[0].object.objectType.name}?` } :
              { message: `Are you sure you want to delete these objects?` })
          .subscribe((confirm: boolean) => {
            if (confirm) {
              context.invoke(methods)
                .subscribe((invoked: Invoked) => {
                  snackBar.open('Successfully deleted.', 'close', { duration: 5000 });
                  refreshService.refresh();
                  this.result.next(true);
                });
            }
          });
      }
    };
  }

  result = new Subject<boolean>();

  execute: (target: ActionTarget) => void;

  name = () => 'Delete';
  description = () => 'Delete';
  disabled = (target: ActionTarget) => {
    if (Array.isArray(target)) {
      const anyDisabled = (target as Deletable[]).filter(v => !v.CanExecuteDelete);
      return target.length > 0 ? anyDisabled.length > 0 : true;
    } else {
      return !(target as Deletable).CanExecuteDelete;
    }
  }
}
