import { GatsbySourceAllors } from './allors/GatsbySourceAllors';
import { CreateNodeArgs, PluginOptions } from 'gatsby';
import { SourceMedia } from './allors/gatsby';

export async function sourceNodes(args: SourceNodesArgs, options: PluginOptions) {

  var gatsby = new GatsbySourceAllors(args, options);
  await gatsby.sourceNodes();
}

export async function onCreateNode(args: CreateNodeArgs, options: PluginOptions) {

  var sourceMedia = new SourceMedia(args, options);
  await sourceMedia.onCreateNode();
}
