// Meta extensions
import './export/meta';

import { Workspace } from '@allors/domain/system';
import { extendMedia } from './export/domain/Media';

// Domain extensions
export function extend(workspace: Workspace) {
  extendMedia(workspace);
}

export { Action } from './export/actions/Action';
export { ActionResult } from './export/actions/ActionResult';
export { ActionTarget } from './export/actions/ActionTarget';

export { AuthenticationConfig } from './export/authentication/authentication.config';
export { AuthenticationInterceptor } from './export/authentication/authentication.interceptor';
export { AuthenticationService } from './export/authentication/authentication.service';

export { AllorsBarcodeDirective } from './export/barcode/barcode.directive';
export { AllorsBarcodeService } from './export/barcode/barcode.service';

export { DateConfig } from './export/date/date.config';
export { DateService } from './export/date/date.service';

export { Filter } from './export/filter/Filter';
export { FilterDefinition } from './export/filter/FilterDefinition';
export { FilterField } from './export/filter/FilterField';
export { FilterFieldDefinition } from './export/filter/FilterFieldDefinition';
export { FilterOptions } from './export/filter/FilterOptions';

export { AllorsFocusDirective } from './export/focus/focus.directive';
export { AllorsFocusService } from './export/focus/focus.service';

export { AssociationField } from './export/forms/AssociationField';
export { Field } from './export/forms/Field';
export { LocalisedRoleField } from './export/forms/LocalisedRoleField';
export { ModelField } from './export/forms/ModelField';
export { RoleField } from './export/forms/RoleField';

export { Invoked } from './export/framework/responses/Invoked';
export { Loaded } from './export/framework/responses/Loaded';
export { Saved } from './export/framework/responses/Saved';

export { Context } from './export/framework/Context';
export { ContextService } from './export/framework/ContextService';
export { Database } from './export/framework/Database';
export { DatabaseConfig } from './export/framework/DatabaseConfig';
export { DatabaseService } from './export/framework/DatabaseService';
export { MetaService } from './export/framework/MetaService';
export { WorkspaceService } from './export/framework/WorkspaceService';

export { MediaConfig } from './export/media/media.config';
export { MediaService } from './export/media/media.service';

export { NavigationService } from './export/navigation/navigation.service';
export { NavigationActivatedRoute } from './export/navigation/NavigationActivatedRoute';

export { PanelService } from './export/panel/panel.service';
export { PanelManagerService } from './export/panel/panelmanager.service';

export { RefreshService } from './export/refresh/refresh.service';

export { SearchFactory } from './export/search/SearchFactory';
export { SearchOptions } from './export/search/SearchOptions';

export { SessionState } from './export/state/SessionState';
export { SingletonId } from './export/state/SingletonId';
export { UserId } from './export/state/UserId';

export { TestScope } from './export/test/test.scope';

