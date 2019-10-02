import { Decompressor } from '../../src/allors/framework/protocol/Decompressor';
import { MetaObject } from '../../src/allors/framework/meta/MetaObject';
import { PushRequest } from '../../src/allors/framework/protocol/push/PushRequest';
import { MetaPopulation } from '../../src/allors/framework/meta/MetaPopulation';

export class PushRequestInfo {
  metaTypeByKey: Map<string, MetaObject>;

  private decompressor: Decompressor;

  constructor(pushRequest: PushRequest, meta: MetaPopulation) {
    this.metaTypeByKey = new Map();
    this.decompressor = new Decompressor();

    const keys: Set<string> = new Set();

    pushRequest.newObjects.forEach(v => {
      keys.add(v.t);
      v.roles.forEach(w => keys.add(w.t));
    });

    pushRequest.objects.forEach(v => {
      v.roles.forEach(w => keys.add(w.t));
    });

    keys.forEach(key => {
      const id = this.decompressor.read(key, v => v);
      const objectType = meta.metaObjectById.get(id);
      this.metaTypeByKey.set(key, objectType);
    });
  }

  is(key: string, metaObject: MetaObject): boolean {
    return this.metaTypeByKey.get(key) === metaObject;
  }
}
