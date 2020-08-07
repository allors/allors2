import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest, BehaviorSubject } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, TestScope, MetaService, RefreshService, Context, Saved, NavigationService } from '@allors/angular/core';
import { ElectronicAddress, Enumeration, Employment, Person, Party, Organisation, CommunicationEventPurpose, FaceToFaceCommunication, CommunicationEventState, OrganisationContactRelationship, InventoryItem, InternalOrganisation, InventoryItemTransaction, InventoryTransactionReason, Part, Facility, Lot, SerialisedInventoryItem, SerialisedItem, NonSerialisedInventoryItemState, SerialisedInventoryItemState, NonSerialisedInventoryItem, ContactMechanism, LetterCorrespondence, PartyContactMechanism, PostalAddress, OrderAdjustment, OrganisationContactKind, PartyRate, TimeFrequency, RateType, PhoneCommunication, TelecommunicationsNumber, PositionType, PositionTypeRate, Country } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta, ids } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/core';
import { InternalOrganisationId, FetcherService, FiltersService } from '@allors/angular/base';
import { IObject, ISessionObject } from '@allors/domain/system';
import { Equals, Sort } from '@allors/data/system';

@Component({
  templateUrl: './postaladdress-edit.component.html',
  providers: [ContextService]
})
export class PostalAddressEditComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  contactMechanism: PostalAddress;
  countries: Country[];
  title: string;

  private subscription: Subscription;
  party: Party;
  partyContactMechanism: PartyContactMechanism;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: IObject,
    public dialogRef: MatDialogRef<PostalAddressEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private internalOrganisationId: InternalOrganisationId,
  ) {

    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(() => {

          const pulls = [
            pull.ContactMechanism({
              object: this.data.id,
              include: {
                PostalAddress_Country: x
              },
            }),
            pull.Country({
              sort: new Sort(m.Country.Name)
            })
          ];

          return this.allors.context
            .load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.allors.context.reset();

        this.countries = loaded.collections.Countries as Country[];
        this.contactMechanism = loaded.objects.ContactMechanism as PostalAddress;

        if (this.contactMechanism.CanWriteAddress1) {
          this.title = 'Edit Postal Address';
        } else {
          this.title = 'View Postal Address';
        }
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        const data: IObject = {
          id: this.contactMechanism.id,
          objectType: this.contactMechanism.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }
}
