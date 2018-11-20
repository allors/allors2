import { MethodType } from '../../../../framework';
import { Deletable } from '../../../../domain';
import { ActionTarget, SessionService, Invoked, ErrorService } from '../../../../angular';

import { DeleteService } from './delete.service';

export class DeleteAction {

  method: MethodType;
  name: (target: ActionTarget) => string;
  description: (target: ActionTarget) => string;
  handler: (target: ActionTarget) => void;

  constructor(deleteService: DeleteService, sessionService: SessionService) {
    const { m } = sessionService;

    this.method = m.Deletable.Delete;
    this.handler = (target: ActionTarget) => {

      const objects: Deletable[] = (target instanceof Array ? target.map(v => v.object) : [target.object]) as Deletable[];
      const methods = objects.filter((v) => v.CanExecuteDelete).map((v) => v.Delete);

      if (methods.length > 0) {
        deleteService.dialogService
          .confirm(
            methods.length === 1 ?
              { message: 'Are you sure you want to delete this organisation?' } :
              { message: 'Are you sure you want to delete these organisations?' })
          .subscribe((confirm: boolean) => {
            if (confirm) {
              sessionService.invoke(methods)
                .subscribe((invoked: Invoked) => {
                  deleteService.snackBar.open('Successfully deleted.', 'close', { duration: 5000 });
                  deleteService.refreshService.refresh();
                },
                  (error: Error) => {
                    deleteService.errorService.handle(error);
                  });
            }
          });
      }
    };
  }

}
