const path = require(`path`)
const { createRemoteFileNode } = require(`gatsby-source-filesystem`)

const organisationTemplate = path.resolve(`./src/templates/organisation.tsx`)

/** @param {import("gatsby").CreatePagesArgs } args*/
exports.createPages = async ({ graphql, actions }) => {
  const { createPage } = actions
  const result = await graphql(
    `
        {
            allAllorsOrganisation {
                edges {
                    node {
                        slug,
                        name,
                    }
                }
            },
        }
        `
  );

  if (result.errors) {
    throw result.errors
  }

  /** @type {import("./graphql-types").AllorsOrganisationConnection } */
  const allAllorsOrganisation = result.data.allAllorsOrganisation
  allAllorsOrganisation.edges.forEach((edge) => {
    const slug = edge.node.slug;
    createPage({
      path: slug,
      component: organisationTemplate,
      context: {
        slug: slug,
      }
    })
  })
}

/** @param {import("gatsby").CreateNodeArgs} args*/
exports.onCreateNode = async ({ node, cache, store, createNodeId, reporter, actions: { createNode } }) => {

  let fileNode
  if (node.internal.type === "AllorsMedia") {
    try {
      fileNode = await createRemoteFileNode({
        url: "http://localhost:5000/image/" + node.uniqueId + "/" + node.revision,
        parentNodeId: node.id,
        store,
        cache,
        createNode,
        createNodeId,
        reporter
      })
    } catch (e) {
      console.log("ERROR: ", e);
    }
  }
  if (fileNode) {
    node.localImage___NODE = fileNode.id
  }
}
