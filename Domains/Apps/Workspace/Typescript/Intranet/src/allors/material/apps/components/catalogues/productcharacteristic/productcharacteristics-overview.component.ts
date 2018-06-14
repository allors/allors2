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
import { SerialisedInventoryItemCharacteristicType } from '../../../../../domain';
import { And, Like, Page, Predicate, PullRequest, Query, Sort, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

interface SearchData {
  name: string;
}

@Component({
  templateUrl: './productcharacteristics-overview.component.html',
})
export class ProductCharacteristicsOverviewComponent implements OnInit, OnDestroy {

  public title = 'Product Characteristics';
  public total: number;
  public searchForm: FormGroup; public advancedSearch: boolean;
  public data: SerialisedInventoryItemCharacteristicType[];
  public filtered: SerialisedInventoryItemCharacteristicType[];

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

    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      name: [''],
    });
  }

  ngOnInit(): void {
    const search$ = this.searchForm.valueChanges
      .debounceTime(400)
      .distinctUntilChanged()
      .startWith({});

    const combined$ = Observable.combineLatest(search$, this.refresh$)
      .scan(([previousData, previousDate], [data, date]) => {
        return [data, date];
      }, [] as [SearchData, Date]);

    this.subscription = combined$
      .switchMap(([data]) => {
        const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

        const predicate: And = new And();
        const predicates: Predicate[] = predicate.predicates;

        if (data.name) {
          const like: string = data.name.replace('*', '%') + '%';
          predicates.push(new Like({ roleType: m.SerialisedInventoryItemCharacteristicType.Name, value: like }));
        }

        const queries: Query[] = [new Query(
          {
            include: [
              new TreeNode({ roleType: m.SerialisedInventoryItemCharacteristicType.LocalisedNames }),
              new TreeNode({ roleType: m.SerialisedInventoryItemCharacteristicType.UnitOfMeasure }),
            ],
            name: 'productCharacteristics',
            objectType: m.SerialisedInventoryItemCharacteristicType,
            predicate,
            sort: [new Sort({ roleType: m.SerialisedInventoryItemCharacteristicType.Name, direction: 'Asc' })],
          })];

        return this.scope.load('Pull', new PullRequest({ queries }));

      })
      .subscribe((loaded) => {
        this.data = loaded.collections.productCharacteristics as SerialisedInventoryItemCharacteristicType[];
        this.total = loaded.values.productCharacteristics_total;
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

  public delete(productCharacteristic: SerialisedInventoryItemCharacteristicType): void {
    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this characteristic?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          // TODO: Logical, physical or workflow delete
        }
      });
  }

  public onView(productCharacteristic: SerialisedInventoryItemCharacteristicType): void {
    this.router.navigate(['/productCharacteristic/' + productCharacteristic.id]);
  }
}
