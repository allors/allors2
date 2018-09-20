import { Component, Input, OnInit } from '@angular/core';
import { AllorsMaterialSearchService } from '../search.service';

import { AllorsMaterialSearchField } from '../search.field';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-search-input',
  templateUrl: './search.input.component.html',
})
export class AllorsMaterialSearchInputComponent extends AllorsMaterialSearchField implements OnInit {

  constructor(searchService: AllorsMaterialSearchService) {
    super(searchService);
  }
}
