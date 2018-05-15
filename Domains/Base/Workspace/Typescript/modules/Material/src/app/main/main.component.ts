import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  templateUrl: './main.component.html',
})
export class MainComponent implements AfterViewInit {

  routes = [
    { title: 'Relations', route: '/relations', icon: 'people', }
  ];

  constructor() { }

  ngAfterViewInit(): void {
  }
}
