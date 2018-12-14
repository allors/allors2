import { MatSnackBar } from '@angular/material';
import { RefreshService } from './../../../../../../angular/base/refresh/refresh.service';
import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, MetaService, ErrorService } from '../../../../../../angular';
import { EmailCommunication } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'emailcommunication-overview-summary',
  templateUrl: './emailcommunication-overview-summary.component.html',
  providers: [PanelService]
})
export class EmailCommunicationOverviewSummaryComponent {

  m: Meta;

  emailCommunication: EmailCommunication;

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
    panel.icon = 'email';
    panel.expandable = true;

    // Minimized
    const pullName = `${panel.name}_${this.m.Person.name}`;

    panel.onPull = (pulls) => {
      const { m, pull, tree, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.EmailCommunication({
          name: pullName,
          object: id,
          include: {
            Originator: x,
            Addressees: x,
            EmailTemplate: x,
            EventPurposes: x,
            CommunicationEventState: x,
            ContactMechanisms: x,
            LastModifiedBy: x,
            WorkEfforts: {
              WorkEffortState: x,
              Priority: x,
            }
          }
        })
      );
    };

    panel.onPulled = (loaded) => {
      this.emailCommunication = loaded.objects[pullName] as EmailCommunication;
    };
  }


  public cancel(): void {

    this.panel.manager.context.invoke(this.emailCommunication.Cancel)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public close(): void {

      this.panel.manager.context.invoke(this.emailCommunication.Close)
        .subscribe(() => {
          this.refreshService.refresh();
          this.snackBar.open('Successfully closed.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
  }

  public reopen(): void {

    this.panel.manager.context.invoke(this.emailCommunication.Reopen)
        .subscribe(() => {
          this.refreshService.refresh();
          this.snackBar.open('Successfully reopened.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
  }

}
