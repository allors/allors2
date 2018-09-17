import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Loaded, Scope, WorkspaceService, DataService, x } from '../../../../../angular';
import { ProductType } from '../../../../../domain';
import { And, Like, Predicate, PullRequest, Sort, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { debounceTime, distinctUntilChanged, startWith, scan, switchMap } from 'rxjs/operators';

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
    private dataService: DataService,
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

    const { m, pull } = this.dataService;

    const search$ = this.searchForm.valueChanges
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
        startWith({}),
      );

    const combined$: Observable<any> = combineLatest(search$, this.refresh$)
      .pipe(
        scan(([previousData, previousDate], [data, date]) => {
          return [data, date];
        }, [])
      );

    this.subscription = combined$
      .pipe(
        switchMap(([data]) => {
          const predicate: And = new And();
          const predicates: Predicate[] = predicate.operands;

          if (data.name) {
            const like: string = data.name.replace('*', '%') + '%';
            predicates.push(new Like({ roleType: m.ProductType.Name, value: like }));
          }

          const pulls = [
            pull.ProductType(
              {
                predicate,
                include: {
                  SerialisedInventoryItemCharacteristicTypes: x,
                },
                sort: new Sort(m.ProductType.Name),
              }
            )
          ];

          return this.scope.load('Pull', new PullRequest({ pulls }));
        })
      )
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
