import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';

import { ErrorService, Scope, WorkspaceService, x, Invoked, Allors } from '../../../../../angular';
import { SerialisedInventoryItemCharacteristicType } from '../../../../../domain';
import { And, Like, Predicate, PullRequest, Sort } from '../../../../../framework';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { StateService } from '../../../services/StateService';

interface SearchData {
  name?: string;
}

@Component({
  templateUrl: './productcharacteristics-overview.component.html',
  providers: [Allors]
})
export class ProductCharacteristicsOverviewComponent implements OnInit, OnDestroy {

  public title = 'Product Characteristics';
  public characteristicTypes: SerialisedInventoryItemCharacteristicType[];
  public filtered: SerialisedInventoryItemCharacteristicType[];

  public search$: BehaviorSubject<SearchData>;
  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    @Self() private allors: Allors,
    private errorService: ErrorService,
    private titleService: Title,
    private snackBar: MatSnackBar,
    private router: Router,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    titleService.setTitle(this.title);

    this.search$ = new BehaviorSubject<SearchData>({});
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    const search$ = this.search$
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
      );

    this.subscription = combineLatest(search$, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([data, refresh, internalOrganisationId]) => {
          const predicate = new And();
          const predicates: Predicate[] = predicate.operands;

          if (data.name) {
            const like: string = data.name.replace('*', '%') + '%';
            predicates.push(new Like({ roleType: m.SerialisedInventoryItemCharacteristicType.Name, value: like }));
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

          return scope.load('Pull', new PullRequest({ pulls }));

        })
      )
      .subscribe((loaded) => {
        this.characteristicTypes = loaded.collections.SerialisedInventoryItemCharacteristicTypes as SerialisedInventoryItemCharacteristicType[];
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        });
  }

  public delete(characteristicType: SerialisedInventoryItemCharacteristicType): void {
    const { scope } = this.allors;

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this characteristic?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          scope.invoke(characteristicType.Delete)
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

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goBack(): void {
    window.history.back();
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public onView(productCharacteristic: SerialisedInventoryItemCharacteristicType): void {
    this.router.navigate(['/productCharacteristic/' + productCharacteristic.id]);
  }
}
