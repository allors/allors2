import { Component, Input, OnInit } from '@angular/core';
import { AllorsMaterialSearchService } from '../search.service';
import { FormControl } from '@angular/forms';

import { ISessionObject } from '../../../../../framework';
import { AllorsMaterialSearchField } from '../search.field';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-search-select',
  templateUrl: './search.select.component.html',
})
export class AllorsMaterialSearchSelectComponent extends AllorsMaterialSearchField implements OnInit {

  @Input() public options: ISessionObject[];

  @Input() public display = 'display';

  formControl: FormControl;

  constructor(searchService: AllorsMaterialSearchService) {
    super(searchService);
  }
}
