import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Saved, ContextService, NavigationService, NavigationActivatedRoute, SearchFactory, MetaService } from '../../../../../angular';
import { Facility, InternalOrganisation, Locale, Organisation, Ownership, SerialisedItem, Part, SerialisedItemState, Party } from '../../../../../domain';
import { Equals, PullRequest, Sort } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { Fetcher } from '../../Fetcher';
import { switchMap, map } from 'rxjs/operators';

@Component({
  templateUrl: './serialiseditem.component.html',
  providers: [ContextService]
})
export class EditSerialisedItemComponent implements OnInit, OnDestroy {

  m: MetaDomain;
  item: SerialisedItem;

  add: boolean;
  edit: boolean;
  fromPart: boolean;

  title: string;
  subTitle: string;
  facility: Facility;
  locales: Locale[];
  suppliers: Organisation[];
  ownerships: Ownership[];
  organisations: Organisation[];
  organisationFilter: SearchFactory;
  activeSuppliers: Organisation[];
  serialisedItemStates: SerialisedItemState[];
  owner: Party;

  forPart: Part;
  part: Part;
  itemPart: Part;
  parts: Part[];

  private subscription: Subscription;
  private refresh$: BehaviorSubject<Date>;
  private fetcher: Fetcher;

  constructor(
    @Self() public allors: ContextService,
    public metaService: MetaService,
    public navigationService: NavigationService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private stateService: StateService,
  ) {

    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);

    this.organisationFilter = new SearchFactory({
      objectType: this.m.Organisation,
      roleTypes: [this.m.Organisation.Name],
    });
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([, , internalOrganisationId]) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.id();
          const partId = navRoute.queryParam(m.Part);
          const ownerId = navRoute.queryParam(m.Party);

          this.fromPart = partId !== undefined;
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
                SerialisedItemState: x,
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
                  ProductType: x,
                  InventoryItemKind: x
                }
              }
            ),
            pull.SerialisedItem(
              {
                object: id,
                name: 'ItemPart',
                fetch: {
                  PartWhereSerialisedItem: {
                    ProductType: x,
                    InventoryItemKind: x
                  }
                }
              }
            ),
            pull.Party({ object: ownerId }),
            pull.Ownership({ sort: new Sort(m.Ownership.Name) }),
            pull.Part({
              name: 'AllParts',
              sort: new Sort(m.Part.Name),
            }),
            pull.SerialisedItemState({
              predicate: new Equals({ propertyType: m.SerialisedItemState.IsActive, value: true }),
              sort: new Sort(m.SerialisedInventoryItemState.Name),
            }),
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, add }))
            );
        })
      )
      .subscribe(({ loaded, add }) => {
        this.allors.context.reset();

        const internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;
        this.facility = internalOrganisation.DefaultFacility;
        this.activeSuppliers = internalOrganisation.ActiveSuppliers as Organisation[];
        this.activeSuppliers = this.activeSuppliers.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));

        this.owner = loaded.objects.Party as Party;
        this.serialisedItemStates = loaded.collections.SerialisedItemStates as SerialisedItemState[];
        this.ownerships = loaded.collections.Ownerships as Ownership[];
        this.locales = loaded.collections.AdditionalLocales as Locale[];

        this.forPart = loaded.objects.Part as Part;
        this.itemPart = loaded.objects.ItemPart as Part;
        this.parts = loaded.collections.AllParts as Part[];
        this.parts = this.parts.filter(v => v.InventoryItemKind.UniqueId === '2596E2DD-3F5D-4588-A4A2-167D6FBE3FAE'.toLowerCase());

        this.part = this.forPart || this.itemPart;

        if (add) {
          this.add = !(this.edit = false);

          this.item = this.allors.context.create('SerialisedItem') as SerialisedItem;

          if (this.fromPart) {
            this.item.AvailableForSale = true;
          }

          if (!this.fromPart) {
            this.item.OwnedBy = this.owner;
            this.item.AvailableForSale = false;
          }
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

    this.onSave();

    this.allors.context
      .save()
      .subscribe(() => {
        this.goBack();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public update(): void {

    this.onSave();

    this.allors.context
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

  private onSave() {
    if (this.part) {
      this.part.AddSerialisedItem(this.item);
    }
  }
  }
