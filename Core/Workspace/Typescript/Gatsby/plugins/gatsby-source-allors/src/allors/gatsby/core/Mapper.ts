import { Loaded } from "../../promise";

import { NodePluginArgs } from "gatsby"
import { ISessionObject } from "../../framework";

export class Mapper {

  constructor(private args: NodePluginArgs, private options) {
  }

  public map(loaded: Loaded) {

    const { actions: { createNode }, createNodeId, createContentDigest } = this.args;

    const session = loaded.session;
    const workspace = session.workspace;
    const ids = loaded.response.objects.map(v => v[0]);

    const contentDigestById = loaded.response.objects
      .reduce((acc, value) => {
        const id = value[0];
        const version = value[1];
        const accessControls = value.length > 2 ? value[2] : undefined;
        const deniedPermissions = value.length > 3 ? value[3] : undefined;

        let contentDigest = `allors-${id}-${version}`;
        if (accessControls) {
          contentDigest += `${accessControls}`
        }

        if (deniedPermissions) {
          contentDigest += `${deniedPermissions}`
        }

        acc[id] = createContentDigest(contentDigest);

        return acc;
      }, {});

    ids.forEach((id) => {

      const sessionObject = session.get(id);
      const workspaceObject = workspace.get(id);
      const type = workspaceObject.objectType;

      const node = {
        id: createNodeId(`allors-${workspaceObject.id}`),
        parent: null,
        children: [],
        internal: {
          type: `Allors${type.name}`,
          contentDigest: contentDigestById[id],
        }
      }

      type.roleTypes.forEach((roleType => {
        const role = workspaceObject.roleByRoleTypeId.get(roleType.id);

        if (!!role) {
          if (roleType.objectType.isUnit) {
            node[roleType.name] = role;
          } else {
            const propertyName = `${roleType.name}___NODE`;

            if (roleType.isOne) {
              if (ids.indexOf(role) > -1) {
                node[propertyName] = createNodeId(`allors-${role}`);
              }
            } else {
              const roleIds = (role as string[]).filter(w => ids.indexOf(w) > -1);
              if (roleIds.length > 0) {
                node[propertyName] = roleIds.map((w) => createNodeId(`allors-${w}`));
              }
            }
          }
        }

      }))

      type.associationTypes.forEach((associationType => {
        const association = sessionObject.getAssociation(associationType);
        const propertyName = `${associationType.name}___NODE`;

        if (associationType.isOne) {
          if (!!association) {
            node[propertyName] = createNodeId(`allors-${association.id}`);
          }
        } else {
          const associationArray = (association as ISessionObject[]);
          if (associationArray.length > 0) {
            const associationIds = associationArray.map(w => w.id);
            node[propertyName] = associationIds.map((w) => createNodeId(`allors-${w}`));
          }
        }
      }))

      createNode(node);
    })
  }

}
