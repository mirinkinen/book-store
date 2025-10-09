# GraphQL Demo

## Operations

### Queries

```
query {
  authors {
    nodes {
      id
      firstName
      lastName
    }
  }
}
```

#### Pagination

```
query {
  authors(first: 2) {
    nodes {
      id
      firstName
      lastName
    }
    pageInfo {
      hasNextPage
      endCursor
    }
  }
}

```
  

#### Filtering

```
query {
  authors(where: { birthdate: { lt: "1900-01-01" } }) {
    nodes {
      id
      firstName
      lastName
    }
  }
}
```

#### Sorting 
```
query {
  authors(order: [{ firstName: ASC }]) {
    nodes {
      lastName
    }
  }
}
```

#### Query Arguments

```
query ($includeBooks: Boolean!) {
  authors(where: { lastName: { contains: "King" } }) {
    nodes {
      firstName
      lastName
      books(where: { title: { nstartsWith: "Book" } })
        @include(if: $includeBooks) {
        nodes {
          title
          datePublished
          price
        }
      }
    }
  }
}
```

### Mutations

- Updates data

```
mutation {
  createAuthor(
    command: {
      firstName: "Mikko"
      lastName: "Mallikas"
      birthdate: "1980-01-01"
      organizationId: "5D8E6753-1479-408E-BB3D-CB3A02BE486C"
    }
  ) {
    author {
      id
    }
    errors {
      ... on UserError {
        message
        code
      }
    }
  }
}
```

### Subscriptions

- Real-time messaging

```
subscription {
  onAuthorCreated {
    id    
    firstName
    lastName
    birthdate
    organizationId    
  }
}
```

## Aligns with CQRS
- Concurrent queries
- Sequential mutations

```
query {
  q1: concurrentQuery
  q2: concurrentQuery
  q3: concurrentQuery
}

mutation {
  m1: concurrentMutation { string }
  m2: concurrentMutation { string }
}
```

## Data loaders

```
query {
  authors {
    nodes {
      __typename
      id
      firstName
      lastName
      books {
        nodes {
          __typename
          id
          title
        }
      }
    }
  }
}
```

## Relay
- [GitHub](https://github.com/facebook/relay)
- JavaScript framework
- Connections
- Paging
- Global identifiers

```
query {
  nodes(
    ids: ["QXV0aG9yOjSUao71h7JGpsNSLcNdju8=", "Qm9vazq9xSWhjk+UR5w2duQB+08k"]
  ) {
    ... on Author {
      id
      firstName
      lastName
    }

    ... on Book {
      id
      title
      datePublished
    }
  }
}
```

## Versioning

- Evolving schema
- Deprecation