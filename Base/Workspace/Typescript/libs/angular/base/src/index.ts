import '@allors/angular/core';

import { Workspace } from '@allors/domain/system';
import { extend as extendCore } from '@allors/angular/core';
export function extend(workspace: Workspace) {
  extendCore(workspace);
}

export { PrintAction } from './export/actions/print/PrintAction';
export { PrintConfig } from './export/actions/print/print.config';
export { PrintService } from './export/actions/print/print.service';

export { FetcherService } from './export/fetcher/FetcherService';
export { FiltersService } from './export/filters/filters.service';
export { InternalOrganisationId } from './export/state/InternalOrganisationId';
