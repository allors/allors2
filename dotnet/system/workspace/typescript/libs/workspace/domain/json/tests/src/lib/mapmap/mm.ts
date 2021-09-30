import { MapMap } from '@allors/workspace/domain/json/system';

export function mm<k1, k2, v>(mapMap: MapMap<k1, k2, v>): Map<k1, Map<k2, v>> {
  return ((mapMap as unknown) as { mapMap: Map<k1, Map<k2, v>> }).mapMap;
}
