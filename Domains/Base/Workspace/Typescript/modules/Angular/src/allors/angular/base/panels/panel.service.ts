import { Injectable } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { Pull } from '../../../framework';
import { Loaded } from '../framework';

import { PanelContainerService } from './panelcontainer.service';

@Injectable({
    providedIn: 'root',
})
export class PanelService {

    name: string;
    title: string;
    icon: string;
    maximizable: boolean;

    onPull: (pulls: Pull[]) => void;
    onPulled: (loaded: Loaded) => void;

    constructor(public container: PanelContainerService) {
        container.panels.push(this);
    }

    get isNormal(): boolean {
        return !this.container.maximized;
    }

    get isMaximized(): boolean {
        return this.container.maximized === this.name;
    }

    toggle() {
        this.container.toggle(this.name);
    }

}
