import { Injectable } from '@angular/core';
import { Params, Router, ActivatedRoute } from '@angular/router';
import { Pull, ObjectType } from '../../../framework';
import { Loaded, Context, DatabaseService, WorkspaceService } from '../framework';
import { PanelService } from './panel.service';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PanelManagerService {

  context: Context;

  id: string;
  objectType: ObjectType;

  panels: PanelService[] = [];
  expanded: string;

  on$: Observable<Date>;
  private onSubject$: BehaviorSubject<Date>;

  constructor(
    databaseService: DatabaseService,
    workspaceService: WorkspaceService,
    public router: Router,
    public route: ActivatedRoute
  ) {
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
    queryParams['panel'] = panel;
    this.router.navigate(['.'], { relativeTo: this.route, queryParams, queryParamsHandling: 'merge' });
  }

}
