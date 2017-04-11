import { Component, OnInit } from '@angular/core';
import { Scope } from '../../angular/base/Scope';
import { AllorsService } from "../allors.service";

import { UnitSample } from "../../domain"

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent extends Scope implements OnInit {

  unitSample: UnitSample;

  constructor(allors: AllorsService) 
  { 
    super("TestUnitSamples", allors.database, allors.workspace);
  }

  ngOnInit() {
    this.refresh();
  }

  protected refresh(): Promise<any> {
    return this.load({Step: 1})
        .then(() => {

            this.unitSample = this.context.objects["unitSample"] as UnitSample;
        });
  }
}
