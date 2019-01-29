import { ActivatedRoute } from '@angular/router';
import { ObjectType } from 'src/allors/framework';

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

  queryParam(objectType: ObjectType): string {
    const queryParamMap = this.activatedRoute.snapshot.queryParamMap;
    const match = objectType.classes.find((v) => queryParamMap.has(v.name));
    return match && queryParamMap.get(match.name);
  }
}
