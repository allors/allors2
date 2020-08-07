import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest, BehaviorSubject, Observable } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { isBefore, isAfter } from 'date-fns';

import { ContextService, TestScope, MetaService, RefreshService, Context, Saved, NavigationService, Action, Invoked, SearchFactory } from '@allors/angular/core';
import { ElectronicAddress, Enumeration, Employment, Person, Party, Organisation, CommunicationEventPurpose, FaceToFaceCommunication, CommunicationEventState, OrganisationContactRelationship, InventoryItem, InternalOrganisation, InventoryItemTransaction, InventoryTransactionReason, Part, Facility, Lot, SerialisedInventoryItem, SerialisedItem, NonSerialisedInventoryItemState, SerialisedInventoryItemState, NonSerialisedInventoryItem, ContactMechanism, LetterCorrespondence, PartyContactMechanism, PostalAddress, OrderAdjustment, OrganisationContactKind, PartyRate, TimeFrequency, RateType, PhoneCommunication, TelecommunicationsNumber, PositionType, PositionTypeRate, ProductIdentification, ProductIdentificationType, ProductType, SerialisedItemCharacteristicType, PurchaseInvoiceApproval, PurchaseOrderApprovalLevel1, PurchaseOrderApprovalLevel2, PurchaseOrder, PurchaseOrderItem, VatRegime, IrpfRegime, InvoiceItemType, SupplierOffering, UnifiedGood, Product, ProductQuote, QuoteItem, RequestItem, UnitOfMeasure, RequestItemState, RequestState, QuoteItemState, QuoteState, SalesOrderItemState, SalesOrderState, ShipmentItemState, ShipmentState, Receipt, SalesInvoice, PaymentApplication, RepeatingPurchaseInvoice, DayOfWeek, RepeatingSalesInvoice, SalesInvoiceItem, SalesOrderItem, SerialisedItemAvailability, NonUnifiedPart, SalesOrder, SalesTerm, TermType, Singleton, IUnitOfMeasure } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta, ids } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/core';
import { InternalOrganisationId, FetcherService, FiltersService } from '@allors/angular/base';
import { IObject, ISessionObject } from '@allors/domain/system';
import { Equals, Sort, And, ContainedIn, Extent, LessThan, Or, Not, Exists, GreaterThan } from '@allors/data/system';
import { PrintService } from '../../../services/actions';

@Component({
  templateUrl: './serialiseditemcharacteristic-edit.component.html',
  providers: [ContextService]
})
export class SerialisedItemCharacteristicEditComponent extends TestScope implements OnInit, OnDestroy {

  public title: string;
  public subTitle: string;

  public m: Meta;

  public productCharacteristic: SerialisedItemCharacteristicType;

  public singleton: Singleton;
  public uoms: IUnitOfMeasure[];
  public timeFrequencies: TimeFrequency[];
  public allUoms: IUnitOfMeasure[];

  private subscription: Subscription;
  locales: Locale[];

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<SerialisedItemCharacteristicEditComponent>,
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
            pull.SerialisedItemCharacteristicType(
              {
                object: this.data.id,
                include: {
                  LocalisedNames: {
                    Locale: x,
                  }
                }
              }
            ),
            pull.Singleton({
              include: {
                AdditionalLocales: {
                  Language: x,
                }
              }
            }),
            pull.UnitOfMeasure({
              predicate: new Equals({ propertyType: m.UnitOfMeasure.IsActive, value: true }),
              sort: new Sort(m.UnitOfMeasure.Name),
            }),
            pull.TimeFrequency({
              predicate: new Equals({ propertyType: m.TimeFrequency.IsActive, value: true }),
              sort: new Sort(m.TimeFrequency.Name),
            })
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

        this.singleton = loaded.collections.Singletons[0] as Singleton;
        this.uoms = loaded.collections.UnitsOfMeasure as UnitOfMeasure[];
        this.timeFrequencies = loaded.collections.TimeFrequencies as TimeFrequency[];
        this.allUoms = this.uoms.concat(this.timeFrequencies).sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));
        this.locales = loaded.collections.AdditionalLocales as Locale[];

        if (isCreate) {
          this.title = 'Add Product Characteristic';

          this.productCharacteristic = this.allors.context.create('SerialisedItemCharacteristicType') as SerialisedItemCharacteristicType;
          this.productCharacteristic.IsActive = true;
        } else {
          this.productCharacteristic = loaded.objects.SerialisedItemCharacteristicType as SerialisedItemCharacteristicType;

          if (this.productCharacteristic.CanWriteName) {
            this.title = 'Edit Product Characteristic';
          } else {
            this.title = 'View Product Characteristic';
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

    this.allors.context
      .save()
      .subscribe(() => {
        const data: IObject = {
          id: this.productCharacteristic.id,
          objectType: this.productCharacteristic.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }
}
