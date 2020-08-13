import { Injectable } from '@angular/core';
import { Params, Router, ActivatedRoute } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';

import { ObjectType } from '@allors/meta/system';
import { Pull } from '@allors/data/system';

import { DatabaseService } from '../framework/DatabaseService';
import { WorkspaceService } from '../framework/WorkspaceService';
import { Context } from '../framework/Context';
import { Loaded } from '../framework/responses/Loaded';
import { PanelService } from './panel.service';


@Injectable()
export class PanelManagerService {
  context: Context;

  id: string;
  objectType: ObjectType;

  panels: PanelService[] = [];
  expanded: string;

  on$: Observable<Date>;
  private onSubject$: BehaviorSubject<Date>;

  get panelContainerClass() {
    return this.expanded ? 'expanded-panel-container' : 'collapsed-panel-container';
  }

  constructor(databaseService: DatabaseService, workspaceService: WorkspaceService, public router: Router, public route: ActivatedRoute) {
    const database = databaseService.database;
    const workspace = workspaceService.workspace;
    this.context = new Context(database, workspace);

    this.on$ = this.onSubject$ = new BehaviorSubject(new Date());
  }

  on() {
    this.onSubject$.next(new Date());
  }

  onPull(pulls: Pull[]): any {
    this.panels.forEach((v) => v.onPull && v.onPull(pulls));
  }

  onPulled(loaded: Loaded): any {
    this.panels.forEach((v) => v.onPulled && v.onPulled(loaded));
  }

  toggle(name: string) {
    let panel;
    if (!this.expanded) {
      panel = name;
    }

    const queryParams: Params = Object.assign({}, this.route.snapshot.queryParams);
    queryParams.panel = panel;
    this.router.navigate(['.'], { relativeTo: this.route, queryParams, queryParamsHandling: 'merge' });
  }
}
