import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest, BehaviorSubject } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, Context, Saved, NavigationService } from '@allors/angular/services/core';
import {
  ElectronicAddress,
  Enumeration,
  Employment,
  Person,
  Party,
  Organisation,
  CommunicationEventPurpose,
  FaceToFaceCommunication,
  CommunicationEventState,
  OrganisationContactRelationship,
  InventoryItem,
  InternalOrganisation,
  InventoryItemTransaction,
  InventoryTransactionReason,
  Part,
  Facility,
  Lot,
  SerialisedInventoryItem,
  SerialisedItem,
  NonSerialisedInventoryItemState,
  SerialisedInventoryItemState,
  NonSerialisedInventoryItem,
  ContactMechanism,
  LetterCorrespondence,
  PartyContactMechanism,
  PostalAddress,
} from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta, ids } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/services/core';
import { InternalOrganisationId, FetcherService } from '@allors/angular/base';
import { IObject, ISessionObject } from '@allors/domain/system';
import { TestScope, Action, SearchFactory } from '@allors/angular/core';

@Component({
  templateUrl: './nonserialisedinventoryitem-edit.component.html',
  providers: [ContextService],
})
export class NonSerialisedInventoryItemEditComponent extends TestScope implements OnInit, OnDestroy {
  public m: Meta;
  public title: string;

  public nonSerialisedInventoryItem: NonSerialisedInventoryItem;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<NonSerialisedInventoryItemEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private fetcher: FetcherService,
    private internalOrganisationId: InternalOrganisationId
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {
    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(() => {
          const isCreate = this.data.id === undefined;

          const pulls = [
            this.fetcher.locales,
          ];

          if (!isCreate) {
            pulls.push(
              pull.NonSerialisedInventoryItem({
                object: this.data.id,
              }),
            );
          }

          return this.allors.context.load(new PullRequest({ pulls })).pipe(map((loaded) => ({ loaded, isCreate })));
        })
      )
      .subscribe(({ loaded, isCreate }) => {
        this.allors.context.reset();

        this.nonSerialisedInventoryItem = loaded.objects.NonSerialisedInventoryItem as NonSerialisedInventoryItem;

        if (this.nonSerialisedInventoryItem.CanWritePartLocation) {
          this.title = 'Edit Inventory Item';
        } else {
          this.title = 'View Inventory Item';
        }
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {
    this.allors.context.save().subscribe((saved: Saved) => {
      const data: IObject = {
        id: this.nonSerialisedInventoryItem.id,
        objectType: this.nonSerialisedInventoryItem.objectType,
      };

      this.dialogRef.close(data);
      this.refreshService.refresh();
    }, this.saveService.errorHandler);
  }
}
