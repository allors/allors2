import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, x, Allors, NavigationActivatedRoute, NavigationService } from '../../../../../../angular';
import { SupplierOffering, Part, RatingType, Ordinal, InternalOrganisation, Organisation, UnitOfMeasure, Currency } from '../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { StateService } from '../../../../services/state';
import { switchMap, map } from 'rxjs/operators';
import { Fetcher } from '../../../Fetcher';

@Component({
  templateUrl: './supplieroffering-edit.component.html',
  providers: [Allors]
})
export class EditSupplierOfferingComponent implements OnInit, OnDestroy {

  title = 'Supplier offering';

  add: boolean;
  edit: boolean;

  m: MetaDomain;

  supplierOffering: SupplierOffering;
  part: Part;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private fetcher: Fetcher;
  ratingTypes: RatingType[];
  preferences: Ordinal[];
  activeSuppliers: Organisation[];
  unitsOfMeasure: UnitOfMeasure[];
  currencies: Currency[];

  constructor(
    @Self() private allors: Allors,
    public navigation: NavigationService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private stateService: StateService,
  ) {

    this.m = this.allors.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.allors.pull);
  }

  public ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([, , internalOrganisationId]) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.param();
          const partId = navRoute.queryParam(m.Part);

          let pulls = [
            this.fetcher.internalOrganisation,
            pull.RatingType({ sort: new Sort(m.RateType.Name) }),
            pull.Ordinal({ sort: new Sort(m.Ordinal.Name) }),
            pull.UnitOfMeasure({ sort: new Sort(m.UnitOfMeasure.Name) }),
            pull.Currency({ sort: new Sort(m.Currency.Name) }),
          ];

          if (!!partId) {
            pulls = [
              ...pulls,
              pull.Part({
                object: partId,
              }),
            ];
          }

          const add = !id;

          if (!add) {
            pulls = [
              ...pulls,
              pull.SupplierOffering({
                object: id,
                include: {
                  Rating: x,
                  Preference: x,
                  Supplier: x,
                  Currency: x,
                  UnitOfMeasure: x
                }
              }),
            ];
          }

          return scope
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, add }))
            );
        })
      )
      .subscribe(({ loaded, add }) => {

        scope.session.reset();

        this.ratingTypes = loaded.collections.RatingTypes as RatingType[];
        this.preferences = loaded.collections.Ordinals as Ordinal[];
        this.unitsOfMeasure = loaded.collections.UnitsOfMeasure as UnitOfMeasure[];
        this.currencies = loaded.collections.Currencies as Currency[];

        this.supplierOffering = loaded.objects.SupplierOffering as SupplierOffering;
        this.part = loaded.objects.Part as Part;

        const internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;
        this.activeSuppliers = internalOrganisation.ActiveSuppliers as Organisation[];

        if (add) {
          this.add = !(this.edit = false);

          this.supplierOffering = scope.session.create('SupplierOffering') as SupplierOffering;

        } else {
          this.edit = !(this.add = false);

          this.supplierOffering = loaded.objects.SupplierOffering as SupplierOffering;
        }
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        },
      );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {
    const { scope } = this.allors;

    scope
      .save()
      .subscribe(() => {
        this.goBack();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
}
