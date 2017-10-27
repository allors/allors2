import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit , Output } from "@angular/core";
import { FormControl } from "@angular/forms";
import { Observable } from "rxjs";

import { Field } from "@baseAngular/core";
import { ISessionObject } from "@baseDomain";
import { RoleType } from "@baseMeta";

@Component({
  selector: "a-mat-autocomplete",
  template: `
<mat-input-container fxLayout="column" fxLayoutAlign="top stretch">
  <input type="text" matInput (focusout)="focusout($event)" [formControl]="searchControl" [matAutocomplete]="usersComp" [placeholder]="label" [required]="required" [disabled]="disabled" [readonly]="readonly">
  <mat-hint *ngIf="hint">{{hint}}</mat-hint>
</mat-input-container>

<mat-autocomplete #usersComp="matAutocomplete" [displayWith]="displayFn()">
  <mat-option *ngFor="let option of filteredOptions | async" [value]="option" (onSelectionChange)="selected(option)">
  {{option[this.display]}}
  </mat-option>
</mat-autocomplete>
`,
})
export class AutocompleteComponent extends Field implements OnInit {
  @Input()
  public display: string = "display";

  @Input()
  public debounceTime: number = 400;

  @Input()
  public  options: ISessionObject[];

  @Input()
  public filter: ((search: string) => Observable<ISessionObject[]>);

  @Output()
  public onSelect: EventEmitter<ISessionObject> = new EventEmitter();

  public filteredOptions: Observable<ISessionObject[]>;

  public searchControl: FormControl = new FormControl();

  public ngOnInit(): void {
    if (this.filter) {
      this.filteredOptions = Observable.of(new Array<ISessionObject>())
        .concat(this.searchControl
          .valueChanges
          .debounceTime(this.debounceTime)
          .distinctUntilChanged()
          .switchMap((search: string) => {
            return this.filter(search);
          }));
    } else {
      this.filteredOptions = Observable.of(new Array<ISessionObject>())
        .concat(this.searchControl
          .valueChanges
          .debounceTime(this.debounceTime)
          .distinctUntilChanged()
          .map((search: string) => {
            const lowerCaseSearch: string = search.trim().toLowerCase();
            return this.options
              .filter((v: ISessionObject) => {
                const optionDisplay: string = v[this.display] ? v[this.display].toString().toLowerCase() : undefined;
                if (optionDisplay) {
                  return optionDisplay.indexOf(lowerCaseSearch) !== -1;
                }
              })
              .sort((a: ISessionObject, b: ISessionObject) => a[this.display] !== b[this.display] ? a[this.display] < b[this.display] ? -1 : 1 : 0);
          }));
    }

    this.searchControl.setValue(this.model);
  }

  public displayFn(): (val: ISessionObject) => string {
    return (val: ISessionObject) => {
      if (val) {
        return val ? val[this.display] : "";
      }
    };
  }

  public selected(option: ISessionObject): void {
    this.model = option;
    this.onSelect.emit(option);
  }

  public focusout(): void {
    if (!this.searchControl.value) {
      this.model = undefined;
      this.onSelect.emit(undefined);
    } else {
      // TODO:
    }
  }
}
