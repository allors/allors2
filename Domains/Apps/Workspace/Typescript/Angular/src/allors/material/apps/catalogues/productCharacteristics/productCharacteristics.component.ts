import { Observable, BehaviorSubject, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';

import { TdLoadingService, TdDialogService, TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../meta/index';
import { PullRequest, Query, Predicate, And, Or, Not, Equals, Like, Contains, ContainedIn, TreeNode, Sort, Page } from '../../../../domain';
import { ProductCharacteristic } from '../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved, Invoked } from '../../../../angular';

interface SearchData {
  name: string;
}

@Component({
  templateUrl: './productCharacteristics.component.html',
})
export class ProductCharacteristicsComponent implements AfterViewInit, OnDestroy {

  private refresh$: BehaviorSubject<Date>;
  private page$: BehaviorSubject<number>;

  private subscription: Subscription;
  private scope: Scope;

  title: string = 'Product Characteristics';
  total: number;
  searchForm: FormGroup;
  data: ProductCharacteristic[];
  filtered: ProductCharacteristic[];
  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private snackBar: MdSnackBar,
    private router: Router,
    private dialogService: TdDialogService,
    public media: TdMediaService) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      name: [''],
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

        if (data.name) {
          const like: string = data.name.replace('*', '%') + '%';
          predicates.push(new Like({ roleType: m.ProductCharacteristic.Name, value: like }));
        }

        const query: Query[] = [new Query(
          {
            name: 'productCharacteristics',
            objectType: m.ProductCharacteristic,
            predicate: predicate,
            page: new Page({ skip: 0, take: take }),
            include: [
              new TreeNode({ roleType: m.ProductCharacteristic.LocalisedNames }),
            ],
          })];

        return this.scope.load('Pull', new PullRequest({ query: query }));

      })
      .subscribe((loaded: Loaded) => {
        this.data = loaded.collections.productCharacteristics as ProductCharacteristic[];
        this.total = loaded.values.productCharacteristics_total;
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
    this.titleService.setTitle('Product Characteristics');
    this.media.broadcast();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  delete(productCharacteristic: ProductCharacteristic): void {
    this.dialogService
      .openConfirm({ message: 'Are you sure you want to delete this characteristic?' })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          // TODO: Logical, physical or workflow delete
        }
      });
  }

  onView(productCharacteristic: ProductCharacteristic): void {
    this.router.navigate(['/catalogues/productCharacteristics/' + productCharacteristic.id + '/edit']);
  }
}
