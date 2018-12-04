import { Component, Self } from '@angular/core';

import { SessionService, AllorsPanelService, Action, RefreshService, NavigationService, ErrorService, Invoked, MetaService } from '../../../../../angular';
import { CommunicationEvent } from '../../../../../domain';
import { MetaDomain } from '../../../../../meta';
import { DeleteService, AllorsMaterialDialogService } from '../../../../../material';
import { MatSnackBar } from '@angular/material';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'communicationevent-panel',
  templateUrl: './communicationevent-panel.component.html',
  providers: [AllorsPanelService]
})
export class CommunicationeventPanelComponent {

  m: MetaDomain;

  delete: Action;

  communicationEvents: CommunicationEvent[];

  constructor(
    public allors: SessionService,
    @Self() public panelService: AllorsPanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    public errorService: ErrorService,
    public deleteService: DeleteService,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
  ) {

    this.m = this.metaService.m;
    this.delete = deleteService.delete(allors);

    panelService.name = 'communicationevent';
    panelService.title = 'Communication Events';
    panelService.icon = 'chat';
    panelService.maximizable = true;

    const communicationEventsPullName = `${panelService.name}_${this.m.CommunicationEvent.objectType.name}`;

    panelService.prePull = (pulls) => {
      const { m, pull, tree, x } = this.metaService;

      const id = this.panelService.panelsService.id;

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

    panelService.postPull = (loaded) => {
        this.communicationEvents = loaded.collections[communicationEventsPullName] as CommunicationEvent[];
      };
  }

  cancel(communicationEvent: CommunicationEvent): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to cancel this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.allors.invoke(communicationEvent.Cancel)
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
          this.allors.invoke(communicationEvent.Close)
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
          this.allors.invoke(communicationEvent.Reopen)
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
