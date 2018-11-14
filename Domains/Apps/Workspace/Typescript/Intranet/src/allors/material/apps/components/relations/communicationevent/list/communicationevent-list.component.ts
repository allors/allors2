import { Component, OnDestroy, OnInit, ViewChild, Self } from '@angular/core';
import { MatSnackBar, PageEvent, MatTableDataSource } from '@angular/material';
import { Title } from '@angular/platform-browser';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';
import { scan, switchMap } from 'rxjs/operators';

import { ErrorService, Invoked, x, Allors, NavigationService } from '../../../../../../angular';
import { AllorsFilterService } from '../../../../../../angular/base/filter';
import { Sorter } from '../../../../../base/sorting';
import { CommunicationEvent } from '../../../../../../domain';
import { And, Like, PullRequest, Sort } from '../../../../../../framework';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { SelectionModel } from '@angular/cdk/collections';

interface Row {
  communicationEvent: CommunicationEvent;
  name: string;
  state: string;
  subject: string;
  involved: string;
  started: Date;
  ended: Date;
  lastModifiedDate: Date;
}

@Component({
  templateUrl: './communicationevent-list.component.html',
  providers: [Allors, AllorsFilterService]
})
export class CommunicationEventListComponent implements OnInit, OnDestroy {

  public title = 'Communications';

  public displayedColumns = ['select', 'state', 'subject', 'involved', 'started', 'ended', 'lastModifiedDate', 'menu'];
  public selection = new SelectionModel<Row>(true, []);

  public total: number;
  public dataSource = new MatTableDataSource<Row>();

  private sort$: BehaviorSubject<Sort>;
  private refresh$: BehaviorSubject<Date>;
  private pager$: BehaviorSubject<PageEvent>;

  private subscription: Subscription;

  constructor(
    @Self() private allors: Allors,
    @Self() private filterService: AllorsFilterService,
    public navigation: NavigationService,
    private errorService: ErrorService,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    titleService: Title,
  ) {

    titleService.setTitle(this.title);

    this.sort$ = new BehaviorSubject<Sort>(undefined);
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.pager$ = new BehaviorSubject<PageEvent>(Object.assign(new PageEvent(), { pageIndex: 0, pageSize: 50 }));
  }

  ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    const predicate = new And([
      new Like({ roleType: m.Person.FirstName, parameter: 'firstName' }),
      new Like({ roleType: m.Person.LastName, parameter: 'lasttName' }),
    ]);

    this.filterService.init(predicate);

    const sorter = new Sorter(
      {
        name: [m.Person.FirstName, m.Person.LastName],
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
        switchMap(([refresh, filterFields, sort, pageEvent]) => {

          const pulls = [
            pull.CommunicationEvent({
              predicate,
              sort: sorter.create(sort),
              include: {
                CommunicationEventState: x,
                FromParties: x,
                ToParties: x,
                InvolvedParties: x,
              },
              arguments: this.filterService.arguments(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            })];

          return scope.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        scope.session.reset();
        this.total = loaded.values.CommunicationEvents_total;
        const communicationEvents = loaded.collections.CommunicationEvents as CommunicationEvent[];

        this.dataSource.data = communicationEvents.map((v) => {
          return {
            communicationEvent: v,
            state: v.CommunicationEventState && v.CommunicationEventState.Name,
            subject: v.Subject,
            involved: v.InvolvedParties.map((w) => w.displayName).join(', '),
            started: v.ActualStart,
            ended: v.ActualEnd,
            lastModifiedDate: v.LastModifiedDate,
          } as Row;
        });
      },this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
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

  public get hasSelection() {
    return !this.selection.isEmpty();
  }

  public get hasDeleteSelection() {
    return this.selectedPeople.filter((v) => v.CanExecuteDelete).length > 0;
  }

  public get selectedPeople() {
    return this.selection.selected.map(v => v.communicationEvent);
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

  public addNew() {
    // TODO:
  }

  public cancel(communicationEvent: CommunicationEvent): void {
    const { scope } = this.allors;

    this.dialogService
      .confirm({ message: 'Are you sure you want to cancel this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          scope.invoke(communicationEvent.Cancel)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  public close(communicationEvent: CommunicationEvent): void {
    const { scope } = this.allors;

    this.dialogService
      .confirm({ message: 'Are you sure you want to close this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          scope.invoke(communicationEvent.Close)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully closed.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  public reopen(communicationEvent: CommunicationEvent): void {
    const { scope } = this.allors;

    this.dialogService
      .confirm({ message: 'Are you sure you want to reopen this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          scope.invoke(communicationEvent.Reopen)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully reopened.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  public delete(communicationEvent: CommunicationEvent): void {
    const { scope } = this.allors;

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this communication event?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          scope.invoke(communicationEvent.Delete)
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
}
