import { Component, OnDestroy, OnInit, Self, Optional, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest, BehaviorSubject } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { isBefore, isAfter } from 'date-fns';

import { ContextService, TestScope, MetaService, RefreshService, SingletonId, Saved, NavigationService, SearchFactory } from '@allors/angular/core';
import { PullRequest } from '@allors/protocol/system';
import { ObjectData, SaveService, AllorsMaterialDialogService } from '@allors/angular/material/core';
import {
  Organisation,
  Facility,
  ProductType,
  ProductIdentificationType,
  Settings,
  Part,
  SupplierRelationship,
  InventoryItemKind,
  SupplierOffering,
  Brand,
  Model,
  PartNumber,
  UnitOfMeasure,
  PartCategory,
  NonUnifiedPart,
  CustomOrganisationClassification,
  IndustryClassification,
  CustomerRelationship,
  InternalOrganisation,
  OrganisationRole,
  LegalForm,
  VatRegime,
  IrpfRegime,
  Currency,
  Person,
  OrganisationContactRelationship,
  Enumeration,
  OrganisationContactKind,
  PersonRole,
  Employment,
  PostalAddress,
  Country,
  Party,
  PartyContactMechanism,
  PurchaseInvoice,
  PurchaseInvoiceType,
  ContactMechanism,
  SerialisedItem,
  Ownership,
  SerialisedItemState,
} from '@allors/domain/generated';
import { Equals, Sort, And, Not, Exists } from '@allors/data/system';
import { FetcherService, InternalOrganisationId, FiltersService } from '@allors/angular/base';
import { IObject, ISessionObject } from '@allors/domain/system';
import { Meta } from '@allors/meta/generated';

@Component({
  templateUrl: './unifiedgood-create.component.html',
  providers: [ContextService]
})
export class UnifiedGoodCreateComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;
  good: Good;

  public title = 'Add Unified Good';

  productTypes: ProductType[];
  inventoryItemKinds: InventoryItemKind[];
  vatRates: VatRate[];
  goodIdentificationTypes: ProductIdentificationType[];
  productNumber: ProductNumber;
  settings: Settings;
  goodNumberType: ProductIdentificationType;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<UnifiedGoodCreateComponent>,
    public metaService: MetaService,
    private refreshService: RefreshService,
    public navigationService: NavigationService,
    private saveService: SaveService,
    private fetcher: FetcherService
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(() => {

          const pulls = [
            this.fetcher.Settings,
            pull.InventoryItemKind(),
            pull.ProductType({ sort: new Sort(m.ProductType.Name) }),
            pull.VatRate(),
            pull.ProductIdentificationType(),
          ];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.allors.context.reset();

        this.inventoryItemKinds = loaded.collections.InventoryItemKinds as InventoryItemKind[];
        this.productTypes = loaded.collections.ProductTypes as ProductType[];
        this.vatRates = loaded.collections.VatRates as VatRate[];
        this.goodIdentificationTypes = loaded.collections.ProductIdentificationTypes as ProductIdentificationType[];
        this.settings = loaded.objects.Settings as Settings;

        const vatRateZero = this.vatRates.find((v: VatRate) => parseFloat(v.Rate) === 0);
        this.goodNumberType = this.goodIdentificationTypes.find((v) => v.UniqueId === 'b640630d-a556-4526-a2e5-60a84ab0db3f');

        this.good = this.allors.context.create('UnifiedGood') as UnifiedGood;
        this.good.VatRate = vatRateZero;

        if (!this.settings.UseProductNumberCounter) {
          this.productNumber = this.allors.context.create('ProductNumber') as ProductNumber;
          this.productNumber.ProductIdentificationType = this.goodNumberType;

          this.good.AddProductIdentification(this.productNumber);
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
          id: this.good.id,
          objectType: this.good.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }
}
