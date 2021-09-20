import { MatSnackBar } from '@angular/material/snack-bar';
import { Subject } from 'rxjs';

import { RefreshService, Context, Invoked } from '@allors/angular/services/core';
import { Deletable } from '@allors/domain/generated';
import { ActionTarget, Action } from '@allors/angular/core';
import { AllorsMaterialDialogService, SaveService } from '@allors/angular/material/services/core';

export class DeleteAction implements Action {
  name = 'delete';

  constructor(
    refreshService: RefreshService,
    dialogService: AllorsMaterialDialogService,
    saveService: SaveService,
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
                },
                saveService.errorHandler);
            }
          });
      }
    };
  }

  result = new Subject<boolean>();

  execute: (target: ActionTarget) => void;

  displayName = () => 'Delete';
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
