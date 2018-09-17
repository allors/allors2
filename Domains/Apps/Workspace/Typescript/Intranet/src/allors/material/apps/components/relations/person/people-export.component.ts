import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';

import * as Papa from 'papaparse';

import { ErrorService, Loaded, Scope, WorkspaceService, DataService, x } from '../../../../../angular';
import { Person } from '../../../../../domain';
import { And, Like, Predicate, PullRequest, Sort, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { debounceTime, distinctUntilChanged, startWith, scan, switchMap } from 'rxjs/operators';

interface SearchData {
  firstName: string;
  lastName: string;
}

@Component({
  templateUrl: './people-export.component.html',
})
export class PeopleExportComponent implements OnDestroy {

  public total: number;

  public title = 'Export People to CSV';
  public subTitle: string;

  public searchForm: FormGroup; public advancedSearch: boolean;

  public data: Person[];
  public csv: string;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  private page$: BehaviorSubject<number>;

  constructor(
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private snackBar: MatSnackBar,
    private router: Router,
    private snackBarService: MatSnackBar,
    private dialogService: AllorsMaterialDialogService) {

    titleService.setTitle(this.title);
    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      firstName: [''],
      lastName: [''],
    });

    this.page$ = new BehaviorSubject<number>(50);

    const search$ = this.searchForm.valueChanges
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
        startWith({}),
      );

    const combined$: Observable<any> = combineLatest(search$, this.page$, this.refresh$)
      .pipe(
        scan(([previousData, previousTake, previousDate], [data, take, date]): [SearchData, number, Date] => {
          return [
            data,
            data !== previousData ? 50 : take,
            date,
          ];
        }, [])
      );


    const { m, pull } = this.dataService;

    this.subscription = combined$
      .pipe(
        switchMap(([data, take]: [SearchData, number]) => {
          const predicate: And = new And();
          const predicates: Predicate[] = predicate.operands;

          if (data.firstName) {
            const like: string = '%' + data.firstName + '%';
            predicates.push(new Like({ roleType: m.Person.FirstName, value: like }));
          }

          if (data.lastName) {
            const like: string = data.lastName.replace('*', '%') + '%';
            predicates.push(new Like({ roleType: m.Person.LastName, value: like }));
          }

          const pulls = [
            pull.Person({
              predicate,
              include: {
                Picture: x,
                GeneralPhoneNumber: x,
              },
              sort: new Sort(m.Person.FirstName),
              skip: 0,
              take
            })];

          return this.scope.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.scope.session.reset();

        this.data = loaded.collections.people as Person[];
        this.total = loaded.values.people_total;

        this.csv = Papa.unparse({
          fields: ['FirstName', 'LastName'],
          data: this.data.map((v: Person) => ([v.FirstName, v.LastName])),
        });
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        });
  }

  public copy(): void {
    const result: boolean = document.execCommand('copy');
    window.getSelection().removeAllRanges();
  }

  public goBack(): void {
    window.history.back();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
