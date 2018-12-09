import { MatSnackBar } from '@angular/material';
import { RefreshService } from '../../../../../../angular/base/refresh/refresh.service';
import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, MetaService, ErrorService } from '../../../../../../angular';
import { LetterCorrespondence } from '../../../../../../domain';
import { MetaDomain } from '../../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'lettercorrespondence-overview-summary',
  templateUrl: './lettercorrespondence-overview-summary.component.html',
  providers: [PanelService]
})
export class LetterCorrespondenceOverviewSummaryComponent {

  m: MetaDomain;

  letterCorrespondence: LetterCorrespondence;

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
    panel.maximizable = true;

    // Minimized
    const pullName = `${panel.name}_${this.m.Person.objectType.name}`;

    panel.onPull = (pulls) => {
      const { m, pull, tree, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.LetterCorrespondence({
          name: pullName,
          object: id,
          include: {
            Originators: x,
            Owner: x,
            Receivers: x,
            EventPurposes: x,
            CommunicationEventState: x,
            ContactMechanisms: x,
            WorkEfforts: {
              WorkEffortState: x,
              Priority: x,
            },
            PostalAddresses: {
              PostalBoundary: {
                Country: x,
              }
            }
          }
        })
      );
    };

    panel.onPulled = (loaded) => {
      this.letterCorrespondence = loaded.objects[pullName] as LetterCorrespondence;
    };
  }


  public cancel(): void {

    this.panel.manager.context.invoke(this.letterCorrespondence.Cancel)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public close(): void {

    this.panel.manager.context.invoke(this.letterCorrespondence.Close)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully closed.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public reopen(): void {

    this.panel.manager.context.invoke(this.letterCorrespondence.Reopen)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully reopened.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

}
