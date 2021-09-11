# TrailerScope

TrailerScope is web SPA application with web API backend on which you can search for movie trailers.

## Licence

All code and files holds Copyright by Dražen Šoronda (dsoronda@gmail.com).
All commercial usage of code and code derivatives are STRICTLY prohibited !!!
This is demo project and it should only be used as such.

## Requirements

- Use an API of an online movie database (e.g. IMDB or Rotten Tomatoes) (**Note** : both are payed services, will use https://imdb-api.com as free alternative)
- Use an API of an online video service (e.g. YouTube or Vimeo) (requires google Account and Google API)
- Create your own WebAPI as middleware to retrieve the results of both services and aggregate them
- Cache the aggregated data for performance
- Make the search as smart as you can
- Optionally create a web page that uses your own web API to search and show movies with trailers
- Use the development language of the vacancy you are applying for

## Technology used

* C# & Blazor
* FluentResult
* FluentValidator
* FluentAssertion
* DbLite as document db
* Flurl API Client
* SOLID principles
* Caching with database backup

## External APIs

- [IMDb-API](https://imdb-api.com) , key : `` ( since free tier is limited, create your own account )
    - [github src](https://github.com/IMDb-API/IMDbApiLib)
    - [nuget package](https://www.nuget.org/packages/IMDbApiLib)
    - [swagger](https://imdb-api.com/swagger/index.html)
- [Youtube](https://developers.google.com/youtube/v3/docs/videos)
- [The Open Movie Database](https://www.omdbapi.com/)

### How do I get set up project

* Set up requirements
    - Add Enviroment variable `IMDbApiKey` with key for IMDb-API access
    - Add Enviroment variable `TrailerScopeLiteDb` with path where to create LiteDb (ie. on Windows `d:\data\trailerscope.litedb`)
* Dependencies
    - External Movie API [IMDb-API](https://imdb-api.com)
* Database configuration - Not required, using embedded LiteDb Document db
* How to run tests - setup Enviroment variables and run tests
* Deployment instructions
~~~
git clone
dotnet build
dotnet run --project src/TrailerScopeBlazorWasm/Server/
~~~
