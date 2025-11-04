# Minimal API Demo

This project demonstrates a simple Minimal API using .NET 9.0.\
It integrates:
- Swagger/OpenAPI for API documentation and testing
- Scalar for interactive API exploration
- Health checks for monitoring the application's status.
- Application Insights for telemetry and logging.
- Serilog for structured logging.
- Security headers for enhanced security.
- New slnx Visual Studio solution format.

# Author
Lionel Schiepers

# Useful links

| url | description |
| --- | ----------- |
| /openapi/v1.json | OpenAPI description |
| /scalar | OpenAPI interactive playgroud UI |
| /health | health checks status |
| /health-ui | health checks UI |

# Services

[SimulationService](MinimalAPIService/SimulationService/README.md)

# TODO

- Add authentication and authorization
- Endpoint that receives any json
- xUnit for integration testing
- Clean architecture
- Event-Driven architecture 
- Domain-Driven design 
- CQRS

# References

[Minimal APIs quick reference](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis)\
[10 Things I Do On Every .NET App - Scott Sauber - NDC Oslo 2025](https://www.youtube.com/watch?v=SvcRvolP2NE&t=1513s)\
[ASP.NET Core Integration Testing Best Practices](https://antondevtips.com/blog/asp-net-core-integration-testing-best-practises)\
[Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet)\
[Central Package Management](https://www.milanjovanovic.tech/blog/central-package-management-in-net-simplify-nuget-dependencies)
```
dotnet tool install CentralisedPackageConverter --global
central-pkg-converter PATH_TO_YOUR_SOLUTION_FOLDER
```
[Static Code Analysis](https://www.milanjovanovic.tech/blog/improving-code-quality-in-csharp-with-static-code-analysis)\
[xUnit v3](https://xunit.net/docs/getting-started/v3/getting-started)
```
dotnet new install xunit.v3.templates
dotnet new xunit3
```
