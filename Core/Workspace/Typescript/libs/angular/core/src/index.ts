// Meta extensions
import './meta';

import { Workspace } from '@allors/domain/system';
import { extendMedia } from './domain/Media';

export { Action } from './actions/Action';
export { ActionResult } from './actions/ActionResult';
export { ActionTarget } from './actions/ActionTarget';

export { AuthenticationConfig } from './authentication/authentication.config';
export { AuthenticationInterceptor } from './authentication/authentication.interceptor';
export { AuthenticationService } from './authentication/authentication.service';

export { AllorsBarcodeDirective } from './barcode/barcode.directive';
export { AllorsBarcodeService } from './barcode/barcode.service';

export { DateConfig } from './date/date.config';
export { DateService } from './date/date.service';

export { Filter } from './filter/Filter';
export { FilterDefinition } from './filter/FilterDefinition';
export { FilterField } from './filter/FilterField';
export { FilterFieldDefinition } from './filter/FilterFieldDefinition';
export { FilterOptions } from './filter/FilterOptions';

export { AllorsFocusDirective } from './focus/focus.directive';
export { AllorsFocusService } from './focus/focus.service';

export { AssociationField } from './forms/AssociationField';
export { Field } from './forms/Field';
export { LocalisedRoleField } from './forms/LocalisedRoleField';
export { ModelField } from './forms/ModelField';
export { RoleField } from './forms/RoleField';

export { Invoked } from './framework/responses/Invoked';
export { Loaded } from './framework/responses/Loaded';
export { Saved } from './framework/responses/Saved';

export { Context } from './framework/Context';
export { ContextService } from './framework/ContextService';
export { Database } from './framework/Database';
export { DatabaseConfig } from './framework/DatabaseConfig';
export { DatabaseService } from './framework/DatabaseService';
export { MetaService } from './framework/MetaService';
export { WorkspaceService } from './framework/WorkspaceService';

export { MediaConfig } from './media/media.config';
export { MediaService } from './media/media.service';

export { NavigationService } from './navigation/navigation.service';
export { NavigationActivatedRoute } from './navigation/NavigationActivatedRoute';

export { PanelService } from './panel/panel.service';
export { PanelManagerService } from './panel/panelmanager.service';

export { RefreshService } from './refresh/refresh.service';

export { SearchFactory } from './search/SearchFactory';
export { SearchOptions } from './search/SearchOptions';

export { SessionState } from './state/SessionState';
export { SingletonId } from './state/SingletonId';
export { UserId } from './state/UserId';

export { TestScope } from './test/test.scope';

// Domain extensions
export function extend(workspace: Workspace) {
  extendMedia(workspace);
}
