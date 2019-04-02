import { Component, Self, OnInit, HostBinding } from '@angular/core';
import { PanelService, NavigationService, RefreshService, Action, MetaService } from '../../../../../../angular';
import { PartyContactMechanism, ContactMechanism } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, Table, CreateData, EditService } from '../../../../..';
import * as moment from 'moment';

interface Row extends TableRow {
  object: ContactMechanism;
  contact: string;
  lastModifiedDate: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'contactmechanism-overview-panel',
  templateUrl: './contactmechanism-overview-panel.component.html',
  providers: [PanelService]
})
export class ContactMechanismOverviewPanelComponent implements OnInit {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: ContactMechanism[];
  table: Table<Row>;

  delete: Action;
  edit: Action;

  get createData(): CreateData {
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
      actions: [
        this.edit,
        this.delete,
      ],
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
      this.objects = loaded.collections[pullName] as ContactMechanism[];

      if (this.objects) {
        this.table.total = loaded.values[`${pullName}_total`] || this.objects.length;
        this.table.data = this.objects.map((v) => {
          return {
            object: v,
            contact: v.displayName,
            lastModifiedDate: moment(v.LastModifiedDate).fromNow()
          } as Row;
        });
      }
    };
  }
}
