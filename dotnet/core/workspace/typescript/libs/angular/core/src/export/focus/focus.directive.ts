import { Directive, ElementRef, Inject, PLATFORM_ID, Input, OnDestroy } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { timer, Subscription } from 'rxjs';
import { filter, switchMap, tap } from 'rxjs/operators';

import { AllorsFocusService } from '@allors/angular/services/core';


@Directive({
  // tslint:disable-next-line:directive-selector
  selector: '[aFocus]',
})
export class AllorsFocusDirective implements OnDestroy {
  @Input() aFocusTrigger: any;

  private subscription: Subscription;

  constructor(
    private readonly element: ElementRef<HTMLElement>,
    private readonly focusService: AllorsFocusService,
    @Inject(PLATFORM_ID) platformId: string,
  ) {
    const inBrowser = isPlatformBrowser(platformId);

    this.subscription = this.focusService.focus$
      .pipe(
        switchMap((v) =>
          timer(100).pipe(
            filter(() => {
              return inBrowser && v === this.aFocusTrigger;
            }),
          ),
        ),
      )
      .subscribe(() => {
        try {
          setTimeout(() => {
            this.element.nativeElement.focus();
          }, 0);
        } catch {}
      });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
