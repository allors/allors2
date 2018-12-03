import { Injectable } from '@angular/core';
import { Params, Router, ActivatedRoute } from '@angular/router';
import { Pull } from '../../../framework';
import { Loaded } from '../framework';
import { AllorsPanelService } from './panel.service';

@Injectable({
    providedIn: 'root',
})
export class AllorsPanelsService {

    id: string;

    panels: AllorsPanelService[] = [];
    maximized: string;

    constructor(
        public router: Router,
        public route: ActivatedRoute
    ) { }

    prePull(pulls: Pull[]): any {
        this.panels.forEach((v) => v.prePull && v.prePull(pulls));
    }

    postPull(loaded: Loaded): any {
        this.panels.forEach((v) => v.postPull && v.postPull(loaded));
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
