import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
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
export class ProductTypesOverviewComponent implements OnInit, OnDestroy {

  public title = 'Product Types';
  public total: number;
  public searchForm: FormGroup; public advancedSearch: boolean;
  public data: ProductType[];
  public filtered: ProductType[];

  private refresh$: BehaviorSubject<Date>;

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

      titleService.setTitle(this.title);

    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      name: [''],
    });
  }

  ngOnInit(): void {
    const search$: Observable<SearchData> = this.searchForm.valueChanges
      .debounceTime(400)
      .distinctUntilChanged()
      .startWith({});

    const combined$: Observable<any> = Observable.combineLatest(search$, this.refresh$)
      .scan(([previousData, previousDate], [data, date]) => {
        return [data,date];
      }, [] as [SearchData, Date]);

    this.subscription = combined$
      .switchMap(([data]) => {
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

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goBack(): void {
    window.history.back();
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
