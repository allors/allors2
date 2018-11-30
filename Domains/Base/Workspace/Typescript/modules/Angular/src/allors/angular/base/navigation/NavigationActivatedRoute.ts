import { ActivatedRoute } from '@angular/router';
import { ObjectType, MetaObjectType } from 'src/allors/framework';

export class NavigationActivatedRoute {

  constructor(private activatedRoute: ActivatedRoute) {
  }

  id(): string {
    return this.activatedRoute.snapshot.paramMap.get('id');
  }

  panel(): string {
    const queryParamMap = this.activatedRoute.snapshot.queryParamMap;
    return queryParamMap.get('panel');
  }

  queryParam(objectTypeOrMetaObjectTypes: ObjectType | MetaObjectType): string {
    const queryParamMap = this.activatedRoute.snapshot.queryParamMap;

    const objectType = objectTypeOrMetaObjectTypes instanceof ObjectType ? objectTypeOrMetaObjectTypes : objectTypeOrMetaObjectTypes.objectType;
    const match = objectType.classes.find((v) => queryParamMap.has(v.name));
    return match && queryParamMap.get(match.name);
  }
}
