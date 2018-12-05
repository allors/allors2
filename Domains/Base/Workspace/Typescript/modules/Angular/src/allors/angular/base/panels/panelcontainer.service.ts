import { Injectable, Self } from '@angular/core';
import { Params, Router, ActivatedRoute } from '@angular/router';
import { Pull } from '../../../framework';
import { Loaded, ContextService, Context, DatabaseService, WorkspaceService } from '../framework';
import { PanelService } from './panel.service';

@Injectable({
    providedIn: 'root',
})
export class PanelContainerService {

    context: Context;

    id: string;

    panels: PanelService[] = [];
    maximized: string;

    constructor(
        databaseService: DatabaseService,
        workspaceService: WorkspaceService,
        public router: Router,
        public route: ActivatedRoute
    ) {
        const database = databaseService.database;
        const workspace = workspaceService.workspace;
        this.context = new Context(database, workspace);
    }

    onPull(pulls: Pull[]): any {
        this.panels.forEach((v) => v.onPull && v.onPull(pulls));
    }

    onPulled(loaded: Loaded): any {
        this.panels.forEach((v) => v.onPulled && v.onPulled(loaded));
    }

    toggle(name: string) {
        let panel;
        if (!this.maximized) {
            panel = name;
        }

        const queryParams: Params = Object.assign({}, this.route.snapshot.queryParams);
        queryParams['panel'] = panel;
        this.router.navigate(['.'], { relativeTo: this.route, queryParams });
    }

}
