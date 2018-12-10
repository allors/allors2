import { MatSnackBar } from '@angular/material';
import { RefreshService } from './../../../../../../angular/base/refresh/refresh.service';
import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, MetaService, ErrorService } from '../../../../../../angular';
import { PhoneCommunication } from '../../../../../../domain';
import { MetaDomain } from '../../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'phonecommunication-overview-summary',
  templateUrl: './phonecommunication-overview-summary.component.html',
  providers: [PanelService]
})
export class PhoneCommunicationOverviewSummaryComponent {

  m: MetaDomain;

  phoneCommunication: PhoneCommunication;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public errorService: ErrorService,
    public navigation: NavigationService,
    public snackBar: MatSnackBar) {

    this.m = this.metaService.m;

    panel.name = 'summary';
    panel.title = 'Summary';
    panel.icon = 'phone';
    panel.expandable = true;

    // Minimized
    const pullName = `${panel.name}_${this.m.Person.objectType.name}`;

    panel.onPull = (pulls) => {
      const { m, pull, tree, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.PhoneCommunication({
          name: pullName,
          object: id,
          include: {
            FromParties: x,
                ToParties: x,
                EventPurposes: x,
                CommunicationEventState: x,
                ContactMechanisms: x,
                WorkEfforts: {
                  WorkEffortState: x,
                  Priority: x
                }
          }
        })
      );
    };

    panel.onPulled = (loaded) => {
      this.phoneCommunication = loaded.objects[pullName] as PhoneCommunication;
    };
  }


  public cancel(): void {

    this.panel.manager.context.invoke(this.phoneCommunication.Cancel)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public close(): void {

      this.panel.manager.context.invoke(this.phoneCommunication.Close)
        .subscribe(() => {
          this.refreshService.refresh();
          this.snackBar.open('Successfully closed.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
  }

  public reopen(): void {

    this.panel.manager.context.invoke(this.phoneCommunication.Reopen)
        .subscribe(() => {
          this.refreshService.refresh();
          this.snackBar.open('Successfully reopened.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
  }

}
