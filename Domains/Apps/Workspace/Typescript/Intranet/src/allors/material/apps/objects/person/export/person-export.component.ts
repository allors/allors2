import { Component, OnDestroy, Self } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Title } from '@angular/platform-browser';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';

import * as Papa from 'papaparse';

import { ErrorService, SessionService } from '../../../../../angular';
import { Person } from '../../../../../domain';
import { And, Like, Predicate, PullRequest, Sort, TreeNode } from '../../../../../framework';
import { debounceTime, distinctUntilChanged, startWith, scan, switchMap } from 'rxjs/operators';

interface SearchData {
  firstName: string;
  lastName: string;
}

@Component({
  templateUrl: './person-export.component.html',
  providers: [SessionService]
})
export class PersonExportComponent implements OnDestroy {

  public total: number;

  public title = 'Export People to CSV';
  public subTitle: string;

  public searchForm: FormGroup; public advancedSearch: boolean;

  public data: Person[];
  public csv: string;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private page$: BehaviorSubject<number>;

  constructor(
    @Self() private allors: SessionService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    titleService: Title) {

    titleService.setTitle(this.title);
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

    const { m, pull, x } = this.allors;

    this.subscription = combined$
      .pipe(
        switchMap(([data, take]: [SearchData, number]) => {
          const predicate = new And();
          const operands = predicate.operands;

          if (data.firstName) {
            const like = '%' + data.firstName + '%';
            operands.push(new Like({ roleType: m.Person.FirstName, value: like }));
          }

          if (data.lastName) {
            const like = data.lastName.replace('*', '%') + '%';
            operands.push(new Like({ roleType: m.Person.LastName, value: like }));
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

          return this.allors.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.allors.session.reset();

        this.data = loaded.collections.people as Person[];
        this.total = loaded.values.people_total;

        this.csv = Papa.unparse({
          fields: ['FirstName', 'LastName'],
          data: this.data.map((v: Person) => ([v.FirstName, v.LastName])),
        });
      }, this.errorService.handler);
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
