import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Saved, Scope, x, Allors, NavigationService, NavigationActivatedRoute, SearchFactory } from '../../../../../../angular';
import { Facility, InternalOrganisation, Locale, Organisation, Ownership, ProductType, SerialisedItem, Part } from '../../../../../../domain';
import { Equals, PullRequest, Sort } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { StateService } from '../../../../services/StateService';
import { Fetcher } from '../../../Fetcher';
import { switchMap, map } from 'rxjs/operators';

@Component({
  templateUrl: './serialiseditem.component.html',
  providers: [Allors]
})
export class EditSerialisedItemComponent implements OnInit, OnDestroy {

  m: MetaDomain;
  scope: Scope;
  item: SerialisedItem;

  add: boolean;
  edit: boolean;

  title: string;
  subTitle: string;
  facility: Facility;
  locales: Locale[];
  suppliers: Organisation[];
  ownerships: Ownership[];
  organisations: Organisation[];
  organisationFilter: SearchFactory;
  part: Part;
  activeSuppliers: Organisation[];

  private subscription: Subscription;
  private refresh$: BehaviorSubject<Date>;
  private fetcher: Fetcher;

  constructor(
    @Self() private allors: Allors,
    public navigationService: NavigationService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private stateService: StateService,
  ) {

    this.m = this.allors.m;
    this.scope = this.allors.scope;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.allors.pull);

    this.organisationFilter = new SearchFactory({
      objectType: this.m.Organisation,
      roleTypes: [this.m.Organisation.Name],
    });
  }

  public ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([, , internalOrganisationId]) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.param();
          const partId = navRoute.queryParam(m.Part);

          const add = !id;

          const pulls = [
            this.fetcher.locales,
            this.fetcher.internalOrganisation,
            pull.SerialisedItem({
              object: id,
              include: {
                PrimaryPhoto: x,
                Photos: x,
                Ownership: x,
                OwnedBy: x,
                RentedBy: x,
                SuppliedBy: x,
                LocalisedNames: {
                  Locale: x,
                },
                LocalisedDescriptions: {
                  Locale: x,
                },
                LocalisedComments: {
                  Locale: x,
                },
                SerialisedItemCharacteristics: {
                  SerialisedItemCharacteristicType: {
                    UnitOfMeasure: x,
                  },
                  LocalisedValues: {
                    Locale: x,
                  },
                }
              }
            }),
            pull.Part(
              {
                object: partId,
                include: {
                  ProductType: x
                }
              }
            ),
            pull.Ownership({
              sort: new Sort(m.Ownership.Name),
            }),
            pull.SerialisedInventoryItemState({
              predicate: new Equals({ propertyType: m.SerialisedInventoryItemState.IsActive, value: true }),
              sort: new Sort(m.SerialisedInventoryItemState.Name),
            }),
          ];

          return scope
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, add }))
            );
        })
      )
      .subscribe(({ loaded, add }) => {
        scope.session.reset();

        const internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;
        this.facility = internalOrganisation.DefaultFacility;
        this.activeSuppliers = internalOrganisation.ActiveSuppliers as Organisation[];
        this.activeSuppliers = this.activeSuppliers.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));

        this.part = loaded.objects.Part as Part;
        this.ownerships = loaded.collections.Ownerships as Ownership[];
        this.locales = loaded.collections.AdditionalLocales as Locale[];

        if (add) {
          this.add = !(this.edit = false);

          this.item = scope.session.create('SerialisedItem') as SerialisedItem;
          this.part.AddSerialisedItem(this.item);
        } else {
          this.edit = !(this.add = false);
          this.item = loaded.objects.SerialisedItem as SerialisedItem;
        }

      },
        (error: any) => {
          this.errorService.handle(error);
          this.navigationService.back();
        },
      );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public refresh(): void {
    this.refresh$.next(new Date());
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

  public update(): void {
    const { scope } = this.allors;

    scope
      .save()
      .subscribe(() => {
        this.snackBar.open('Successfully saved.', 'close', { duration: 5000 });
        this.goBack();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public goBack(): void {
    window.history.back();
  }
}
