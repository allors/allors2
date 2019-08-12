import { Component } from '@angular/core';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(iconRegistry: MatIconRegistry, sanitizer: DomSanitizer) {
    iconRegistry.addSvgIcon('allors', sanitizer.bypassSecurityTrustResourceUrl('assets/icons/allors.svg'));
    iconRegistry.addSvgIcon('allors-dark', sanitizer.bypassSecurityTrustResourceUrl('assets/icons/allors-dark.svg'));
    iconRegistry.addSvgIcon('allors-mark', sanitizer.bypassSecurityTrustResourceUrl('assets/icons/allors-mark.svg'));
  }}
