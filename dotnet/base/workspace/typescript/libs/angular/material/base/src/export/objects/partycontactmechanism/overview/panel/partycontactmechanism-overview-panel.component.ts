import { Component, OnInit, Self, HostBinding } from '@angular/core';
import { formatDistance } from 'date-fns';

import { MetaService, NavigationService, PanelService, RefreshService } from '@allors/angular/services/core';
import { PartyContactMechanism } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { TableRow, Table, DeleteService, EditService } from '@allors/angular/material/core';
import { TestScope, Action } from '@allors/angular/core';
import { ObjectData } from '@allors/angular/material/services/core';

interface Row extends TableRow {
  object: PartyContactMechanism;
  purpose: string;
  contact: string;
  lastModifiedDate: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'partycontactmechanism-overview-panel',
  templateUrl: './partycontactmechanism-overview-panel.component.html',
  providers: [PanelService]
})
export class PartyContactMechanismOverviewPanelComponent extends TestScope implements OnInit {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: PartyContactMechanism[];
  table: Table<Row>;

  delete: Action;
  edit: Action;

  get createData(): ObjectData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
    };
  }

  contactMechanismsCollection = 'Current';
  currentPartyContactMechanisms: PartyContactMechanism[];
  inactivePartyContactMechanisms: PartyContactMechanism[];
  allPartyContactMechanisms: PartyContactMechanism[] = [];

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

    this.panel.name = 'partycontactmechanism';
    this.panel.title = 'Party ContactMechanisms';
    this.panel.icon = 'contacts';
    this.panel.expandable = true;

    this.delete = this.deleteService.delete(this.panel.manager.context);
    this.edit = this.editService.edit();

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'purpose', sort },
        { name: 'contact', sort },
        { name: 'lastModifiedDate', sort },
      ],
      actions: [
        this.edit,
        this.delete,
      ],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    const pullName = `${this.panel.name}_${this.m.PartyContactMechanism.name}`;
    const active = `${this.panel.name}_${this.m.PartyContactMechanism.name}_active`;
    const inactive = `${this.panel.name}_${this.m.PartyContactMechanism.name}_inactive`;

    this.panel.onPull = (pulls) => {

      const { pull, x, tree } = this.metaService;
      const id = this.panel.manager.id;

      const partyContactMechanismTree = tree.PartyContactMechanism({
        ContactPurposes: x,
        ContactMechanism: {
          PostalAddress_Country: x
        }
      });

      pulls.push(
        pull.Party({
          name: pullName,
          object: id,
          fetch: {
            PartyContactMechanisms: {
              include: partyContactMechanismTree
            }
          }
        }),
        pull.Party({
          name: active,
          object: id,
          fetch: {
            CurrentPartyContactMechanisms: {
              include: partyContactMechanismTree
            }
          }
        }),
        pull.Party({
          name: inactive,
          object: id,
          fetch: {
            InactivePartyContactMechanisms: {
              include: partyContactMechanismTree
            }
          }
        })
      );
    };

    this.panel.onPulled = (loaded) => {
      this.objects = loaded.collections[pullName] as PartyContactMechanism[];

      this.currentPartyContactMechanisms = loaded.collections[active] as PartyContactMechanism[];
      this.inactivePartyContactMechanisms = loaded.collections[inactive] as PartyContactMechanism[];

      this.allPartyContactMechanisms = [];

      if (this.currentPartyContactMechanisms !== undefined) {
        this.allPartyContactMechanisms = this.allPartyContactMechanisms.concat(this.currentPartyContactMechanisms);
      }

      if (this.inactivePartyContactMechanisms !== undefined) {
        this.allPartyContactMechanisms = this.allPartyContactMechanisms.concat(this.inactivePartyContactMechanisms);
      }

      if (this.objects) {
        this.table.total = loaded.values[`${pullName}_total`] || this.objects.length;
        this.refreshTable();
      }
    };
  }

  public refreshTable() {
    this.table.data = this.partyContactMechanisms.map((v) => {
      return {
        object: v,
        purpose: v.ContactPurposes.map(w => w.Name).join(', '),
        contact: v.ContactMechanism.displayName,
        lastModifiedDate: formatDistance(new Date(v.LastModifiedDate), new Date())
      } as Row;
    });
  }

  get partyContactMechanisms(): any {

    switch (this.contactMechanismsCollection) {
      case 'Current':
        return this.currentPartyContactMechanisms;
      case 'Inactive':
        return this.inactivePartyContactMechanisms;
      case 'All':
      default:
        return this.allPartyContactMechanisms;
    }
  }
}
