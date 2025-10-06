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
  

#### Filtering, sorting

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

### Subscriptions

- Real-time messaging


## Aligns with CQRS
- Concurrent queries
- Sequential mutations

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
- JavaScript framework
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

# QXV0aG9yOjSUao71h7JGpsNSLcNdju8=  --> Author:4\x94j\x8Eõ\x87²F¦ÃR-Ã]\x8Eï
# Qm9vazq9xSWhjk+UR5w2duQB+08k      --> Book:½Å%¡\x8EO\x94G\x9C6vä\x01ûO$
```

- Connections
- Paging

## Versioning

- Evolving schema
- Deprecation