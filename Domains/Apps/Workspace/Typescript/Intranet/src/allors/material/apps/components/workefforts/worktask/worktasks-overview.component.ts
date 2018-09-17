import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Invoked, Loaded, PdfService, Scope, WorkspaceService, DataService } from '../../../../../angular';
import { InternalOrganisation, Person, Priority, Singleton, WorkEffortAssignment, WorkEffortState, WorkTask } from '../../../../../domain';
import { And, ContainedIn, Equals, Fetch, Like, Predicate, PullRequest, TreeNode, Sort } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { Fetcher } from '../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { debounceTime, distinctUntilChanged, startWith, scan, switchMap } from 'rxjs/operators';

interface SearchData {
  name: string;
  description: string;
  state: string;
  priority: string;
}

@Component({
  templateUrl: './worktasks-overview.component.html',
})
export class WorkTasksOverviewComponent implements OnInit, OnDestroy {
  public m: MetaDomain;

  public total: number;

  public title = 'Work Tasks';

  public searchForm: FormGroup; public advancedSearch: boolean;

  public data: WorkTask[];

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
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private snackBar: MatSnackBar,
    private router: Router,
    private snackBarService: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    public pdfService: PdfService,
    private stateService: StateService) {

    titleService.setTitle(this.title);
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.dataService);

    this.searchForm = this.formBuilder.group({
      assignee: [''],
      description: [''],
      name: [''],
      priority: [''],
      state: [''],
    });

    const search$ = this.searchForm.valueChanges
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
        startWith({}),
      );

    const combined$ = combineLatest(search$, this.refresh$)
      .pipe(
        scan(([previousData, previousDate], [data, date]) => {
          return [data, date];
        }, [])
      );

    this.subscription = combined$
      .pipe(
        switchMap(([data]) => {

          const { m, pull } = this.dataService;

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Organisation({
              predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }),
              sort: new Sort(m.Organisation.PartyName),
            }),
            pull.WorkEffortState({
              sort: new Sort(m.WorkEffortState.Name),
            }),
            pull.Priority({
              predicate: new Equals({ propertyType: m.Priority.IsActive, value: true }),
              sort: new Sort(m.Priority.Name),
            })
          ];

          return this.scope
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
                  predicates.push(new Equals({ propertyType: m.WorkTask.WorkEffortState, value: this.workEffortState }));
                }

                if (data.priority) {
                  predicates.push(new Equals({ propertyType: m.WorkTask.Priority, value: this.priority }));
                }

                const workTasksQuery: Query = new Query(
                  {
                    name: 'worktasks',
                    objectType: m.WorkTask,
                    predicate,
                    include: [
                      new TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                      new TreeNode({ roleType: m.WorkEffort.Priority }),
                      new TreeNode({ roleType: m.WorkEffort.CreatedBy }),
                    ],
                  });

                return this.scope
                  .load('Pull', new PullRequest({ pulls: [workTasksQuery] }));
              })
            );
        })
      )
      .subscribe((loaded) => {

        this.scope.session.reset();

        this.data = loaded.collections.worktasks as WorkTask[];
        this.total = loaded.values.worktasks_total;
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        });
  }

  ngOnInit(): void {
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public print(worktask: WorkTask) {
    this.pdfService.display(worktask);
  }

  public goBack(): void {
    window.history.back();
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public delete(worktask: WorkTask): void {
    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this work task?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(worktask.Delete)
            .subscribe((invoked: Invoked) => {
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
