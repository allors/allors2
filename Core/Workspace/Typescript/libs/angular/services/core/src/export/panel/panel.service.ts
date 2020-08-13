import { Injectable } from '@angular/core';

import { Pull } from '@allors/data/system';

import { Loaded } from '@allors/angular/services/core';

@Injectable()
export abstract class PanelService {
  abstract name: string;
  abstract title: string;
  abstract icon: string;
  abstract expandable: boolean;

  abstract onPull: (pulls: Pull[]) => void;
  
  abstract onPulled: (loaded: Loaded) => void;

  abstract readonly isCollapsed: boolean;

  abstract isExpanded: boolean;

  abstract toggle();
}
