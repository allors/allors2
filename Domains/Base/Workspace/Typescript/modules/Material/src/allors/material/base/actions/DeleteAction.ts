import { MatSnackBar } from '@angular/material';

import { MethodType } from '../../../framework';
import { Deletable } from '../../../domain';
import { ActionTarget, SessionService, Invoked, ErrorService } from '../../../angular';

import { AllorsMaterialDialogService } from '../services/dialog';

export class DeleteAction {

  method: MethodType;
  name: (target: ActionTarget) => string;
  description: (target: ActionTarget) => string;
  handler: (target: ActionTarget) => void;

  constructor(
    private sessionService: SessionService,
    private errorService: ErrorService,
    private dialogService: AllorsMaterialDialogService,
    private snackBar: MatSnackBar,
  ) {
    const { m } = this.sessionService;

    this.method = m.Deletable.Delete;
    this.handler = (target: ActionTarget) => {

      const objects: Deletable[] = (target instanceof Array ? target.map(v => v.object) : [target.object]) as Deletable[];
      const methods = objects.filter((v) => v.CanExecuteDelete).map((v) => v.Delete);

      if (methods.length > 0) {
        this.dialogService
          .confirm(
            methods.length === 1 ?
              { message: 'Are you sure you want to delete this organisation?' } :
              { message: 'Are you sure you want to delete these organisations?' })
          .subscribe((confirm: boolean) => {
            if (confirm) {
              this.sessionService.invoke(methods)
                .subscribe((invoked: Invoked) => {
                  this.snackBar.open('Successfully deleted.', 'close', { duration: 5000 });
                  this.sessionService.refresh();
                },
                  (error: Error) => {
                    this.errorService.handle(error);
                  });
            }
          });
      }
    };
  }

}
