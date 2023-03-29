using CinemaManager.Movies;
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
        private readonly string movieListCackeKey = "movieList";
        private readonly string _movieApiKey = "68e5fbda-9ec9-4858-97b2-4a8349764c63";
        private readonly string _moviesApiUrl = "http://localhost:7172";
        private readonly IMemoryCache _cache;

        public MoviesRepository(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<MovieEntity> GetByIdAsync(string id, CancellationToken cancel)
        {
            try
            {
                var availableMovies = await GetAllMovies(cancel);
                var requestedMovie = availableMovies.FirstOrDefault(x => x.Id == id);

                return ConvertToMovieEntity(requestedMovie);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        private async Task<List<MovieDto>> GetAllMovies(CancellationToken cancel)
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
                            StoreMoviesInCache(availableMovies);
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

        private void StoreMoviesInCache(List<MovieDto> availableMovies)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(2))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(24))
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(1024);
            _cache.Set(movieListCackeKey, availableMovies, cacheEntryOptions);
        }

        private List<MovieDto> GetAvailableMoviesFromCache()
        {
            if (_cache.TryGetValue(movieListCackeKey, out List<MovieDto> availableMovies))
            {
                return availableMovies;
            }
            throw new Exception("There are no movies available. Please, try again later");
        }
    }
}
