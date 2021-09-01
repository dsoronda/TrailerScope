# TrailerScope

TrailerScope is web SPA application with web API backend on which you can search for movie trailers.

## Requiremenets

- Use an API of an online movie database (e.g. IMDB or Rotten Tomatoes) (**Note** : both are payed services, will use https://imdb-api.com as free alternative)
- Use an API of an online video service (e.g. YouTube or Vimeo) (requires google Account and Google API)
- Create your own WebAPI as middleware to retrieve the results of both services and aggregate them
- Cache the aggregated data for performance
- Make the search as smart as you can
- Optionally create a web page that uses your own web API to search and show movies with trailers
- Use the development language of the vacancy you are applying for

Tips

    Follow industry standards when building your own API
    Showing a lot of embedded videos on your web page increases page load time significantly

Bonus

    Add the option to share trailers on social networks;
    Make sure your website is secure and super fast;
        - ? secure how ? require API key for requests or ?
    Create a contact form to request a trailer for a movie;
        - this should be on frontend with API backend
    Surprise us! Build something we haven’t thought of or what you think is a valuable addition;

NOTE: Try to remember and briefly document the decisions you made and why you made them. We can’t wait to pick your brain.


## Technology used

* C# & Blazor
* FluentResult
* FluentValidator
* FluentAsertion
* DbLite as document db

## External APIs

- [IMDb-API](https://imdb-api.com) , key : ``
    - [github src](https://github.com/IMDb-API/IMDbApiLib)
    - [nuget package](https://www.nuget.org/packages/IMDbApiLib)
    - [swagger](https://imdb-api.com/swagger/index.html)
- [Youtube](https://developers.google.com/youtube/v3/docs/videos)
- [The Open Movie Database](https://www.omdbapi.com/)

### TODO : How do I get set up

* Summary of set up
    - Add api key for IMDb-api as enviroment variable "imdb-api-key" (secure store)
        - on Linux create file in home folder named `imdb_api_key.txt` and put there api key (as single line)

* Configuration
* Dependencies
    - External Movie API [IMDb-API](https://imdb-api.com)
    - TODO : Youtube API 
* Database configuration - Not required, using embeded nosql db
* How to run tests - See setup
* Deployment instructions
    - git clone
    - dotnet build
    - TODO: dontet run -p src/....
