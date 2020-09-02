import { ActivatedRoute } from '@angular/router';
import { ObjectType } from '@allors/meta/system';

export class NavigationActivatedRoute {

  constructor(private activatedRoute: ActivatedRoute) {
  }

  id(): string | null {
    return this.activatedRoute.snapshot.paramMap.get('id');
  }

  panel(): string | null {
    const queryParamMap = this.activatedRoute.snapshot.queryParamMap;
    return queryParamMap.get('panel');
  }

  queryParam(objectType: ObjectType): string | null {
    const queryParamMap = this.activatedRoute.snapshot.queryParamMap;
    const match = objectType.classes.find((v) => queryParamMap.has(v.name));
    return match ? queryParamMap.get(match.name) : null;
  }
}
