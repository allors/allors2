import { Component, OnDestroy, OnInit, ViewChild, Self } from '@angular/core';
import { Location } from '@angular/common';
import { Title } from '@angular/platform-browser';
import { MatSnackBar, MatTableDataSource, MatSort, MatDialog, Sort, PageEvent } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';

import { PullRequest, SessionObject, And, Like, Sort as AllorsSort, RoleType, Extent, Filter } from '../../../../../../framework';
import { ErrorService, Invoked, MediaService, x, Allors, NavigationService } from '../../../../../../angular';
import { AllorsFilterService } from '../../../../../../angular/base/filter';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { Sorter } from '../../../../../base/sorting';

import { Person } from '../../../../../../domain';
import { PersonAddComponent } from '../add/person-add.module';

interface Row {
  person: Person;
  name: string;
  email: string;
  phone: string;
  lastModifiedDate: Date;
}

@Component({
  templateUrl: './person-list.component.html',
  providers: [Allors, AllorsFilterService]
})
export class PersonListComponent implements OnInit, OnDestroy {

  public title = 'People';

  public displayedColumns = ['select', 'name', 'email', 'phone', 'lastModifiedDate', 'menu'];
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
    public mediaService: MediaService,
    private errorService: ErrorService,
    private dialog: MatDialog,
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
            pull.Person({
              predicate,
              sort: sorter.create(sort),
              include: {
                Salutation: x,
                Picture: x,
                GeneralPhoneNumber: x,
                GeneralEmail: x,
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
        this.total = loaded.values.People_total;
        const people = loaded.collections.People as Person[];

        this.dataSource.data = people.map((v) => {
          return {
            person: v,
            name: v.displayName,
            email: v.displayEmail,
            phone: v.displayPhone,
            lastModifiedDate: v.LastModifiedDate,
          } as Row;
        });
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public get hasSelection() {
    return !this.selection.isEmpty();
  }

  public get hasDeleteSelection() {
    return this.selectedPeople.filter((v) => v.CanExecuteDelete).length > 0;
  }

  public get selectedPeople() {
    return this.selection.selected.map(v => v.person);
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

  public delete(person: Person | Person[]): void {

    const { scope } = this.allors;

    const people = person instanceof SessionObject ? [person as Person] : person instanceof Array ? person : [];
    const methods = people.filter((v) => v.CanExecuteDelete).map((v) => v.Delete);

    if (methods.length > 0) {
      this.dialogService
        .confirm(
          methods.length === 1 ?
            { message: 'Are you sure you want to delete this person?' } :
            { message: 'Are you sure you want to delete these people?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            scope.invoke(methods)
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

  public addNew() {
    const dialogRef = this.dialog.open(PersonAddComponent, {
      autoFocus: false,
      disableClose: true
    });
    dialogRef.afterClosed().subscribe(result => {
      this.refresh();
    });
  }
}
