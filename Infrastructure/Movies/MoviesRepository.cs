using CinemaManager.Movies;
using Infrastructure.Cache;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace CinemaInfrastructure.Movies
{
    public class MoviesRepository : IMoviesReposiroty
    {
        private readonly string movieListCacheKey = "movieList";
        private readonly string _movieApiKey = "68e5fbda-9ec9-4858-97b2-4a8349764c63";
        private readonly string _moviesApiUrl = "http://localhost:7172";
        private readonly ICacheService _cache;

        public MoviesRepository(ICacheService cache)
        {
            _cache = cache;
        }

        public async Task<MovieEntity> GetByIdAsync(string id)
        {
            try
            {
                var availableMovies = await GetAllMovies();
                var requestedMovie = availableMovies.FirstOrDefault(x => x.Id == id);

                return ConvertToMovieEntity(requestedMovie);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        private async Task<List<MovieDto>> GetAllMovies()
        {
            try
            {
                var cancelationToken = new CancellationTokenSource().Token;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.BaseAddress = new Uri(_moviesApiUrl);
                    client.DefaultRequestHeaders.Add("X-Apikey", _movieApiKey);
                    using (HttpResponseMessage response = await client.GetAsync("v1/movies", cancelationToken))
                    {
                        using (HttpContent content = response.Content)
                        {
                            var moviesAsJsonString = await response.Content.ReadAsStringAsync();
                            var availableMovies = JsonConvert.DeserializeObject<List<MovieDto>>(moviesAsJsonString);
                            CacheMovies(availableMovies);
                            return availableMovies;
                        }
                    }
                }
            }
            catch
            {
                return GetAvailableMoviesFromCache();
            }
        }

        private MovieEntity ConvertToMovieEntity(MovieDto movieDto)
        {
            if (movieDto != null)
            {
                return new MovieEntity
                {
                    Title = movieDto.Title,
                    ImdbId = $"{movieDto.Title}-{movieDto.ImDbRating}-{movieDto.ImDbRatingCount}",
                    Stars = movieDto.ImDbRating,
                    ReleaseDate = new DateTime(int.Parse(movieDto.Year), 1, 1)
                };
            }
            return null;
        }

        private void CacheMovies(List<MovieDto> availableMovies)
        {
            var expirationTime = DateTimeOffset.Now.AddHours(24);
            _cache.SetData<List<MovieDto>>(movieListCacheKey, availableMovies, expirationTime);
        }

        private List<MovieDto> GetAvailableMoviesFromCache()
        {
            var cachedMovies = _cache.GetData<List<MovieDto>>(movieListCacheKey);

            if (cachedMovies != null) return cachedMovies;

            throw new Exception("There are no movies available. Please, try again later");
        }
    }
}
