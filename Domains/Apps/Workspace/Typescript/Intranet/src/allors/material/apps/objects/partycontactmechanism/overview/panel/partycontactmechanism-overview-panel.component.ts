import { Component, Self, OnInit } from '@angular/core';
import { PanelService, NavigationService, RefreshService, ErrorService, Action, MetaService } from '../../../../../../angular';
import { PartyContactMechanism, ContactMechanism } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, Table, CreateData, EditService, EditData } from '../../../../..';
import * as moment from 'moment';

interface Row extends TableRow {
  object: ContactMechanism;
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
export class PartyContactMechanismOverviewPanelComponent implements OnInit {

  m: Meta;

  objects: PartyContactMechanism[];
  table: Table<Row>;

  delete: Action;
  edit: Action;

  get createData(): CreateData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
    };
  }

  contactMechanismsCollection = 'Current';
  currentPartyContactMechanisms: PartyContactMechanism[];
  inactivePartyContactMechanisms: PartyContactMechanism[];
  allPartyContactMechanisms: PartyContactMechanism[];

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

    this.panel.name = 'partycontactmechanism';
    this.panel.title = 'Contact Mechanisms';
    this.panel.icon = 'contacts';
    this.panel.expandable = true;

    this.delete = this.deleteService.delete(this.panel.manager.context);
    this.edit = this.editService.edit();

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'purpose' },
        { name: 'contact' },
        { name: 'last modified' },
      ],
      actions: [
        this.edit,
        this.delete,
      ],
      defaultAction: this.edit,
    });

    const pullName = `${this.panel.name}_${this.m.PartyContactMechanism.name}`;
    const active = `${this.panel.name}_${this.m.PartyContactMechanism.name}_active`;
    const inactive = `${this.panel.name}_${this.m.PartyContactMechanism.name}_inactive`;

    this.panel.onPull = (pulls) => {

      const { pull, x } = this.metaService;
      const id = this.panel.manager.id;

      pulls.push(
        pull.Party({
          name: pullName,
          object: id,
          fetch: {
            PartyContactMechanisms: {
              include: {
                ContactPurposes: x,
                ContactMechanism: {
                  PostalAddress_PostalBoundary: {
                    Country: x,
                  }
                }
              }
            }
          }
        }),
        pull.Party({
          name: active,
          object: id,
          fetch: {
            CurrentPartyContactMechanisms: {
              include: {
                ContactPurposes: x,
                ContactMechanism: {
                  PostalAddress_PostalBoundary: {
                    Country: x,
                  }
                }
              }
            }
          }
        }),
        pull.Party({
          name: inactive,
          object: id,
          fetch: {
            InactivePartyContactMechanisms: {
              include: {
                ContactPurposes: x,
                ContactMechanism: {
                  PostalAddress_PostalBoundary: {
                    Country: x,
                  }
                }
              }
            }
          }
        })
      );
    };

    this.panel.onPulled = (loaded) => {
      this.objects = loaded.collections[pullName] as PartyContactMechanism[];

      this.currentPartyContactMechanisms = loaded.collections[active] as PartyContactMechanism[];
      this.inactivePartyContactMechanisms = loaded.collections[inactive] as PartyContactMechanism[];
      this.allPartyContactMechanisms = this.currentPartyContactMechanisms.concat(this.inactivePartyContactMechanisms);

      if (this.objects) {
        this.table.total = loaded.values[`${pullName}_total`] || this.objects.length;
        this.refreshTable();
      }
    };
  }

  public refreshTable() {
    this.table.data = this.partyContactMechanisms.map((v) => {
      return {
        object: v.ContactMechanism,
        purpose: v.ContactPurposes.map(w => w.Name).join(', '),
        contact: v.ContactMechanism.displayName,
        lastModifiedDate: moment(v.LastModifiedDate).fromNow()
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
