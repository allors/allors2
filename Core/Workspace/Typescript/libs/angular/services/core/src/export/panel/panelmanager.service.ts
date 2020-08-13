import { Injectable } from '@angular/core';
import { Params, Router, ActivatedRoute } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';

import { ObjectType } from '@allors/meta/system';
import { Pull } from '@allors/data/system';

import { PanelService, Context, Loaded } from '@allors/angular/services/core';

@Injectable()
export abstract class PanelManagerService {
  abstract context: Context;

  abstract id: string;
  abstract objectType: ObjectType;

  abstract panels: PanelService[] = [];
  abstract expanded: string;

  abstract on$: Observable<Date>;

  abstract readonly panelContainerClass: string;

  abstract on(): void;

  abstract onPull(pulls: Pull[]): any;

  abstract onPulled(loaded: Loaded): any;

  abstract toggle(name: string): void;
}
