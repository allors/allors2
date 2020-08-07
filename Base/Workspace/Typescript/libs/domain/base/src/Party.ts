import { Meta } from '@allors/meta/generated';
import { assert } from '@allors/meta/system';
import { PartCategory } from '@allors/domain/generated';

declare module '@allors/domain/generated' {
  interface Party {
    displayName: string;
  }
}

export function extendParty(workspace) {}
