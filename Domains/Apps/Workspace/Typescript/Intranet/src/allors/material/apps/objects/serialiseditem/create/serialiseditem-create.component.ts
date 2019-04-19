import * as moment from 'moment';

import { Component, OnDestroy, OnInit, Self, Inject, Optional } from '@angular/core';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { SaveService, FiltersService, ObjectData } from '../../../../../material';
import {  ContextService, SearchFactory, MetaService, RefreshService, FetcherService, InternalOrganisationId } from '../../../../../angular';
import { Locale, Organisation, Ownership, SerialisedItem, Part, SerialisedItemState, Party, SupplierRelationship } from '../../../../../domain';
import { Equals, PullRequest, Sort, IObject } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  templateUrl: './serialiseditem-create.component.html',
  providers: [ContextService]
})
export class SerialisedItemCreateComponent implements OnInit, OnDestroy {

  readonly m: Meta;
  serialisedItem: SerialisedItem;

  public title = 'Add Serialised Asset';

  locales: Locale[];
  suppliers: Organisation[];
  ownerships: Ownership[];
  organisations: Organisation[];
  organisationFilter: SearchFactory;
  activeSuppliers: Organisation[];
  serialisedItemStates: SerialisedItemState[];
  owner: Party;
  part: Part;
  itemPart: Part;
  parts: Part[];
  currentSuppliers: Set<Organisation>;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public filtersService: FiltersService,
    public dialogRef: MatDialogRef<SerialisedItemCreateComponent>,
    public metaService: MetaService,
    private refreshService: RefreshService,
    private saveService: SaveService,
    private fetcher: FetcherService,
    private internalOrganisationId: InternalOrganisationId,
    ) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(([]) => {

          const pulls = [
            this.fetcher.locales,
            pull.Party({ object: this.data.associationId }),
            pull.Part({
              name: 'forPart',
              object: this.data.associationId
            }),
            pull.Ownership({ sort: new Sort(m.Ownership.Name) }),
            pull.SupplierRelationship({
              include: {
                Supplier: x
              }
            }),
            pull.Part({
              include: {
                InventoryItemKind: x,
                SerialisedItems: x
              },
              sort: new Sort(m.Part.Name),
            }),
            pull.SerialisedItemState({
              predicate: new Equals({ propertyType: m.SerialisedItemState.IsActive, value: true }),
              sort: new Sort(m.SerialisedInventoryItemState.Name),
            }),
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.allors.context.reset();

        const now = moment.utc();

        const supplierRelationships = loaded.collections.SupplierRelationships as SupplierRelationship[];
        const currentsupplierRelationships = supplierRelationships.filter(v => moment(v.FromDate).isBefore(now) && (v.ThroughDate === null || moment(v.ThroughDate).isAfter(now)));
        this.currentSuppliers = new Set(currentsupplierRelationships.map(v => v.Supplier).sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0)));

        this.owner = loaded.objects.Party as Party;
        this.part = loaded.objects.forPart as Part;

        this.serialisedItemStates = loaded.collections.SerialisedItemStates as SerialisedItemState[];
        this.ownerships = loaded.collections.Ownerships as Ownership[];
        this.locales = loaded.collections.AdditionalLocales as Locale[];

        this.parts = loaded.collections.Parts as Part[];
        this.parts = this.parts.filter(v => v.InventoryItemKind.UniqueId === '2596E2DD-3F5D-4588-A4A2-167D6FBE3FAE'.toLowerCase());

        this.serialisedItem = this.allors.context.create('SerialisedItem') as SerialisedItem;
        this.serialisedItem.AvailableForSale = false;
        this.serialisedItem.OwnedBy = this.owner;

        if (this.part) {
          this.serialisedItem.Name = this.part.Name;
        }
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public partSelected(part: Part): void {

    this.serialisedItem.Name = part.Name;
  }

  public save(): void {

    this.part.AddSerialisedItem(this.serialisedItem);

    this.allors.context
      .save()
      .subscribe(() => {
        const data: IObject = {
          id: this.serialisedItem.id,
          objectType: this.serialisedItem.objectType,
        };

        this.dialogRef.close(data);
      },
      this.saveService.errorHandler
    );
  }
}
