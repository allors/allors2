import { Component, NgZone } from "@angular/core";
import { MatIconRegistry } from "@angular/material";
import { DomSanitizer } from "@angular/platform-browser";
import { Router } from "@angular/router";

@Component({
  selector: "app-root",
  styleUrls: ["./app.component.scss"],
  templateUrl: "./app.component.html",
})
export class AppComponent {

  constructor(private iconRegistry: MatIconRegistry, private domSanitizer: DomSanitizer, router: Router, ngZone: NgZone) {

    (window as any).allors = {router, ngZone};

    this.iconRegistry.addSvgIconInNamespace("assets", "teradata", this.domSanitizer.bypassSecurityTrustResourceUrl("assets/icons/teradata.svg"));
    this.iconRegistry.addSvgIconInNamespace("assets", "github", this.domSanitizer.bypassSecurityTrustResourceUrl("assets/icons/github.svg"));
    this.iconRegistry.addSvgIconInNamespace("assets", "covalent", this.domSanitizer.bypassSecurityTrustResourceUrl("assets/icons/covalent.svg"));
    this.iconRegistry.addSvgIconInNamespace("assets", "covalent-mark", this.domSanitizer.bypassSecurityTrustResourceUrl("assets/icons/covalent-mark.svg"));
    this.iconRegistry.addSvgIconInNamespace("assets", "teradata-ux", this.domSanitizer.bypassSecurityTrustResourceUrl("assets/icons/teradata-ux.svg"));
    this.iconRegistry.addSvgIconInNamespace("assets", "appcenter", this.domSanitizer.bypassSecurityTrustResourceUrl("assets/icons/appcenter.svg"));
    this.iconRegistry.addSvgIconInNamespace("assets", "listener", this.domSanitizer.bypassSecurityTrustResourceUrl("assets/icons/listener.svg"));
    this.iconRegistry.addSvgIconInNamespace("assets", "querygrid", this.domSanitizer.bypassSecurityTrustResourceUrl("assets/icons/querygrid.svg"));
  }
}
