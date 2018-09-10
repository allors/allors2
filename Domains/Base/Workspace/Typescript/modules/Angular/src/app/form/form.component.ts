import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  templateUrl: './form.component.html',
})
export class FormComponent implements OnInit {

  constructor(private title: Title) { }

  public ngOnInit() {
    this.title.setTitle('Form');
  }
}
