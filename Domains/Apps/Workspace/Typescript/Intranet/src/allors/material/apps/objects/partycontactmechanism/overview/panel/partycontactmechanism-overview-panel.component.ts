import { Component, Self, OnInit } from '@angular/core';
import { PanelService, NavigationService, Saved, RefreshService, ErrorService, Action, MetaService } from '../../../../../../angular';
import { PartyContactMechanism, Person, PostalAddress, ContactMechanism } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, Table, CreateData, EditService } from '../../../../..';
import * as moment from 'moment';
import { PostalAddressEditModule } from '../../../postaladdress/edit/postaladdress-edit.module';

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
  currentContactMechanisms: PartyContactMechanism[];
  inactiveContactMechanisms: PartyContactMechanism[];
  allContactMechanisms: PartyContactMechanism[];

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

      const { pull, tree, x } = this.metaService;
      const id = this.panel.manager.id;


      pulls.push(
        pull.Person({
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
        pull.Person({
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
        pull.Person({
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

      this.currentContactMechanisms = loaded.collections[active] as PartyContactMechanism[];
      this.inactiveContactMechanisms = loaded.collections[inactive] as PartyContactMechanism[];
      this.allContactMechanisms = this.currentContactMechanisms.concat(this.inactiveContactMechanisms);

      if (this.objects) {
        this.table.total = loaded.values[`${pullName}_total`] || this.objects.length;
        this.refreshTable();
      }
    };
  }

  public refreshTable() {
    this.table.data = this.contactMechanisms.map((v) => {
      return {
        object: v,
        purpose: v.ContactPurposes.map(w => w.Name).join(', '),
        contact: this.displayName(v.ContactMechanism),
        lastModifiedDate: moment(v.LastModifiedDate).fromNow()
      } as Row;
    });
  }

  private displayName(contactMechanism: ContactMechanism): string {
    if (contactMechanism.objectType.name === this.metaService.m.PostalAddress.name) {
      const postalAddress = contactMechanism as PostalAddress;
      return postalAddress.displayName;
    }

    return '';
  }

  get contactMechanisms(): any {

    switch (this.contactMechanismsCollection) {
      case 'Current':
        return this.currentContactMechanisms;
      case 'Inactive':
        return this.inactiveContactMechanisms;
      case 'All':
      default:
        return this.allContactMechanisms;
    }
  }
}
