import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
    // tslint:disable-next-line:component-selector
    selector: 'a-mat-filter-search',
    templateUrl: './filter-search.component.html',
})
export class AllorsMaterialFilterSearchComponent {
    @Input()
    parent: FormGroup;
}
