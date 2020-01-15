const path = require(`path`)
const { createRemoteFileNode } = require(`gatsby-source-filesystem`)

const organisationTemplate = path.resolve(`./src/templates/organisation.tsx`)

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

    const organisationEdges = result.data.allAllorsOrganisation.edges
    organisationEdges.forEach((edge) => {
        slug = edge.node.slug;
        createPage({
            path: slug,
            component: organisationTemplate,
            context: {
              slug: slug,
            }
        })
    })
}

exports.onCreateNode = async ({ node, cache, actions, store, createNodeId }) => {
  const { createNode } = actions

  let fileNode
  if (node.internal.type === "AllorsMedia") {
    try {
      fileNode = await createRemoteFileNode({
        url: "http://localhost:5000/image/" + node.uniqueId + "/" + node.revision,
        parentNodeId: node.id,
        store,
        cache,
        createNode,
        createNodeId
      })
    } catch (e) {
      console.log("ERROR: ", e);
    }
  }
    if (fileNode) {
      node.localImage___NODE = fileNode.id
    }
}
