import { Injectable } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { Pull } from '../../../framework';
import { Loaded } from '../framework';

import { AllorsPanelsService } from './panels.service';

@Injectable({
    providedIn: 'root',
})
export class AllorsPanelService {
    name: string;
    prePull: (pulls: Pull[]) => void;
    postPull: (loaded: Loaded) => void;

    constructor(
        public panelsService: AllorsPanelsService,
        public router: Router,
        public route: ActivatedRoute) {

        panelsService.panels.push(this);
    }

    get isMinimized(): boolean {
        return !this.panelsService.active;
    }

    get isMaximized(): boolean {
        return this.panelsService.active === this.name;
    }

    toggle() {
        let panel;
        if (!this.panelsService.active) {
            panel = this.name;
        }

        const queryParams: Params = Object.assign({}, this.route.snapshot.queryParams);
        queryParams['panel'] = panel;
        this.router.navigate(['.'], { relativeTo: this.route, queryParams });
    }

}
