import { createRemoteFileNode } from "gatsby-source-filesystem"
import { CreateNodeArgs } from "gatsby";

export async function onCreateNode({ node, cache, store, createNodeId, reporter, actions: { createNode } }: CreateNodeArgs) {

  let fileNode
  if (node.internal.type === "AllorsMedia") {

    const url = "http://localhost:5000/media/" + node.uniqueId + "/" + node.fileName;
    try {
      fileNode = await createRemoteFileNode({
        url,
        parentNodeId: node.id,
        store,
        cache,
        createNode,
        createNodeId,
        reporter
      })
    } catch (e) {
      reporter.panicOnBuild("Could not create remote file node for " + url, e);
    }
  }

  if (fileNode) {
    node.file___NODE = fileNode.id
  }
}
