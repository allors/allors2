import { Component, Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable()
export class LayoutService {

  public columns = {'display': 'grid'};

  public columns1x = {'grid-template-columns': '1fr'};

  public columns2x = {'grid-template-columns': '1fr 1fr', 'grid-gap': '1rem'};

  public columns3x = {'grid-template-columns': '1fr 1fr 1fr', 'grid-gap': '1rem'};
}
