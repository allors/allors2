import { MetaPopulation, assert, MetaObject } from '@allors/meta/system';
import { PushRequest } from '@allors/protocol/system';

export class PushRequestInfo {
  metaTypeByKey: Map<string, MetaObject>;

  constructor(pushRequest: PushRequest, meta: MetaPopulation) {
    this.metaTypeByKey = new Map();

    const keys: Set<string> = new Set();

    pushRequest.newObjects?.forEach(v => {
      keys.add(v.t);
      v.roles?.forEach(w => keys.add(w.t));
    });

    pushRequest.objects?.forEach(v => {
      v.roles?.forEach(w => keys.add(w.t));
    });

    keys.forEach(key => {
      const id = key;
      const objectType = meta.metaObjectById.get(id);
      assert(objectType);
      this.metaTypeByKey.set(key, objectType);
    });
  }

  is(key: string, metaObject: MetaObject): boolean {
    return this.metaTypeByKey.get(key) === metaObject;
  }
}
