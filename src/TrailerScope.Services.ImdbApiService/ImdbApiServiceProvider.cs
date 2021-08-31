using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FluentResults;
using IMDbApiLib;
using IMDbApiLib.Models;
using TrailerScope.Contracts.Services;
using TrailerScope.Domain.Entities;
using System.Linq;

namespace TrailerScope.Services.ImdbApiService
{
    public class ImdbApiServiceProvider : IImdbApiService
    {
        private readonly string _apiKey;

        public ImdbApiServiceProvider(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        private ApiLib GetApiLib() => new ApiLib(_apiKey);

        public async Task<Result<IEnumerable<MovieInfo>>> GetMoviesByTitleAsync(string title)
        {
            // throw new NotImplementedException();          
            var apiLib = GetApiLib();

            // Title Data
            SearchData data = await apiLib.SearchTitleAsync(title);
            if (!string.IsNullOrWhiteSpace(data.ErrorMessage))
                return Result.Fail<IEnumerable<MovieInfo>>(data.ErrorMessage);
            // return data;

            var list = data.ToMovieInfoList();
            return Result.Ok<IEnumerable<MovieInfo>>(list);
        }
    }

    internal static class SearchDataToMovieInfoAdapter
    {
        public static IEnumerable<MovieInfo> ToMovieInfoList(this SearchData data)
        {
            // TODO : implement this
            return data.Results.Select(x => new MovieInfo()
            {
                Title = x.Title,
                Description = x.Description,
                Poster = x.Image,
                ReleaseYear = x.Description.Contains(')')
                    ? Convert.ToInt16(x.Description.Substring(0, x.Description.IndexOf(')')).Remove('('))
                    : 0
            });
        }
    }
}