import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import 'rxjs/add/observable/combineLatest';



import { ErrorService, Loaded, Scope, WorkspaceService } from '../../../../../angular';
import { ProductType } from '../../../../../domain';
import { And, Like, Page, Predicate, PullRequest, Query, Sort, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

interface SearchData {
  name: string;
}

@Component({
  templateUrl: './producttypes-overview.component.html',
})
export class ProductTypesOverviewComponent implements OnDestroy {

  public title = 'Products';
  public total: number;
  public searchForm: FormGroup;
  public data: ProductType[];
  public filtered: ProductType[];

  private refresh$: BehaviorSubject<Date>;
  private page$: BehaviorSubject<number>;

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private snackBar: MatSnackBar,
    private router: Router,
    private dialogService: AllorsMaterialDialogService) {

    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      name: [''],
    });

    this.page$ = new BehaviorSubject<number>(50);

    const search$: Observable<SearchData> = this.searchForm.valueChanges
      .debounceTime(400)
      .distinctUntilChanged()
      .startWith({});

    const combined$: Observable<any> = Observable.combineLatest(search$, this.page$, this.refresh$)
        .scan(([previousData, previousTake, previousDate], [data, take, date]) => {
          return [
            data,
            data !== previousData ? 50 : take,
            date,
          ];
        }, [] as [SearchData, number, Date]);

    this.subscription = combined$
      .switchMap(([data, take]) => {
        const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

        const predicate: And = new And();
        const predicates: Predicate[] = predicate.predicates;

        if (data.name) {
          const like: string = data.name.replace('*', '%') + '%';
          predicates.push(new Like({ roleType: m.ProductType.Name, value: like }));
        }

        const queries: Query[] = [
          new Query(
          {
            name: 'productTypes',
            objectType: m.ProductType,
            predicate,
            page: new Page({ skip: 0, take }),
            include: [
              new TreeNode({ roleType: m.ProductType.SerialisedInventoryItemCharacteristicTypes }),
            ],
            sort: [new Sort({ roleType: m.ProductType.Name, direction: 'Asc' })],
          }),
        ];

        return this.scope.load('Pull', new PullRequest({ queries }));

      })
      .subscribe((loaded) => {
        this.data = loaded.collections.productTypes as ProductType[];
        this.total = loaded.values.productTypes_total;
      },
      (error: any) => {
        this.errorService.handle(error);
        this.goBack();
      });
  }

  public more(): void {
    this.page$.next(this.data.length + 50);
  }

  public goBack(): void {
    window.history.back();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public delete(productType: ProductType): void {
     this.dialogService
      .confirm({ message: 'Are you sure you want to delete this product type?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          // TODO: Logical, physical or workflow delete
        }
      }); 
  }

  public onView(productType: ProductType): void {
    this.router.navigate(['/productType/' + productType.id]);
  }
}
