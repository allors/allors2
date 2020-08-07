import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest, BehaviorSubject, Observable } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { isBefore, isAfter } from 'date-fns';

import { ContextService, TestScope, MetaService, RefreshService, Context, Saved, NavigationService, Action, Invoked, SearchFactory, SingletonId } from '@allors/angular/core';
import { ElectronicAddress, Enumeration, Employment, Person, Party, Organisation, CommunicationEventPurpose, FaceToFaceCommunication, CommunicationEventState, OrganisationContactRelationship, InventoryItem, InternalOrganisation, InventoryItemTransaction, InventoryTransactionReason, Part, Facility, Lot, SerialisedInventoryItem, SerialisedItem, NonSerialisedInventoryItemState, SerialisedInventoryItemState, NonSerialisedInventoryItem, ContactMechanism, LetterCorrespondence, PartyContactMechanism, PostalAddress, OrderAdjustment, OrganisationContactKind, PartyRate, TimeFrequency, RateType, PhoneCommunication, TelecommunicationsNumber, PositionType, PositionTypeRate, ProductIdentification, ProductIdentificationType, ProductType, SerialisedItemCharacteristicType, PurchaseInvoiceApproval, PurchaseOrderApprovalLevel1, PurchaseOrderApprovalLevel2, PurchaseOrder, PurchaseOrderItem, VatRegime, IrpfRegime, InvoiceItemType, SupplierOffering, UnifiedGood, Product, ProductQuote, QuoteItem, RequestItem, UnitOfMeasure, RequestItemState, RequestState, QuoteItemState, QuoteState, SalesOrderItemState, SalesOrderState, ShipmentItemState, ShipmentState, Receipt, SalesInvoice, PaymentApplication, RepeatingPurchaseInvoice, DayOfWeek, RepeatingSalesInvoice, SalesInvoiceItem, SalesOrderItem, SerialisedItemAvailability, NonUnifiedPart, SalesOrder, SalesTerm, TermType, Singleton, IUnitOfMeasure, Shipment, ShipmentItem, OrderShipment, PurchaseOrderState, Good, SubContractorRelationship, SupplierRelationship, UserProfile } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta, ids } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/core';
import { InternalOrganisationId, FetcherService, FiltersService } from '@allors/angular/base';
import { IObject, ISessionObject } from '@allors/domain/system';
import { Equals, Sort, And, ContainedIn, Extent, LessThan, Or, Not, Exists, GreaterThan } from '@allors/data/system';
import { PrintService } from '../../../services/actions';


@Component({
  templateUrl: './userprofile-edit.component.html',
  providers: [ContextService]
})
export class UserProfileEditComponent extends TestScope implements OnInit, OnDestroy {

  public title: string;
  public subTitle: string;

  public m: Meta;

  public userProfile: UserProfile;

  private subscription: Subscription;
  internalOrganizations: Organisation[];
  supportedLocales: Locale[];

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<UserProfileEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private singletonId: SingletonId,
  ) {

    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(() => {

          const pulls = [
            pull.Singleton({
              object: this.singletonId.value,
              include: {
                Locales: x,
              }
            }),
            pull.UserProfile({
              object: this.data.id,
              include: {
                DefaultInternalOrganization: x,
                DefaulLocale: x,
              }
            }),
            pull.Organisation(
              {
                predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }),
                sort: new Sort(m.Organisation.PartyName)
              }
            )
          ];

          return this.allors.context
            .load(new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded }))
            );
        })
      )
      .subscribe(({ loaded }) => {

        this.allors.context.reset();

        this.userProfile = loaded.objects.UserProfile as UserProfile;
        this.internalOrganizations = loaded.collections.Organisations as Organisation[];

        const singleton = loaded.objects.Singleton as Singleton;
        this.supportedLocales = singleton.Locales;

        this.title = 'Edit User Profile';
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context
      .save()
      .subscribe((saved: Saved) => {
        const data: IObject = {
          id: this.userProfile.id,
          objectType: this.userProfile.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }
}
