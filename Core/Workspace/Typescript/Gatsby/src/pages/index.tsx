import { graphql, Link } from "gatsby"
import Img from "gatsby-image"
import * as React from "react"

import { Query } from "../../graphql-types"
import Layout from "../components/layout"

export default class IndexPage extends React.Component<{ data: Query }> {
  public render() {
    const { siteName } = this.props.data.site.siteMetadata;
    const { allAllorsPerson } = this.props.data;

    return (
      <Layout>
        <h1>{siteName}</h1>

        {allAllorsPerson.edges.map(v =>
          <ul>
            <li><Link to={v.node.slug}>{v.node.firstName}</Link></li>
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
    allAllorsPerson {
      edges {
        node {
          slug
          firstName
        }
      }
    }
  }
`
