
import { AllorsMaterialSearchService } from './search.service';
import { Input } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

export class AllorsMaterialSearchField {

  @Input() public name: string;

  @Input() public label: string;

  public formGroup: FormGroup;

  public formControl: FormControl;

  public get visible(): boolean {
    const isFirst =  this.searchService.first === this.formControl;
    return isFirst || this.searchService.advancedSearch;
  }

  constructor(private searchService: AllorsMaterialSearchService) {
  }

  public ngOnInit() {
    this.formGroup = this.searchService.formGroup;
    this.formControl = new FormControl();
    this.formGroup.registerControl(this.name, this.formControl);

    if (!this.searchService.first) {
      this.searchService.first = this.formControl;
    }

    if (!this.label) {
      this.label = this.name;
    }
  }
}
