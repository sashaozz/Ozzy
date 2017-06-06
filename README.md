
# Ozzy - Framework for complex backend applications

## Building
To build Ozzy just open Ozzy.sln in Visual Studio and build.

[![Build status](https://ci.appveyor.com/api/projects/status/6jgm738fj2td9ujh?svg=true)](https://ci.appveyor.com/project/sashaozz/ozzy)
[![License LGPLv3](https://img.shields.io/badge/license-LGPLv3-green.svg)](http://www.gnu.org/licenses/lgpl-3.0.html)
[![Latest version](https://img.shields.io/nuget/v/ozzy.svg)](https://www.nuget.org/packages?q=ozzy)

# Overview
Ozzy is an framework for building complex backend applications in .Net. with following features

- Easy Clustered deployment for High Availability and scalability
- [Event-Driven](http://microservices.io/patterns/data/event-driven-architecture.html) architecture and [**Domain Event**](https://martinfowler.com/eaaDev/DomainEvent.html) pattern implementation out of the box
- [Saga (Process Manager)](https://msdn.microsoft.com/en-us/library/jj591569.aspx) pattern implementation for handling complex workflows
- Internal message bus
- Dozens of useful implementations for
    - Background Tasks and Jobs
    - Scheduled Tasks    
    - Feature Flags
    - Distributed Locks
    - Queues
    - and more
- Admin UI (based on awesome [AdminLTE](https://github.com/almasaeed2010/AdminLTE/) template) which is build using React.js and Typescript

Ozzy Framework is build with Asp.Net Core and can be deployed and used on Windows, Linux, Docker and Cloud as monolithic or microservice application.

# How to get
Best way to start with Ozzy framework is via nuget packages.
Prerelease packages from CI system are published to Myget feed.
https://www.myget.org/F/sashaozz/api/v3/index.json

# Status

Currently Ozzy framework can be considered in late alpha stage. However it is already used in production for my personal projects. To become beta release some internal decisions and API should be fixed which is not here yet but awaited soon.

# Performance and Scalability

Ozzy framework is designed for Scalability and Performance from the ground up. Exact numbers will be provided after excessive performance testing but it is already capable to deliver tens of thousands of mixed read/write operations per second per node.

# Documentation
The work on documentation just begun. Please see folder [Docs](/sashaozz/Ozzy/tree/master/docs) for first documents with description of the system.
