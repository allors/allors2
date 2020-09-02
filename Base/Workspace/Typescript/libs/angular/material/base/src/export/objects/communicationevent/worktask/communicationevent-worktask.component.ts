import { Component, Self, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { MetaService, ContextService, RefreshService, Saved } from '@allors/angular/services/core';
import { Organisation, WorkTask, WorkEffortState, Priority, WorkEffortPurpose, Person, WorkEffortPartyAssignment, CommunicationEvent } from '@allors/domain/generated';
import { InternalOrganisationId } from '@allors/angular/base';
import { Sort, Equals } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { SaveService } from '@allors/angular/material/services/core';

@Component({
  templateUrl: './communicationevent-worktask.component.html',
  providers: [ContextService]
})
export class CommunicationEventWorkTaskComponent implements OnInit, OnDestroy {

  public title = 'Work Task';
  public subTitle: string;

  public m: Meta;

  public workTask: WorkTask;

  public workEffortStates: WorkEffortState[];
  public priorities: Priority[];
  public workEffortPurposes: WorkEffortPurpose[];
  public employees: Person[];
  public workEffortPartyAssignments: WorkEffortPartyAssignment[];
  public assignees: Person[] = [];

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    public metaService: MetaService,
    private saveService: SaveService,
    private route: ActivatedRoute,
    public refreshService: RefreshService,
    private internalOrganisationId: InternalOrganisationId,
    titleService: Title) {

    titleService.setTitle(this.title);

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest([this.route.url, this.refreshService.refresh$, this.internalOrganisationId.observable$])
      .pipe(
        switchMap(([urlSegments, date, internalOrganisationId]) => {

          const id: string = this.route.snapshot.paramMap.get('id');
          const roleId: string = this.route.snapshot.paramMap.get('roleId');

          const pulls = [
            pull.CommunicationEvent({
              object: id,
              include: { CommunicationEventState: x }
            }),
            pull.WorkTask({
              object: roleId,
            }),
            pull.InternalOrganisation({
              object: id,
              include: { ActiveEmployees: x }
            }),
            pull.WorkEffortState({
              sort: new Sort(m.WorkEffortState.Name)
            }),
            pull.Priority({
              predicate: new Equals({ propertyType: m.Priority.IsActive, value: true }),
              sort: new Sort(m.Priority.Name),
            }),
            pull.WorkEffortPurpose({
              predicate: new Equals({ propertyType: m.WorkEffortPurpose.IsActive, value: true }),
              sort: new Sort(m.WorkEffortPurpose.Name),
            }),
            pull.WorkEffortPartyAssignment()
          ];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.subTitle = 'edit work task';
        this.workTask = loaded.objects.Worktask as WorkTask;
        const communicationEvent: CommunicationEvent = loaded.objects.CommunicationEvent as CommunicationEvent;

        if (!this.workTask) {
          this.subTitle = 'add a new work task';
          this.workTask = this.allors.context.create('WorkTask') as WorkTask;
          communicationEvent.AddWorkEffort(this.workTask);
        }

        this.workEffortStates = loaded.collections.WorkEffortStates as WorkEffortState[];
        this.priorities = loaded.collections.Priorities as Priority[];
        this.workEffortPurposes = loaded.collections.WorkEffortPurposes as WorkEffortPurpose[];
        const internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.employees = internalOrganisation.ActiveEmployees;
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.assignees.forEach((assignee: Person) => {
      const workEffortPartyAssignment: WorkEffortPartyAssignment = this.allors.context.create('WorkEffortPartyAssignment') as WorkEffortPartyAssignment;
      workEffortPartyAssignment.Assignment = this.workTask;
      workEffortPartyAssignment.Party = assignee;
    });

    this.allors.context.save()
      .subscribe((saved: Saved) => {
        this.goBack();
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }

  public goBack(): void {
    window.history.back();
  }
}
