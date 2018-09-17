import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';
import { debounceTime, distinctUntilChanged, startWith, scan, switchMap } from 'rxjs/operators';

import { ErrorService, Scope, WorkspaceService, DataService, x } from '../../../../../angular';
import { SerialisedInventoryItemCharacteristicType } from '../../../../../domain';
import { And, Like, Predicate, PullRequest, Sort, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { UnitOfMeasure } from '../../../../../domain/generated/UnitOfMeasure.g';

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

    const combined$ = combineLatest(search$, this.refresh$)
      .pipe(
        scan(([previousData, previousDate], [data, date]) => {
          return [data, date];
        }, [])
      );

    this.subscription = combined$
      .pipe(
        switchMap(([data]) => {
          const predicate = new And();

          if (data.name) {
            const like: string = data.name.replace('*', '%') + '%';
            predicate.operands.push(new Like({ roleType: m.SerialisedInventoryItemCharacteristicType.Name, value: like }));
          }

          const pulls = [
            pull.SerialisedInventoryItemCharacteristicType({
              predicate,
              include: {
                LocalisedNames: x,
                UnitOfMeasure: x,
              },
              sort: new Sort(m.SerialisedInventoryItemCharacteristicType.Name),
            })];

          return this.scope.load('Pull', new PullRequest({ pulls }));

        })
      )
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
