import { Component, OnInit } from '@angular/core';
import { Test } from '../decorators/test';

@Test
@Component({
  templateUrl: './second.component.html',
  styleUrls: ['./second.component.scss']
})
export class SecondComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
