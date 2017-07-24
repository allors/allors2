import { Observable, BehaviorSubject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild , ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { MdSnackBar } from '@angular/material';

import { TdLoadingService, TdDialogService, TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../meta';
import { PullRequest, Query, Predicate, And, Or, Not, Equals, Like, Contains, ContainedIn, TreeNode, Sort, Page } from '../../../../domain';
import { Request } from '../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved } from '../../../../angular';

interface SearchData {
  requestNumber: string;
  company: string;
  description: string;
}

@Component({
  templateUrl: './requests.component.html',
})
export class RequestsComponent implements AfterViewInit, OnDestroy {

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  private page$: BehaviorSubject<number>;
  total: number;

  searchForm: FormGroup;

  title: string = 'Requests';
  data: Request[];
  filtered: Request[];

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private router: Router,
    public dialogService: TdDialogService,
    public media: TdMediaService,
    private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      requestNumber: [''],
      company: [''],
      description: [''],
    });

    this.page$ = new BehaviorSubject<number>(50);

    const search$: Observable<SearchData> = this.searchForm.valueChanges
      .debounceTime(400)
      .distinctUntilChanged()
      .startWith({});

    const combined$: Observable<any> = Observable.combineLatest(search$, this.page$)
      .scan(([previousData, previousTake]: [SearchData, number], [data, take]: [SearchData, number]): [SearchData, number] => {
        return [
          data,
          data !== previousData ? 50 : take,
        ];
      }, [] as [SearchData, number]);

    this.subscription = combined$
      .switchMap(([data, take]: [SearchData, number]) => {
        const m: MetaDomain = this.allors.meta;

        const predicate: And = new And();
        const predicates: Predicate[] = predicate.predicates;

        if (data.requestNumber) {
          const like: string = '%' + data.requestNumber + '%';
          predicates.push(new Like({ roleType: m.Request.RequestNumber, value: like }));
        }

        if (data.company) {
          const partyQuery: Query = new Query({
            objectType: m.Party, predicate: new Like({
              roleType: m.Party.PartyName, value: data.company.replace('*', '%') + '%',
            }),
          });

          const containedIn: ContainedIn = new ContainedIn({ roleType: m.Request.Originator, query: partyQuery });
          predicates.push(containedIn);
        }

        if (data.description) {
          const like: string = data.description.replace('*', '%') + '%';
          predicates.push(new Like({ roleType: m.Request.Description, value: like }));
        }

        const query: Query[] = [new Query(
          {
            name: 'requests',
            objectType: m.Request,
            predicate: predicate,
            page: new Page({ skip: 0, take: take }),
            include: [
              new TreeNode({ roleType: m.Request.Originator }),
              new TreeNode({ roleType: m.Request.CurrentObjectState }),
            ],
            sort: [new Sort({ roleType: m.Request.RequestNumber, direction: 'Desc' })],
          })];

        return this.scope.load('Pull', new PullRequest({ query: query }));

      })
      .subscribe((loaded: Loaded) => {
        this.data = loaded.collections.requests as Request[];
        this.total = loaded.values.requests_total;
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      });
  }

  more(): void {
    this.page$.next(this.data.length + 50);
  }

  goBack(): void {
    this.router.navigate(['/']);
  }

  ngAfterViewInit(): void {
    this.titleService.setTitle('Requests');
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  onView(request: Request): void {
    this.router.navigate(['/orders/requests/' + request.id + '/edit']);
  }
}
