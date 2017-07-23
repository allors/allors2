import { Component, Input, AfterViewInit } from '@angular/core';
import { TdMediaService } from '@covalent/core';

@Component({
  templateUrl: './edit.component.html',
})
export class EditComponent {

  constructor(public media: TdMediaService) { }
}
