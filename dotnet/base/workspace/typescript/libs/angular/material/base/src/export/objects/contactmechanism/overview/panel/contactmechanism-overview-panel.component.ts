import { Component, OnInit, Self, HostBinding } from '@angular/core';
import { formatDistance } from 'date-fns';

import { TestScope, Action } from '@allors/angular/core';
import { ContactMechanism } from '@allors/domain/generated';
import { TableRow, Table, EditService, DeleteService } from '@allors/angular/material/core';
import { Meta } from '@allors/meta/generated';
import { MetaService, PanelService, RefreshService, NavigationService } from '@allors/angular/services/core';
import { ObjectData } from '@allors/angular/material/services/core';

interface Row extends TableRow {
  object: ContactMechanism;
  contact: string;
  lastModifiedDate: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'contactmechanism-overview-panel',
  templateUrl: './contactmechanism-overview-panel.component.html',
  providers: [PanelService],
})
export class ContactMechanismOverviewPanelComponent extends TestScope implements OnInit {
  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: ContactMechanism[];
  table: Table<Row>;

  delete: Action;
  edit: Action;

  get createData(): ObjectData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
    };
  }

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
    this.panel.name = 'contactmechanism';
    this.panel.title = 'Contact Mechanisms';
    this.panel.icon = 'contacts';
    this.panel.expandable = true;

    this.delete = this.deleteService.delete(this.panel.manager.context);
    this.edit = this.editService.edit();

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'contact', sort },
        { name: 'lastModifiedDate', sort },
      ],
      actions: [this.edit, this.delete],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    const pullName = `${this.panel.name}_${this.m.PartyContactMechanism.name}`;

    this.panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;
      const id = this.panel.manager.id;

      pulls.push(
        pull.Party({
          name: pullName,
          object: id,
          fetch: {
            PartyContactMechanisms: {
              ContactMechanism: {
                include: {
                  PostalAddress_Country: x,
                },
              },
            },
          },
        })
      );
    };

    this.panel.onPulled = (loaded) => {
      this.objects = loaded.collections[pullName] as ContactMechanism[];

      if (this.objects) {
        this.table.total = loaded.values[`${pullName}_total`] || this.objects.length;
        this.table.data = this.objects.map((v) => {
          return {
            object: v,
            contact: v.displayName,
            lastModifiedDate: formatDistance(new Date(v.LastModifiedDate), new Date()),
          } as Row;
        });
      }
    };
  }
}
