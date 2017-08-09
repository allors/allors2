import { Component, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { TdMediaService } from '@covalent/core';
import { MenuService, MenuItem } from '../../../allors/angular/index';

@Component({
  templateUrl: './main.component.html',
})
export class MainComponent implements AfterViewInit {

  modules: MenuItem[] = [];

  usermenu: any[] = [
    { icon: 'tune', route: '.', title: 'Account settings', },
    { icon: 'exit_to_app', route: '.', title: 'Sign out', },
  ];

  constructor(public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef, public menu: MenuService) {
    this.modules = this.menu.modules;
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }
}
