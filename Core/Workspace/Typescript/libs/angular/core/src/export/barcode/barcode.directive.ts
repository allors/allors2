import { Directive, HostListener } from '@angular/core';

import { AllorsBarcodeService } from '@allors/angular/services/core';

@Directive({
  // tslint:disable-next-line:directive-selector
  selector: '[aBarcode]',
})
export class AllorsBarcodeDirective {

  constructor(private barcodeService: AllorsBarcodeService) {
  }

  @HostListener('document:keypress', ['$event'])
  onKeypress(event: KeyboardEvent) {
    this.barcodeService.onKeypress(event);
  }
}
