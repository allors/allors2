import { ChipsComponent } from "./components/chips";
import { MediaUploadComponent } from "./components/content/MediaUpload";
import { DefaultErrorService } from "./errors/error.service";
import { errorDialog } from "./errors/errorDialog";

export const COVALENT: any[] = [
  ChipsComponent,
  MediaUploadComponent,
 ];

export {
  DefaultErrorService,
  errorDialog,
  ChipsComponent,
  MediaUploadComponent,
};
