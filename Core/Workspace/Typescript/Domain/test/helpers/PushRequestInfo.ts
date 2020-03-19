import { MetaObject } from '../../src/allors/framework/meta/MetaObject';
import { PushRequest } from '../../src/allors/framework/protocol/push/PushRequest';
import { MetaPopulation } from '../../src/allors/framework/meta/MetaPopulation';
import { assert } from '../../src/allors/framework/assert';

export class PushRequestInfo {
  metaTypeByKey: Map<string, MetaObject>;

  constructor(pushRequest: PushRequest, meta: MetaPopulation) {
    this.metaTypeByKey = new Map();

    const keys: Set<string> = new Set();

    pushRequest.newObjects?.forEach(v => {
      keys.add(v.t);
      v.roles.forEach(w => keys.add(w.t));
    });

    pushRequest.objects?.forEach(v => {
      v.roles.forEach(w => keys.add(w.t));
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
