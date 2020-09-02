import '@allors/angular/core';

import { Workspace } from '@allors/domain/system';
import { extend as extendCore } from '@allors/angular/core';
export function extend(workspace: Workspace) {
  extendCore(workspace);
}

export { PrintAction } from './export/actions/print/PrintAction';
export { PrintService, PrintConfig } from './export/actions/print/print.service';

export { FetcherService } from './export/fetcher/FetcherService';
export { InternalOrganisationId } from './export/state/InternalOrganisationId';

export { Filters } from './export/filters/filters';
