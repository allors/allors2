import { Workspace } from '@allors/domain/system';

export { PrintAction } from './actions/print/PrintAction';
export { PrintConfig } from './actions/print/print.config';
export { PrintService } from './actions/print/print.service';

export { FetcherService } from './fetcher/FetcherService';
export { FiltersService } from './filters/filters.service';
export { InternalOrganisationId } from './state/InternalOrganisationId';

export function extend(workspace: Workspace) {}
