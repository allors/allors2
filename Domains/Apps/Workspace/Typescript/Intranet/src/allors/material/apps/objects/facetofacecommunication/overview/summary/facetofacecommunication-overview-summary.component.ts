import { MatSnackBar } from '@angular/material';
import { RefreshService } from '../../../../../../angular/base/refresh/refresh.service';
import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, MetaService, ErrorService } from '../../../../../../angular';
import { FaceToFaceCommunication } from '../../../../../../domain';
import { MetaDomain } from '../../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'facetofacecommunication-overview-summary',
  templateUrl: './facetofacecommunication-overview-summary.component.html',
  providers: [PanelService]
})
export class FaceToFaceCommunicationOverviewSummaryComponent {

  m: MetaDomain;

  faceToFaceCommunication: FaceToFaceCommunication;

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
    panel.icon = 'meeting_room';
    panel.maximizable = true;

    // Minimized
    const pullName = `${panel.name}_${this.m.FaceToFaceCommunication.objectType.name}`;

    panel.onPull = (pulls) => {
      const { m, pull, tree, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.FaceToFaceCommunication({
          name: pullName,
          object: id,
          include: {
            Owner: x,
            InvolvedParties: x,
            FromParties: x,
            ToParties: x,
            EventPurposes: x,
            CommunicationEventState: x,
            ContactMechanisms: x,
            WorkEfforts: {
              WorkEffortState: x,
              Priority: x,
            }
          }
        })
      );
    };

    panel.onPulled = (loaded) => {
      this.faceToFaceCommunication = loaded.objects[pullName] as FaceToFaceCommunication;
    };
  }

  public cancel(): void {

    this.panel.manager.context.invoke(this.faceToFaceCommunication.Cancel)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public close(): void {

      this.panel.manager.context.invoke(this.faceToFaceCommunication.Close)
        .subscribe(() => {
          this.refreshService.refresh();
          this.snackBar.open('Successfully closed.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
  }

  public reopen(): void {

    this.panel.manager.context.invoke(this.faceToFaceCommunication.Reopen)
        .subscribe(() => {
          this.refreshService.refresh();
          this.snackBar.open('Successfully reopened.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
  }

}
