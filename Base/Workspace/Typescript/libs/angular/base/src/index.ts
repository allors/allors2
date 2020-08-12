import { Workspace } from '@allors/domain/system';
import { extend as extendCore } from '@allors/angular/core';
export function extend(workspace: Workspace) {
  extendCore(workspace);
}

export { PrintAction } from './external/actions/print/PrintAction';
export { PrintConfig } from './external/actions/print/print.config';
export { PrintService } from './external/actions/print/print.service';

export { FetcherService } from './external/fetcher/FetcherService';
export { FiltersService } from './external/filters/filters.service';
export { InternalOrganisationId } from './external/state/InternalOrganisationId';
