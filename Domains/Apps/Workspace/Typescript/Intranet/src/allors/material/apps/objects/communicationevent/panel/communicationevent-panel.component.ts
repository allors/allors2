import { Component, Self } from '@angular/core';

import { ContextService, PanelService, Action, RefreshService, NavigationService, ErrorService, Invoked, MetaService } from '../../../../../angular';
import { CommunicationEvent } from '../../../../../domain';
import { MetaDomain } from '../../../../../meta';
import { DeleteService, AllorsMaterialDialogService } from '../../../../../material';
import { MatSnackBar } from '@angular/material';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'communicationevent-panel',
  templateUrl: './communicationevent-panel.component.html',
  providers: [PanelService]
})
export class CommunicationeventPanelComponent {

  m: MetaDomain;

  delete: Action;

  communicationEvents: CommunicationEvent[];

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    public errorService: ErrorService,
    public deleteService: DeleteService,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
  ) {

    this.m = this.metaService.m;
    this.delete = deleteService.delete(panel.container.context);

    panel.name = 'communicationevent';
    panel.title = 'Communication Events';
    panel.icon = 'chat';
    panel.maximizable = true;

    const communicationEventsPullName = `${panel.name}_${this.m.CommunicationEvent.objectType.name}`;

    panel.onPull = (pulls) => {
      const { m, pull, tree, x } = this.metaService;

      const id = this.panel.container.id;

      pulls.push(
        pull.Party({
          name: communicationEventsPullName,
          object: id,
          fetch: {
            CommunicationEventsWhereInvolvedParty: {
              include: {
                CommunicationEventState: x,
                FromParties: x,
                ToParties: x,
                InvolvedParties: x,
              }
            }
          }
        }),
      );
    };

    panel.onPulled = (loaded) => {
        this.communicationEvents = loaded.collections[communicationEventsPullName] as CommunicationEvent[];
      };
  }

  cancel(communicationEvent: CommunicationEvent): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to cancel this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.panel.container.context.invoke(communicationEvent.Cancel)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
              this.refreshService.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  close(communicationEvent: CommunicationEvent): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to close this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.panel.container.context.invoke(communicationEvent.Close)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully closed.', 'close', { duration: 5000 });
              this.refreshService.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  reopen(communicationEvent: CommunicationEvent): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to reopen this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.panel.container.context.invoke(communicationEvent.Reopen)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully reopened.', 'close', { duration: 5000 });
              this.refreshService.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }
}
