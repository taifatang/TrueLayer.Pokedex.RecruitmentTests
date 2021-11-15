# TrueLayer.Pokedex.RecruitmentTests

## Pokedex

### Running
To run the project please navigate to the solution directory and execute:
```
dotnet run --project .\src\Hosts\Hosts.csproj
```

Swagger Endpoint: https://localhost:5001/swagger/index.html

### Testing

Unit tests and InMemory tests Only
```
dotnet test --filter FullyQualifiedName!~AcceptanceTests
```
To run all tests (The service must be running):
```
dotnet test
```

The E2E tests suite expects the service to be hosted default on the default .Net HTTPS BaseUrl; https://localhost:5001/. This could be adjusted in the `appsettings.test.json`.

## Future Improvement

1. The layout of the project follows [Microsoft's Design a DDD-oriented microservice](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice). I split them by folders to reduce cluttering.
2. I would like to propose introducing a query parameter to `pokemon/{name}` rather than having a separate endpoint for translation. As it could be considered a filter operation and could potentially reduce duplication.
3. Introduce an exception middleware that catches service exceptions and turns them into the meaningful error response.
4. Improve naming clarity.
5. Rework my FakeHttpClient to be more robust.
6. Introduce Docker, I have decided not to include the docker boilerplate as I couldn't get it to run with JetBrain Rider.
7. Improve validation with a `[FromPath]` binding
8. I have added some inline comments
