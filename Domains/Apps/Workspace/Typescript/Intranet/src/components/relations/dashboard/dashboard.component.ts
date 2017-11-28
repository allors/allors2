import { Component } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { TdMediaService } from "@covalent/core";

@Component({
  template: `
<mat-toolbar>
  <div layout="row" layout-align="start center" flex>
    <button mat-icon-button tdLayoutManageListOpen [hideWhenOpened]="true" style="display: none">
          <mat-icon>arrow_back</mat-icon>
        </button>
    <span>{{title}}</span>
    <span flex></span>
    <button mat-icon-button><mat-icon>settings</mat-icon></button>
  </div>
</mat-toolbar>

<mat-card>
  <mat-card-title>Blah Blah</mat-card-title>
  <mat-card-subtitle>blah blah blah blah</mat-card-subtitle>
  <mat-divider></mat-divider>
  <mat-card-content>
    more blah blah
  </mat-card-content>
</mat-card>
`,
})
export class DashboardComponent {

  public title = "Relations Dashboard";

  constructor(public media: TdMediaService, private titleService: Title) {

      this.titleService.setTitle(this.title);
  }
}
