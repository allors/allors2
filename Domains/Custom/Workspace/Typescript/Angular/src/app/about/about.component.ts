import { Observable } from 'rxjs/Rx';
import { Component, OnInit } from '@angular/core';
import { Scope } from '../../angular/base/Scope';

import { AllorsService } from '../allors.service';

import { UnitSample } from '../../domain';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent implements OnInit {

  scope: Scope;
  unitSample: UnitSample;

  constructor(allors: AllorsService) {
    this.scope = new Scope('TestUnitSamples', allors.database, allors.workspace);
  }

  ngOnInit() {
    this.refresh().subscribe();
  }

  protected refresh(): Observable<any> {
    return this.scope
        .load({Step: 1})
        .do(() => {
            this.unitSample = this.scope.objects['unitSample'] as UnitSample;
        });
  }
}
