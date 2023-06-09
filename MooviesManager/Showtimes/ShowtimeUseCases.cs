﻿using CinemaManager.Auditoriums;
using CinemaManager.Movies;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CinemaManager.Showtimes
{
    public class ShowtimeUseCases : IShowtimeUseCase
    {
        private IAuditoriumsRepository _uditoriumsRepository;
        private IMoviesReposiroty _moviesRepository;

        public ShowtimeUseCases(IAuditoriumsRepository auditoriumRepository, IMoviesReposiroty moviesRepository)
        {
            _uditoriumsRepository = auditoriumRepository;
            _moviesRepository = moviesRepository;
        }
        public async Task<ShowtimeEntity> CreateShowtime(ShowtimeCreationDto showtime, CancellationToken cancel)
        {
            var auditorium = await _uditoriumsRepository.GetAsync(showtime.AuditoriumId, cancel);
            var movie = await _moviesRepository.GetByIdAsync(showtime.MovieId);
            var newShowtime = new ShowtimeFactory(auditorium, movie, showtime.SessionDate).Create().Showtime;
            if (!auditorium.ShowtimeConflict(newShowtime))
            {
                auditorium.AddShowtime(newShowtime);
                await _uditoriumsRepository.SaveChanges(cancel);
                var createdShowtime = auditorium.Showtimes.OrderByDescending(x => x.Id).FirstOrDefault();
                return createdShowtime;
            }
            else
                throw new ArgumentException("The Showtime could not be created. It conflicts with another Showtimes in the same Auditorium. Please, use another date, time or the Auditorium.");
        }
    }
}
