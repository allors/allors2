import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Location } from '@angular/common';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar, MatTableDataSource, MatSort } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Invoked, MediaService, Scope, WorkspaceService, DataService, x } from '../../../../../../angular';
import { Person } from '../../../../../../domain';
import { And, Like, PullRequest, Sort } from '../../../../../../framework';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { debounceTime, distinctUntilChanged, startWith, scan, switchMap } from 'rxjs/operators';
import { SelectionModel } from '@angular/cdk/collections';

interface SearchData {
  firstName: String;
  lastName: String;
}

@Component({
  templateUrl: './person-list.component.html',
})
export class PersonListComponent implements OnInit, OnDestroy {

  displayedColumns = ['select', 'name', 'email', 'phone', 'lastModified', 'menu'];
  dataSource = new MatTableDataSource<Person>();
  selection = new SelectionModel<Person>(true, []);

  public title = 'People';
  public total: number;
  public searchForm: FormGroup; public advancedSearch: boolean;

  public data: Person[];

  @ViewChild(MatSort) sort: MatSort;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    public mediaService: MediaService,
    public router: Router,
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    private location: Location,
    titleService: Title) {

    titleService.setTitle(this.title);
    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      firstName: [''],
      lastName: [''],
    });
  }

  public ngOnInit(): void {

    this.dataSource.sort = this.sort;
    this.dataSource.sortingDataAccessor = ((item: Person, id: string) => {
      switch (id) {
        case 'name':
          return item.displayName;
      }
    });

    const { m, pull } = this.dataService;

    const search$ = this.searchForm.valueChanges
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
        startWith({} as SearchData),
      );

    this.subscription = combineLatest(search$, this.refresh$)
      .pipe(
        switchMap(([data, refresh]) => {
          const predicate: And = new And();

          if (data.firstName) {
            // TODO: should be a shared function
            const like: string = '%' + data.firstName + '%';
            predicate.operands.push(new Like({ roleType: m.Person.FirstName, value: like }));
          }

          if (data.lastName) {
            // TODO: should be a shared function
            const like: string = data.lastName.replace('*', '%') + '%';
            predicate.operands.push(new Like({ roleType: m.Person.LastName, value: like }));
          }

          const pulls = [
            pull.Person({
              predicate,
              include: {
                Salutation: x,
                Picture: x,
                GeneralPhoneNumber: x,
              },
              sort: new Sort(m.Person.PartyName),
            })];

          return this.scope.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.scope.session.reset();
        this.data = loaded.collections.People as Person[];
        this.total = loaded.values.People_total;

        this.dataSource.data = this.data;
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

  /** Whether the number of selected elements matches the total number of rows. */
  public isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
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

  public delete(person: Person): void {
    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this person?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(person.Delete)
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
