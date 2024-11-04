# CSharpAspCoreAdoPg Endpoint Project

## Overview
This project is a C# ASP.NET Core application designed as an endpoint for load testing. It is part of a comparative analysis of various programming languages and frameworks, focusing on performance under high load conditions. The goal is to evaluate the behavior and efficiency of C# in a controlled, repeatable load-testing environment, using tools such as **Podman** for containerized deployment and **wrk** for load testing.

## Project Structure
The project is structured with a clean architecture to ensure modularity, scalability, and maintainability. Key components include:

- **Configurations**: Handles database connection configurations.
- **Controllers**: Used to setup the route, and coordinate Services calls.
- **Models**: Defines the data structures used in the application.
- **Services**: Implements the business logic for handling incoming requests and interacting with the database.
- **Wrappers**: Contains utility classes for wrapping responses and enhancing reusability.

## Key Features
- **Database Integration**: Connects to a PostgreSQL database using `Npgsql`, with a configuration-driven connection string.
- **Endpoint Performance**: Designed for optimal performance under high loads.
- **Containerized Deployment**: Easily deployable using Podman for containerization.
- **Load Testing**: Configured for performance testing using `wrk`, allowing for thorough benchmarking and analysis.

## Deployment Instructions
1. Set up the postgres container with appropriate environmenal variables: https://github.com/brycolem/benchmark/tree/main/postgres
2. Make sure the environment variable are availe to the scripts.  DATABASE, DB_USER, and DB_PWD
3. **Build and Deploye the Project**:
   Ensure you have podman and Lua installed, then build the project with:
   ```bash
   ./run_container.lua
