import { Component, Self, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { AllorsMaterialSearchService } from './search.service';
import { Subscription } from 'rxjs';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-search',
  templateUrl: './search.component.html',
  providers: [AllorsMaterialSearchService]
})
export class AllorsMaterialSearchComponent implements OnDestroy {
  @Output() public values: EventEmitter<any> = new EventEmitter<any>();

  private subscription: Subscription;

  constructor(@Self() public searchService: AllorsMaterialSearchService) {

    this.subscription = searchService.formGroup.valueChanges
      .subscribe((v) => {
        this.values.emit(v);
      });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
      delete(this.subscription);
    }
  }
}
