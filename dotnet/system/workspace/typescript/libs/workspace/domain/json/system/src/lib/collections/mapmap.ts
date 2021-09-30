export class MapMap<k1, k2, v> {
  private mapMap: Map<k1, Map<k2, v>>;

  constructor() {
    this.mapMap = new Map();
  }

  keys() {
    return this.mapMap.keys();
  }

  get(key1: k1, key2: k2): v | undefined {
    return this.mapMap.get(key1)?.get(key2);
  }

  set(key1: k1, key2: k2, value: v | undefined): this {
    let map = this.mapMap.get(key1);

    if (value === undefined) {
      if (map !== undefined) {
        map.delete(key2);
        if (map.size === 0) {
          this.mapMap.delete(key1);
        }
      }
    } else {
      if (map === undefined) {
        map = new Map();
        this.mapMap.set(key1, map);
      }

      map.set(key2, value);
    }

    return this;
  }
}
