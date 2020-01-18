import { graphql, Link } from "gatsby"
import Img from "gatsby-image"
import * as React from "react"

import { Query } from "../../graphql-types"
import Layout from "../components/layout"

export default class IndexPage extends React.Component<{ data: Query }> {
  public render() {
    const { siteName } = this.props.data.site.siteMetadata;
    const { allAllorsOrganisation } = this.props.data;

    return (
      <Layout>
        <h1>{siteName}</h1>

        {allAllorsOrganisation.edges.map(v =>
          <ul>
            <li><Link to={v.node.slug}>{v.node.name}</Link></li>
          </ul>
        )
        }
      </Layout>
    )
  }
}

export const pageQuery = graphql`
  query IndexQuery {
    site {
      siteMetadata {
        siteName
      }
    },
    allAllorsOrganisation {
      edges {
        node {
          name
          slug
        }
      }
    }
  }
`
