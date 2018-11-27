import { Component, OnDestroy, Self } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, SessionService } from '../../../../angular';
import { InternalOrganisation, Person, Priority, WorkEffortState, WorkTask, WorkEffortPartyAssignment } from '../../../../domain';
import { And, ContainedIn, Equals, Like, Predicate, PullRequest, Sort, Filter } from '../../../../framework';
import { MetaDomain } from '../../../../meta';
import { StateService } from '../../services/state';
import { Fetcher } from '../Fetcher';
import { AllorsMaterialDialogService } from '../../../base/services/dialog';
import { debounceTime, distinctUntilChanged, startWith, scan, switchMap } from 'rxjs/operators';


@Component({
  templateUrl: './workeffortassignments-overview.component.html',
  providers: [SessionService]
})
export class WorkEffortAssignmentsOverviewComponent implements OnDestroy {
  public m: MetaDomain;

  public total: number;

  public title = 'Work Task Assignements';

  public searchForm: FormGroup; public advancedSearch: boolean;

  public data: WorkEffortPartyAssignment[];

  public workEffortStates: WorkEffortState[];
  public selectedWorkEffortState: WorkEffortState;
  public workEffortState: WorkEffortState;

  public priorities: Priority[];
  public selectedPriority: Priority;
  public priority: Priority;

  public assignees: Person[];
  public selectedAssignee: Person;
  public assignee: Person;

  private refresh$: BehaviorSubject<Date>;
  private fetcher: Fetcher;
  private subscription: Subscription;

  private page$: BehaviorSubject<number>;

  constructor(
    @Self() private allors: SessionService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private titleService: Title,
    private router: Router,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    titleService.setTitle(this.title);
    this.m = this.allors.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.allors.pull);

    this.searchForm = this.formBuilder.group({
      assignee: [''],
      description: [''],
      name: [''],
      priority: [''],
      state: [''],
    });

    this.page$ = new BehaviorSubject<number>(50);

    const search$ = this.searchForm.valueChanges
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
        startWith({}),
      );

    const combined$ = combineLatest(search$, this.page$, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        scan(([previousData], [data, take, date, internalOrganisationId]) => {
          return [
            data,
            data !== previousData ? 50 : take,
            date,
            internalOrganisationId,
          ];
        }, [])
      );

    const { m, pull, x } = this.allors;

    this.subscription = combined$
      .pipe(
        switchMap(([data]) => {

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Organisation({
              predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }),
              sort: new Sort(m.Organisation.PartyName),
            }),
            pull.Organisation(),
            pull.WorkEffortState({
              sort: new Sort(m.WorkEffortState.Name),
            }),
            pull.Priority({
              predicate: new Equals({ propertyType: m.Priority.IsActive, value: true }),
              sort: new Sort(m.Priority.Name),
            })
          ];

          return this.allors
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              switchMap((loaded) => {
                this.workEffortStates = loaded.collections.workEffortStates as WorkEffortState[];
                this.workEffortState = this.workEffortStates.find((v: WorkEffortState) => v.Name === data.state);

                this.priorities = loaded.collections.priorities as Priority[];
                this.priority = this.priorities.find((v: Priority) => v.Name === data.priority);

                const internalOrganisation: InternalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;
                this.assignees = internalOrganisation.ActiveEmployees;
                this.assignee = this.assignees.find((v: Person) => v.displayName === data.assignee);

                const predicate: And = new And();
                const predicates: Predicate[] = predicate.operands;

                if (data.name) {
                  const like: string = '%' + data.name + '%';
                  predicates.push(new Like({ roleType: m.WorkTask.Name, value: like }));
                }

                if (data.description) {
                  const like: string = '%' + data.description + '%';
                  predicates.push(new Like({ roleType: m.WorkTask.Description, value: like }));
                }

                if (data.state) {
                  predicates.push(new Equals({ propertyType: m.WorkTask.WorkEffortState, object: this.workEffortState }));
                }

                if (data.priority) {
                  predicates.push(new Equals({ propertyType: m.WorkTask.Priority, object: this.priority }));
                }

                const workTasksExtent = new Filter(
                  {
                    name: 'worktasks',
                    objectType: m.WorkTask,
                    predicate,
                  });

                const assignmentPredicate: And = new And();
                const assignmentPredicates: Predicate[] = assignmentPredicate.operands;
                assignmentPredicates.push(new ContainedIn({ propertyType: m.WorkEffortPartyAssignment.Assignment, extent: workTasksExtent }));

                if (data.assignee) {
                  assignmentPredicates.push(new Equals({ propertyType: m.WorkEffortPartyAssignment.Party, object: this.assignee }));
                }

                const assignmentsQuery = [
                  pull.WorkEffortPartyAssignment({
                    predicate: assignmentPredicate,
                    include: {
                      Party: x,
                      Assignment: {
                        WorkEffortState: x,
                        Priority: x,
                      }
                    },
                  })
                ];

                return this.allors
                  .load('Pull', new PullRequest({ pulls: assignmentsQuery }));
              })
            );
        })
      )
      .subscribe((loaded) => {

        this.allors.session.reset();

        this.data = loaded.collections.workEffortAssignments as WorkEffortPartyAssignment[];
        this.total = loaded.values.workEffortAssignments_total;
      }, this.errorService.handler);
  }

  public more(): void {
    this.page$.next(this.data.length + 50);
  }

  public goBack(): void {
    window.history.back();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public delete(worktask: WorkTask): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this work task?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.allors.invoke(worktask.Delete)
            .subscribe(() => {
              this.snackBar.open('Successfully deleted.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  public onView(person: Person): void {
    this.router.navigate(['/relations/person/' + person.id]);
  }
}
