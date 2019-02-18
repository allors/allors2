import { Component, Self, OnInit, HostBinding } from '@angular/core';
import { PanelService, NavigationService, RefreshService, ErrorService, Action, MetaService } from '../../../../../../angular';
import { PriceComponent } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, Table, CreateData, EditService, Sorter } from '../../../../..';
import * as moment from 'moment';

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
export class PriceComponentOverviewPanelComponent implements OnInit {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: PriceComponent[];
  table: Table<Row>;

  delete: Action;
  edit: Action;

  get createData(): CreateData {
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
    public errorService: ErrorService,
    public deleteService: DeleteService,
    public editService: EditService
  ) {

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
        from: moment(v.FromDate).format('L'),
        through: v.ThroughDate !== null ? moment(v.ThroughDate).format('L') : '',
        lastModifiedDate: moment(v.LastModifiedDate).fromNow()
      } as Row;
    });
  }

  get priceComponents(): PriceComponent[] {

    switch (this.priceComponentsCollection) {
      case 'Current':
        return this.objects.filter(v => moment(v.FromDate).isBefore(moment()) && (!v.ThroughDate || moment(v.ThroughDate).isAfter(moment())));
      case 'Inactive':
        return this.objects.filter(v => moment(v.FromDate).isAfter(moment()) || (v.ThroughDate && moment(v.ThroughDate).isBefore(moment())));
      case 'All':
      default:
        return this.objects;
    }
  }
}
