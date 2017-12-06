import { NgModule } from "@angular/core";
import { MatButtonModule,  MatDialogModule } from "@angular/material";
import { SharedModule } from "../../shared.module";
import { NewGoodDialogComponent } from "./newgood-dialog-component";
export { NewGoodDialogComponent } from "./newgood-dialog-component";

@NgModule({
  declarations: [
    NewGoodDialogComponent,
  ],
  entryComponents: [
    NewGoodDialogComponent,
  ],
  exports: [
    NewGoodDialogComponent,
    MatDialogModule,
    SharedModule,
  ],
  imports: [
    MatDialogModule,
    SharedModule,
  ],
})
export class NewGoodDialogModule {}
