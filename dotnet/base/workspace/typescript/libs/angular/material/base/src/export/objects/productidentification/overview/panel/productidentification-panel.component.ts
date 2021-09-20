import { Component, OnInit, Self, HostBinding, Input } from '@angular/core';

import { MetaService, PanelService, RefreshService } from '@allors/angular/services/core';
import { ProductIdentification } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { TableRow, Table, DeleteService, EditService } from '@allors/angular/material/core';
import { TestScope, Action } from '@allors/angular/core';
import { ObjectData, ObjectService } from '@allors/angular/material/services/core';
import { Pull, Fetch, Step } from '@allors/data/system';
import { RoleType } from '@allors/meta/system';

interface Row extends TableRow {
  object: ProductIdentification;
  type: string;
  identification: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'productidentification-panel',
  templateUrl: './productIdentification-panel.component.html',
  providers: [PanelService]
})
export class ProductIdentificationsPanelComponent extends TestScope implements OnInit {
  @Input() roleType: RoleType;

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: ProductIdentification[];
  table: Table<Row>;

  delete: Action;
  edit: Action;

  get createData(): ObjectData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
      associationRoleType: this.roleType,
    };
  }
  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public objectService: ObjectService,
    public refreshService: RefreshService,
    public editService: EditService,
    public deleteService: DeleteService,
  ) {
    super();

    this.m = this.metaService.m;
  }

  ngOnInit() {

    this.panel.name = 'productidentification';
    this.panel.title = 'Product Identification';
    this.panel.icon = 'fingerprint';
    this.panel.expandable = true;

    this.delete = this.deleteService.delete(this.panel.manager.context);
    this.edit = this.editService.edit();

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'type', sort },
        { name: 'identification', sort },
      ],
      actions: [
        this.edit,
        this.delete,
      ],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    const pullName = `${this.panel.name}_${this.m.ProductIdentification.name}`;

    this.panel.onPull = (pulls) => {
      const { x, tree } = this.metaService;
      const { id, objectType } = this.panel.manager;

      pulls.push(
        new Pull(objectType, {
          name: pullName,
          object: id,
          fetch: new Fetch({
            step: new Step({
              propertyType: this.roleType,
              include: tree.ProductIdentification({
                ProductIdentificationType: x,
              })
            })
          })
        })
      );

      this.panel.onPulled = (loaded) => {
        this.objects = loaded.collections[pullName] as ProductIdentification[];
        this.table.total = loaded.values[`${pullName}_total`] || this.objects.length;
        this.table.data = this.objects.map((v) => {
          return {
            object: v,
            type: v.ProductIdentificationType && v.ProductIdentificationType.Name,
            identification: v.Identification,
          } as Row;
        });
      };
    };
  }
}
