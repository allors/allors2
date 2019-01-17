import { Component, OnDestroy, OnInit, Self, Inject, Optional } from '@angular/core';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, ContextService, SearchFactory, MetaService, RefreshService } from '../../../../../angular';
import { Facility, Locale, Organisation, Ownership, SerialisedItem, Part, SerialisedItemState, Party } from '../../../../../domain';
import { Equals, PullRequest, Sort } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { Fetcher } from '../../Fetcher';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ObjectData, CreateData } from '../../../../../../allors/material/base/services/object';

@Component({
  templateUrl: './serialiseditem-create.component.html',
  providers: [ContextService]
})
export class SerialisedItemCreateComponent implements OnInit, OnDestroy {

  readonly m: Meta;
  serialisedItem: SerialisedItem;

  public title = 'Add Serialised Asset';

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
  private fetcher: Fetcher;

  constructor(
    @Self() public allors: ContextService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: CreateData,
    public dialogRef: MatDialogRef<SerialisedItemCreateComponent>,
    public metaService: MetaService,
    private refreshService: RefreshService,
    private errorService: ErrorService,
    public stateService: StateService) {

    this.m = this.metaService.m;
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([]) => {

          const pulls = [
            this.fetcher.locales,
            this.fetcher.internalOrganisation,
            pull.Party({ object: this.data.associationId }),
            pull.Ownership({ sort: new Sort(m.Ownership.Name) }),
            pull.Part({
              include: {
                InventoryItemKind: x,
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

        const internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.facility = internalOrganisation.DefaultFacility;
        this.activeSuppliers = internalOrganisation.ActiveSuppliers as Organisation[];
        this.activeSuppliers = this.activeSuppliers.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));

        this.owner = loaded.objects.Party as Party;
        this.serialisedItemStates = loaded.collections.SerialisedItemStates as SerialisedItemState[];
        this.ownerships = loaded.collections.Ownerships as Ownership[];
        this.locales = loaded.collections.AdditionalLocales as Locale[];

        this.parts = loaded.collections.Parts as Part[];
        this.parts = this.parts.filter(v => v.InventoryItemKind.UniqueId === '2596E2DD-3F5D-4588-A4A2-167D6FBE3FAE'.toLowerCase());

        this.serialisedItem = this.allors.context.create('SerialisedItem') as SerialisedItem;
        this.serialisedItem.AvailableForSale = false;
        this.serialisedItem.OwnedBy = this.owner;
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context
      .save()
      .subscribe(() => {
        const data: ObjectData = {
          id: this.serialisedItem.id,
          objectType: this.serialisedItem.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
