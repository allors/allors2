import { Component, Self, OnInit, HostBinding } from '@angular/core';
import { PanelService, NavigationService, RefreshService, Action, MetaService, TestScope } from '../../../../../../angular';
import { WorkEffort } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, Table, ObjectData, EditService, OverviewService } from '../../../../..';

interface Row extends TableRow {
  object: WorkEffort;
  id: string;
  takenBy: string;
  name: string;
  description: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'workeffort-overview-panel',
  templateUrl: './workeffort-overview-panel.component.html',
  providers: [PanelService]
})
export class WorkEffortOverviewPanelComponent extends TestScope implements OnInit {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: WorkEffort[];
  table: Table<Row>;

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
    public editService: EditService,
    public overviewService: OverviewService,
  ) {
    super();

    this.m = this.metaService.m;
  }

  ngOnInit() {

    this.panel.name = 'workeffort';
    // this.panel.title = 'Child Work Orders';
    this.panel.title = 'Child Work Orders';
    this.panel.icon = 'business';
    this.panel.expandable = true;

    this.edit = this.editService.edit();

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'id', sort },
        { name: 'takenBy', sort },
        { name: 'name', sort },
        { name: 'description', sort },
      ],
      actions: [
        this.edit,
      ],
      defaultAction: this.overviewService.overview(),
      autoSort: true,
      autoFilter: true,
    });

    const pullName = `${this.panel.name}_${this.m.WorkEffort.name}`;

    this.panel.onPull = (pulls) => {

      const { pull, x } = this.metaService;
      const id = this.panel.manager.id;

      pulls.push(
        pull.WorkEffort({
          name: pullName,
          object: id,
          fetch: {
            Children: {
              include: {
                TakenBy: x
              }
            }
          }
        })
      );
    };

    this.panel.onPulled = (loaded) => {
      this.objects = loaded.collections[pullName] as WorkEffort[];

      if (this.objects) {
        this.table.total = loaded.values[`${pullName}_total`] || this.objects.length;
        this.table.data = this.objects.map((v) => {
          return {
            object: v,
            id: v.WorkEffortNumber,
            takenBy: v.TakenBy.displayName,
            name: v.Name,
            description: v.Description,
          } as Row;
        });
      }
    };
  }
}
