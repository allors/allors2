import { Injectable } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { Pull } from '../../../framework';
import { Loaded } from '../framework';

import { PanelsService } from './panels.service';

@Injectable({
    providedIn: 'root',
})
export class AllorsPanelService {
    name: string;
    title: string;
    icon: string;
    maximizable: boolean;

    prePull: (pulls: Pull[]) => void;
    postPull: (loaded: Loaded) => void;

    constructor(
        public panelsService: PanelsService
    ) {

        panelsService.panels.push(this);
    }

    get isMinimized(): boolean {
        return !this.panelsService.maximized;
    }

    get isMaximized(): boolean {
        return this.panelsService.maximized === this.name;
    }

    toggle() {
        this.panelsService.toggle(this.name);
    }

}
