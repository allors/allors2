import { Injectable } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { Pull } from '../../../framework';
import { Loaded } from '../framework';

import { PanelManagerService } from './panelmanager.service';

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

    constructor(public manager: PanelManagerService) {
        manager.panels.push(this);
    }

    get isNormal(): boolean {
        return !this.manager.maximized;
    }

    get isMaximized(): boolean {
        return this.manager.maximized === this.name;
    }

    toggle() {
        this.manager.toggle(this.name);
    }

}
