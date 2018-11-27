import { Component, OnDestroy, Self, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Location } from '@angular/common';
import { MatTableDataSource, PageEvent, MatSnackBar } from '@angular/material';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';
import { scan, switchMap } from 'rxjs/operators';

import { AllorsFilterService, ErrorService, SessionService, NavigationService, MediaService } from '../../../../../angular';
import { InternalOrganisation, WorkTask } from '../../../../../domain';
import { And, ContainedIn, Equals, Like, Predicate, PullRequest, Sort } from '../../../../../framework';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { SelectionModel } from '@angular/cdk/collections';
import { Sorter } from '../../../../base/sorting';

interface Row {
  workTask: WorkTask;
  name: string;
  state: string;
  description: string;
  priority: string;
  createdBy: string;
  lastModifiedDate: Date;
}

@Component({
  templateUrl: './worktasks-list.component.html',
  providers: [SessionService, AllorsFilterService]
})
export class WorkTasksOverviewComponent implements OnInit, OnDestroy {

  public searchForm: FormGroup; public advancedSearch: boolean;

  public title = 'Work Tasks';

  public displayedColumns = ['select', 'name', 'state', 'description', 'priority', 'createdBy', 'lastModifiedDate', 'menu'];
  public selection = new SelectionModel<Row>(true, []);

  public total: number;
  public dataSource = new MatTableDataSource<Row>();

  private sort$: BehaviorSubject<Sort>;
  private refresh$: BehaviorSubject<Date>;
  private pager$: BehaviorSubject<PageEvent>;

  private subscription: Subscription;

  constructor(
    @Self() public allors: SessionService,
    @Self() private filterService: AllorsFilterService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    private errorService: ErrorService,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    private location: Location,
    titleService: Title) {

    titleService.setTitle(this.title);

    this.sort$ = new BehaviorSubject<Sort>(undefined);
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.pager$ = new BehaviorSubject<PageEvent>(Object.assign(new PageEvent(), { pageIndex: 0, pageSize: 50 }));
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.allors;

    const predicate = new And([
      new Like({ roleType: m.WorkTask.Name, parameter: 'name' }),
      new Like({ roleType: m.WorkTask.Description, parameter: 'description' }),
      // TODO: State, Priority
    ]);

    this.filterService.init(predicate);

    const sorter = new Sorter(
      {
        name: m.WorkTask.Name,
        lastModifiedDate: m.Person.LastModifiedDate,
      }
    );

    this.subscription = combineLatest(this.refresh$, this.filterService.filterFields$, this.sort$, this.pager$)
      .pipe(
        scan(([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent]) => {
          return [
            refresh,
            filterFields,
            sort,
            (previousRefresh !== refresh || filterFields !== previousFilterFields) ? Object.assign({ pageIndex: 0 }, pageEvent) : pageEvent,
          ];
        }, []),
        switchMap(([, filterFields, sort, pageEvent]) => {

          const pulls = [
            pull.WorkTask({
              predicate,
              sort: sorter.create(sort),
              include: {
                WorkEffortState: x,
                Priority: x,
                CreatedBy: x,
              },
              arguments: this.filterService.arguments(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            })];

          return this.allors.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.session.reset();
        this.total = loaded.values.People_total;
        const workTasks = loaded.collections.WorkTasks as WorkTask[];

        this.dataSource.data = workTasks.map((v) => {
          return {
            workTask: v,
            name: v.Name,
            state: v.WorkEffortState && v.WorkEffortState.Name,
            description: v.Description,
            priority: v.Priority && v.Priority.Name,
            createdBy: v.CreatedBy && v.CreatedBy.displayName,
            lastModifiedDate: v.LastModifiedDate
          } as Row;
        });
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public get hasSelection() {
    return !this.selection.isEmpty();
  }

  public get selectedPeople() {
    return this.selection.selected.map(v => v.workTask);
  }

  public isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  public masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.dataSource.data.forEach(row => this.selection.select(row));
  }

  public goBack(): void {
    this.location.back();
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public sort(event: Sort): void {
    this.sort$.next(event);
  }

  public page(event: PageEvent): void {
    this.pager$.next(event);
  }
}
