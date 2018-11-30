import { Injectable } from '@angular/core';
import { AllorsPanelService } from './panel.service';
import { Pull } from '../../../framework';
import { Loaded } from '../framework';

@Injectable({
    providedIn: 'root',
})
export class AllorsPanelsService {
    id: string;
    active: string;
    panels: AllorsPanelService[] = [];

    prePull(pulls: Pull[]): any {
        this.panels.forEach((v) => v.prePull && v.prePull(pulls));
    }

    postPull(loaded: Loaded): any {
        this.panels.forEach((v) => v.postPull && v.postPull(loaded));
    }

}
