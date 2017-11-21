import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { TdMediaService } from '@covalent/core';

@Component({
  templateUrl: './main.component.html',
})
export class MainComponent implements AfterViewInit {

  routes = [
    { title: 'Relations', route: '/relations', icon: 'people', }];

  usermenu = [
    { icon: 'tune', route: '.', title: 'Account settings', },
    { icon: 'exit_to_app', route: '.', title: 'Sign out', },
  ];

  constructor(public media: TdMediaService) { }

   ngAfterViewInit(): void {
      this.media.broadcast();
  }
}
