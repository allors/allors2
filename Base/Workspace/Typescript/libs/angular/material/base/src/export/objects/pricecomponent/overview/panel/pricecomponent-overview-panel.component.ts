import { Component, Self, OnInit, OnDestroy, Inject, HostBinding } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { isBefore, isAfter, format, formatDistance } from 'date-fns';

import { TestScope, MetaService, NavigationService, PanelService, MediaService, ContextService, RefreshService, Action, ActionTarget, Invoked } from '@allors/angular/core';
import { Organisation, Person, OrganisationContactRelationship, OrganisationContactKind, SupplierOffering, Part, RatingType, Ordinal, UnitOfMeasure, Currency, Settings, SupplierRelationship, WorkTask, SalesInvoice, FixedAsset, Printable, UnifiedGood, Payment, Invoice, PurchaseInvoice, WorkEffort, SerialisedItem, SalesOrder, ProductQuote, PurchaseOrder, PriceComponent } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { ObjectData, SaveService, TableRow, Table, ObjectService, MethodService, DeleteService, EditService, OverviewService, Sorter } from '@allors/angular/material/core';
import { FiltersService, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { Sort, ContainedIn, Extent, Equals } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { IObject } from '@allors/domain/system';


interface Row extends TableRow {
  object: PriceComponent;
  type: string;
  price: string;
  from: string;
  through: string;
  lastModifiedDate: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'pricecomponent-overview-panel',
  templateUrl: './pricecomponent-overview-panel.component.html',
  providers: [PanelService]
})
export class PriceComponentOverviewPanelComponent extends TestScope implements OnInit {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: PriceComponent[];
  table: Table<Row>;

  delete: Action;
  edit: Action;

  get createData(): ObjectData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
    };
  }

  priceComponentsCollection: string;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,

    public deleteService: DeleteService,
    public editService: EditService
  ) {
    super();

    this.m = this.metaService.m;
  }

  ngOnInit() {

    const { pull, x, m } = this.metaService;

    this.panel.name = 'priceComponent';
    this.panel.title = 'Price Components';
    this.panel.icon = 'business';
    this.panel.expandable = true;

    this.delete = this.deleteService.delete(this.panel.manager.context);
    this.edit = this.editService.edit();

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'type' },
        { name: 'price', sort },
        { name: 'from', sort },
        { name: 'through', sort },
        { name: 'lastModifiedDate' },
      ],
      actions: [
        this.edit,
        this.delete,
      ],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    const sorter = new Sorter(
      {
        price: m.PriceComponent.Price,
        from: m.PriceComponent.FromDate,
        through: m.PriceComponent.ThroughDate,
        lastModifiedDate: m.PriceComponent.LastModifiedDate,
      }
    );

    const pullName = `${this.panel.name}_${this.m.PriceComponent.name}`;

    this.panel.onPull = (pulls) => {

      const id = this.panel.manager.id;

      pulls.push(
        pull.Part({
          name: pullName,
          object: id,
          fetch: {
            PriceComponentsWherePart: {
              include: {
                Currency: x
              }
            }
          }
        }),
        pull.Product({
          name: pullName,
          object: id,
          fetch: {
            PriceComponentsWhereProduct: {
              include: {
                Currency: x
              }
            }
          }
        }),
      );
    };

    this.panel.onPulled = (loaded) => {
      this.objects = loaded.collections[pullName] as PriceComponent[];

      if (this.objects) {
        this.table.total = loaded.values[`${pullName}_total`] || this.objects.length;
        this.refreshTable();
      }
    };
  }

  public refreshTable() {
    this.table.data = this.priceComponents.map((v) => {
      return {
        object: v,
        type: v.objectType.name,
        price: v.Currency.IsoCode + ' ' + v.Price,
        from: format(new Date(v.FromDate), 'dd-MM-yyyy'),
        through: v.ThroughDate !== null ? format(new Date(v.ThroughDate), 'dd-MM-yyyy') : '',
        lastModifiedDate: formatDistance(new Date(v.LastModifiedDate), new Date())
      } as Row;
    });
  }

  get priceComponents(): PriceComponent[] {

    switch (this.priceComponentsCollection) {
      case 'Current':
        return this.objects.filter(v => isBefore(new Date(v.FromDate), new Date()) && (!v.ThroughDate || isAfter(new Date(v.ThroughDate), new Date())));
      case 'Inactive':
        return this.objects.filter(v => isAfter(new Date(v.FromDate), new Date()) || (v.ThroughDate && isBefore(new Date(v.ThroughDate), new Date())));
      case 'All':
      default:
        return this.objects;
    }
  }
}
