import { Injectable } from '@angular/core';

import { Pull } from '@allors/data/system';
import { PanelService, Loaded, PanelManagerService } from '@allors/angular/services/core';

@Injectable()
export class PanelServiceCore extends PanelService {

    name: string;
    title: string;
    icon: string;
    expandable: boolean;

    onPull: (pulls: Pull[]) => void;
    onPulled: (loaded: Loaded) => void;

    constructor(public manager: PanelManagerService) {
        super();
        manager.panels.push(this);
    }

    get isCollapsed(): boolean {
        return !this.manager.expanded;
    }

    get isExpanded(): boolean {
        return this.manager.expanded === this.name;
    }

    toggle() {
        this.manager.toggle(this.name);
    }

}
