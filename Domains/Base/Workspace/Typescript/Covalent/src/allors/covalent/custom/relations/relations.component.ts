import { AfterViewInit, ChangeDetectorRef, Component, Input } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";

@Component({
  templateUrl: "./relations.component.html",
})
export class RelationsComponent implements AfterViewInit {

  public pages = [ {title: "Organisations", icon: "building", link: "organisations"}, {title: "People", icon: "people", link: "people"}];

  constructor(public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef, public activatedRoute: ActivatedRoute) {
  }

  public ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }
}
