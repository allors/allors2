import { CreateNodeArgs, SourceNodesArgs, PluginOptions } from 'gatsby';
import { createRemoteFileNode } from "gatsby-source-filesystem"
import { GatsbySourceAllors } from './allors/GatsbySourceAllors';

export async function sourceNodes(args: SourceNodesArgs, options: PluginOptions) {

  var gatsby = new GatsbySourceAllors(args, options);
  await gatsby.sourceNodes();
}

export async function onCreateNode({ node, cache, store, createNodeId, reporter, actions: { createNode } }: CreateNodeArgs, options: PluginOptions) {

  if (node.internal.type === "AllorsMedia") {

    const fileName = node.fileName as string
    const fileComponent = fileName ? "/" + encodeURIComponent(fileName) : '';
    const url = (options["url"] as string) + "media/" + node.uniqueId + fileComponent;

    try {
      const fileNode = await createRemoteFileNode({
        url,
        parentNodeId: node.id,
        store,
        cache,
        createNode,
        createNodeId,
        reporter
      });

      node.file___NODE = fileNode.id;
    } catch (e) {
      reporter.panicOnBuild("Could not create remote file node for " + url, e);
    }
  }
}
