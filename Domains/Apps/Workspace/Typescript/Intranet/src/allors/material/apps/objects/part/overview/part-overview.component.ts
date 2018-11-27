import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Invoked, SessionService, NavigationService, NavigationActivatedRoute, MediaService } from '../../../../../angular';
import { InternalOrganisation, Part, IGoodIdentification, SerialisedItem, BasePrice, PriceComponent, SupplierOffering } from '../../../../../domain';
import { PullRequest, Equals, Sort } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { Fetcher } from '../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './part-overview.component.html',
  providers: [SessionService]
})
export class PartOverviewComponent implements OnInit, OnDestroy {

  m: MetaDomain;

  title = 'Part Overview';
  part: Part;
  internalOrganisation: InternalOrganisation;
  serialised: boolean;
  suppliers: string;
  sellingPrice: BasePrice;
  currentPricecomponents: PriceComponent[] = [];
  inactivePricecomponents: PriceComponent[] = [];
  allPricecomponents: PriceComponent[] = [];
  supplierOfferingsCollection = 'Current';
  allSupplierOfferings: SupplierOffering[];

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private fetcher: Fetcher;
  currentSupplierOfferings: SupplierOffering[];
  inactiveSupplierOfferings: SupplierOffering[];


  constructor(
    @Self() private allors: SessionService,
    public navigationService: NavigationService,
    private errorService: ErrorService,
    private titleService: Title,
    public mediaService: MediaService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    this.titleService.setTitle(this.title);

    this.m = this.allors.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.allors.pull);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([urlSegments, date, internalOrganisationId]) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.param();

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.PriceComponent({
              predicate: new Equals({ propertyType: m.PriceComponent.Part, object: id }),
              include: {
                Part: x,
                Currency: x
              },
              sort: new Sort({ roleType: m.PriceComponent.FromDate, descending: true })
            }),
            pull.Part({
              object: id,
              include: {
                GoodIdentifications: {
                  GoodIdentificationType: x
                },
                ProductType: x,
                InventoryItemKind: x,
                ManufacturedBy: x,
                SerialisedItems: {
                  PrimaryPhoto: x,
                  SerialisedItemState: x,
                  OwnedBy: x
                },
                Brand: x,
                Model: x
              }
            }),
            pull.Part({
              object: id,
              fetch: {
                SupplierOfferingsWherePart: {
                  include: {
                    Currency: x
                  }
                }
              }
            }),
          ];

          return this.allors
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.session.reset();

        const now = new Date();

        this.internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;

        this.part = loaded.objects.Part as Part;
        this.serialised = this.part.InventoryItemKind.UniqueId === '2596E2DD-3F5D-4588-A4A2-167D6FBE3FAE'.toLowerCase();

        this.allPricecomponents = loaded.collections.PriceComponents as PriceComponent[];
        this.currentPricecomponents = this.allPricecomponents.filter(v => v.FromDate <= now && (v.ThroughDate === null || v.ThroughDate >= now));
        this.inactivePricecomponents = this.allPricecomponents.filter(v => v.FromDate > now || (v.ThroughDate !== null && v.ThroughDate < now));

        this.allSupplierOfferings  = loaded.collections.SupplierOfferings as SupplierOffering[];
        this.currentSupplierOfferings = this.allSupplierOfferings.filter(v => v.FromDate <= now && (v.ThroughDate === null || v.ThroughDate >= now));
        this.inactiveSupplierOfferings = this.allSupplierOfferings.filter(v => v.FromDate > now || (v.ThroughDate !== null && v.ThroughDate < now));

        if (this.part.SuppliedBy.length > 0) {
          this.suppliers = this.part.SuppliedBy
            .map(v => v.displayName)
            .reduce((acc: string, cur: string) => acc + ', ' + cur);
        }
      }, this.errorService.handler);
  }

  public deleteGoodIdentification(goodIdentification: IGoodIdentification): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.allors.invoke(goodIdentification.Delete)
            .subscribe(() => {
              this.snackBar.open('Successfully deleted.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  public deletePriceComponent(pricecomponent: PriceComponent): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.allors.invoke(pricecomponent.Delete)
            .subscribe(() => {
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

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public deleteSerialisedItem(item: SerialisedItem): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.allors.invoke(item.Delete)
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

  get offerings(): any {

    switch (this.supplierOfferingsCollection) {
      case 'Current':
        return this.currentSupplierOfferings;
      case 'Inactive':
        return this.inactiveSupplierOfferings;
      case 'All':
      default:
        return this.allSupplierOfferings;
    }
  }
}
