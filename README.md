# The Purpose

The purpose of this application is to demonstrate architecture, communication patterns and coding patterns that are often used in distributed systems.

# Microservice architecture

The software uses microservice architecture with asynchronous communication pattern between the services.

# Smart Endpoints

The public endpoints in services accept incoming requests from clients and return response in synchronously.

Endpoints are implemented as RESTful OData services, which provides flexible queries and dynamic response models to the clients. OData also provides a standardized implementation of REST APIs for fast integration.

The service endpoints supports the following features.

## Flexible queries

OData provides powerful query options with possibilities of filtering and joining data.

## Dynamic viewmodels

## Optimistic locking

Updates to the system use optimistic locking to protect users from incorrect updates.