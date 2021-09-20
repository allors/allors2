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
export { AuthenticationServiceCore } from './export/authentication/authentication.service.core';

export { AllorsBarcodeDirective } from './export/barcode/barcode.directive';
export { AllorsBarcodeServiceCore } from './export/barcode/barcode.service.core';

export { DateConfig } from './export/date/date.config';
export { DateServiceCore } from './export/date/date.service.core';

export { Filter } from './export/filter/Filter';
export { FilterDefinition } from './export/filter/FilterDefinition';
export { FilterField } from './export/filter/FilterField';
export { FilterFieldDefinition } from './export/filter/FilterFieldDefinition';
export { FilterOptions } from './export/filter/FilterOptions';

export { AllorsFocusDirective } from './export/focus/focus.directive';
export { AllorsFocusServiceCore } from './export/focus/focus.service.core';

export { AssociationField } from './export/forms/AssociationField';
export { Field } from './export/forms/Field';
export { LocalisedRoleField } from './export/forms/LocalisedRoleField';
export { ModelField } from './export/forms/ModelField';
export { RoleField } from './export/forms/RoleField';

export { MediaConfig } from './export/media/media.config';
export { MediaServiceCore } from './export/media/media.service.core';

export { NavigationServiceCore } from './export/navigation/navigation.service.core';
export { NavigationActivatedRoute } from './export/navigation/NavigationActivatedRoute';

export { RefreshServiceCore } from './export/refresh/refresh.service.core';

export { SearchFactory } from './export/search/SearchFactory';
export { SearchOptions } from './export/search/SearchOptions';

export { TestScope } from './export/test/test.scope';
