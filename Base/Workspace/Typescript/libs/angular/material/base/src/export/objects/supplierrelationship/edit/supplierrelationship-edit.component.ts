import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest, BehaviorSubject, Observable } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { isBefore, isAfter } from 'date-fns';

import { ContextService, TestScope, MetaService, RefreshService, Context, Saved, NavigationService, Action, Invoked, SearchFactory } from '@allors/angular/core';
import { ElectronicAddress, Enumeration, Employment, Person, Party, Organisation, CommunicationEventPurpose, FaceToFaceCommunication, CommunicationEventState, OrganisationContactRelationship, InventoryItem, InternalOrganisation, InventoryItemTransaction, InventoryTransactionReason, Part, Facility, Lot, SerialisedInventoryItem, SerialisedItem, NonSerialisedInventoryItemState, SerialisedInventoryItemState, NonSerialisedInventoryItem, ContactMechanism, LetterCorrespondence, PartyContactMechanism, PostalAddress, OrderAdjustment, OrganisationContactKind, PartyRate, TimeFrequency, RateType, PhoneCommunication, TelecommunicationsNumber, PositionType, PositionTypeRate, ProductIdentification, ProductIdentificationType, ProductType, SerialisedItemCharacteristicType, PurchaseInvoiceApproval, PurchaseOrderApprovalLevel1, PurchaseOrderApprovalLevel2, PurchaseOrder, PurchaseOrderItem, VatRegime, IrpfRegime, InvoiceItemType, SupplierOffering, UnifiedGood, Product, ProductQuote, QuoteItem, RequestItem, UnitOfMeasure, RequestItemState, RequestState, QuoteItemState, QuoteState, SalesOrderItemState, SalesOrderState, ShipmentItemState, ShipmentState, Receipt, SalesInvoice, PaymentApplication, RepeatingPurchaseInvoice, DayOfWeek, RepeatingSalesInvoice, SalesInvoiceItem, SalesOrderItem, SerialisedItemAvailability, NonUnifiedPart, SalesOrder, SalesTerm, TermType, Singleton, IUnitOfMeasure, Shipment, ShipmentItem, OrderShipment, PurchaseOrderState, Good, SubContractorRelationship, SupplierRelationship } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta, ids } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/core';
import { InternalOrganisationId, FetcherService, FiltersService } from '@allors/angular/base';
import { IObject, ISessionObject } from '@allors/domain/system';
import { Equals, Sort, And, ContainedIn, Extent, LessThan, Or, Not, Exists, GreaterThan } from '@allors/data/system';


@Component({
  templateUrl: './supplierrelationship-edit.component.html',
  providers: [ContextService]
})
export class SupplierRelationshipEditComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  partyRelationship: SupplierRelationship;
  internalOrganisation: Organisation;
  organisation: Organisation;
  title: string;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<SupplierRelationshipEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private internalOrganisationId: InternalOrganisationId,
    private fetcher: FetcherService
  ) {
    super();

    this.m = this.metaService.m;
  }

  static canCreate(createData: ObjectData, context: Context) {

    const personId = ids.Person;
    if (createData.associationObjectType.id === personId) {
      return false;
    }

    return true;
  }

  public ngOnInit(): void {

    const { pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(() => {

          const isCreate = this.data.id === undefined;

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.SupplierRelationship({
              object: this.data.id,
              include: {
                InternalOrganisation: x,
                Parties: x
              }
            }),
            pull.Organisation({
              object: this.data.associationId,
            }),
          ];

          return this.allors.context
            .load(new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {

        this.allors.context.reset();

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.organisation = loaded.objects.Organisation as Organisation;

        if (isCreate) {

          if (this.organisation === undefined) {
            this.dialogRef.close();
          }

          this.title = 'Add Supplier Relationship';

          this.partyRelationship = this.allors.context.create('SupplierRelationship') as SupplierRelationship;
          this.partyRelationship.FromDate = new Date().toISOString();
          this.partyRelationship.Supplier = this.organisation;
          this.partyRelationship.InternalOrganisation = this.internalOrganisation;
          this.partyRelationship.NeedsApproval = false;
        } else {
          this.partyRelationship = loaded.objects.SupplierRelationship as SupplierRelationship;

          if (this.partyRelationship.CanWriteFromDate) {
            this.title = 'Edit Supplier Relationship';
          } else {
            this.title = 'View Supplier Relationship';
          }
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
          id: this.partyRelationship.id,
          objectType: this.partyRelationship.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }
}
