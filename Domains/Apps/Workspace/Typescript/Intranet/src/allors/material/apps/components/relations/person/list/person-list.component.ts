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

interface Row {
  person: Person;
  name: string;
  email: string;
  phone: string;
  lastModifiedDate: Date;
}

const title = 'People';

@Component({
  templateUrl: './person-list.component.html',
})
export class PersonListComponent implements OnInit, OnDestroy {

  public displayedColumns = ['select', 'name', 'email', 'phone', 'lastModified', 'menu'];
  public dataSource = new MatTableDataSource<Row>();
  public selection = new SelectionModel<Row>(true, []);

  public data: Row[];

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

    titleService.setTitle(title);
    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    this.dataSource.sort = this.sort;

    const { m, pull } = this.dataService;

    this.subscription = this.refresh$
      .pipe(
        switchMap((refresh) => {
          const pulls = [
            pull.Person({
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
        const people = loaded.collections.People as Person[];
        this.data = people.map((v) => {
          return {
            person: v,
            name: v.displayName,
            email: v.displayEmail,
            phone: v.displayPhone,
            lastModifiedDate: v.LastModifiedDate,
          } as Row;
        });

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
