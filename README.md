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
To run all tests (The Host.csproj must be running):
```
dotnet test
```

The E2E tests suite expects the service to be hosted on the default .Net HTTPS BaseUrl; https://localhost:5001/. This could be configured in the `appsettings.test.json`.

## Comments

1. The layout of the project follows [Microsoft's Design a DDD-oriented microservice](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice). I split them by folders to reduce cluttering in this exercise.
2. I have added some inline comments.

## Improvements

1. I would like to propose introducing a query parameter to `pokemon/{name}` rather than having a separate endpoint for translation. As it could be considered a filter operation and could potentially reduce duplication.
2. Introduce an exception middleware that catches service exceptions and turns them into the meaningful error response.
3. Improve naming clarity.
4. Rework my `FakeHttpClient` to be more robust.
5. Introduce Docker, I have decided not to include the docker boilerplate as I couldn't get it to run with JetBrain Rider.
6. Improve validation with a `[FromPath]` binding.
7. Includes case sensitivity consideration e.g. poke api supports lower case only.
8. FunTranslation removes format/control characters i.e /n, maybe format should be retained?
9. Identifiers to correlate logs.

## Fun Facts
* Searching `pikachu` returns a chinese description which is unaffected by `Shakespeare`.
* FunTranslation has a duplicate set of endpoints for `HttpGet` and `HttpPost`
