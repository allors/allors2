import { Injectable } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';

@Injectable()
export class AllorsMaterialSearchService {

  public formGroup: FormGroup;

  public first: FormControl;

  public advancedSearch: boolean;

  constructor() {
    this.formGroup = new FormGroup({});
    this.advancedSearch = false;
  }

}
