import { AfterViewInit, ChangeDetectorRef, Component } from "@angular/core";
import { Router } from "@angular/router";
import { TdMediaService } from "@covalent/core";

import { MenuItem, MenuService } from "@allors";

@Component({
  templateUrl: "./main.component.html",
})
export class MainComponent implements AfterViewInit {

  public modules: MenuItem[] = [];

  public usermenu: any[] = [
    { icon: "tune", route: ".", title: "Account settings" },
    { icon: "exit_to_app", route: ".", title: "Sign out" },
  ];

  constructor(public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef, public menu: MenuService) {
    this.modules = this.menu.modules;
  }

  public ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }
}
