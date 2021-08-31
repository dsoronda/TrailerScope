using System;

namespace TrailerScope.Services.ImdbApiService
{
    public class ImdbApiServiceProvider : IImdbApiService
    {
        private readonly string _apiKey;

        public ImdbApiServiceProvider(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }


        public bool Test()
        {
            throw new NotImplementedException();
        }
    }
}