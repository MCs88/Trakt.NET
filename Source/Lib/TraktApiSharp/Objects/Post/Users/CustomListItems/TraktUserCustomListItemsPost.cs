﻿namespace TraktApiSharp.Objects.Post.Users.CustomListItems
{
    using Get.Movies;
    using Get.People;
    using Get.Shows;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Utils;

    public class TraktUserCustomListItemsPost
    {
        [JsonProperty(PropertyName = "movies")]
        public IEnumerable<TraktUserCustomListItemsPostMovie> Movies { get; set; }

        [JsonProperty(PropertyName = "shows")]
        public IEnumerable<TraktUserCustomListItemsShow> Shows { get; set; }

        [JsonProperty(PropertyName = "people")]
        public IEnumerable<TraktPerson> People { get; set; }

        public static TraktUserCustomListItemsPostBuilder Builder() => new TraktUserCustomListItemsPostBuilder();
    }

    public class TraktUserCustomListItemsPostBuilder
    {
        private TraktUserCustomListItemsPost _listItemsPost;

        public TraktUserCustomListItemsPostBuilder()
        {
            _listItemsPost = new TraktUserCustomListItemsPost();
        }

        public TraktUserCustomListItemsPostBuilder AddMovie(TraktMovie movie)
        {
            if (movie == null)
                throw new ArgumentNullException(nameof(movie));

            if (movie.Ids == null)
                throw new ArgumentNullException(nameof(movie.Ids));

            if (!movie.Ids.HasAnyId)
                throw new ArgumentException("no movie ids set or valid", nameof(movie.Ids));

            if (_listItemsPost.Movies == null)
                _listItemsPost.Movies = new List<TraktUserCustomListItemsPostMovie>();

            var existingMovie = _listItemsPost.Movies.Where(m => m.Ids == movie.Ids).FirstOrDefault();

            if (existingMovie != null)
                return this;

            (_listItemsPost.Movies as List<TraktUserCustomListItemsPostMovie>).Add(new TraktUserCustomListItemsPostMovie
            {
                Ids = movie.Ids
            });

            return this;
        }

        public TraktUserCustomListItemsPostBuilder AddShow(TraktShow show)
        {
            if (show == null)
                throw new ArgumentNullException(nameof(show));

            if (show.Ids == null)
                throw new ArgumentNullException(nameof(show.Ids));

            if (!show.Ids.HasAnyId)
                throw new ArgumentException("no show ids set or valid", nameof(show.Ids));

            if (_listItemsPost.Shows == null)
                _listItemsPost.Shows = new List<TraktUserCustomListItemsShow>();

            var existingShow = _listItemsPost.Shows.Where(s => s.Ids == show.Ids).FirstOrDefault();

            if (existingShow != null)
                return this;

            (_listItemsPost.Shows as List<TraktUserCustomListItemsShow>).Add(new TraktUserCustomListItemsShow
            {
                Ids = show.Ids
            });

            return this;
        }

        public TraktUserCustomListItemsPostBuilder AddShow(TraktShow show, int season, params int[] seasons)
        {
            if (show == null)
                throw new ArgumentNullException(nameof(show));

            if (show.Ids == null)
                throw new ArgumentNullException(nameof(show.Ids));

            if (!show.Ids.HasAnyId)
                throw new ArgumentException("no show ids set or valid", nameof(show.Ids));

            if (_listItemsPost.Shows == null)
                _listItemsPost.Shows = new List<TraktUserCustomListItemsShow>();

            var seasonsToAdd = new int[seasons.Length + 1];
            seasonsToAdd[0] = season;
            seasons.CopyTo(seasonsToAdd, 1);

            var listItemsShowSeasons = new List<TraktUserCustomListItemsShowSeason>();

            for (int i = 0; i < seasonsToAdd.Length; i++)
            {
                listItemsShowSeasons.Add(new TraktUserCustomListItemsShowSeason { Number = seasonsToAdd[i] });
            }

            var existingShow = _listItemsPost.Shows.Where(s => s.Ids == show.Ids).FirstOrDefault();

            if (existingShow != null)
            {
                existingShow.Seasons = listItemsShowSeasons;
            }
            else
            {
                var listItemsShow = new TraktUserCustomListItemsShow();
                listItemsShow.Ids = show.Ids;

                listItemsShow.Seasons = listItemsShowSeasons;
                (_listItemsPost.Shows as List<TraktUserCustomListItemsShow>).Add(listItemsShow);
            }

            return this;
        }

        public TraktUserCustomListItemsPostBuilder AddShow(TraktShow show, SAE season, params SAE[] seasons)
        {
            if (show == null)
                throw new ArgumentNullException(nameof(show));

            if (show.Ids == null)
                throw new ArgumentNullException(nameof(show.Ids));

            if (!show.Ids.HasAnyId)
                throw new ArgumentException("no show ids set or valid", nameof(show.Ids));

            if (_listItemsPost.Shows == null)
                _listItemsPost.Shows = new List<TraktUserCustomListItemsShow>();

            var seasonsAndEpisodesToAdd = new SAE[seasons.Length + 1];
            seasonsAndEpisodesToAdd[0] = season;
            seasons.CopyTo(seasonsAndEpisodesToAdd, 1);

            var listItemsShowSeasons = new List<TraktUserCustomListItemsShowSeason>();

            for (int i = 0; i < seasonsAndEpisodesToAdd.Length; i++)
            {
                var listItemsShowSingleSeason = new TraktUserCustomListItemsShowSeason { Number = seasonsAndEpisodesToAdd[i].Season };
                var listItemsShowSingleSeasonEpisodes = new List<TraktUserCustomListItemsShowEpisode>();

                for (int j = 0; j < seasonsAndEpisodesToAdd[i].Episodes.Length; j++)
                {
                    listItemsShowSingleSeasonEpisodes.Add(new TraktUserCustomListItemsShowEpisode
                    {
                        Number = seasonsAndEpisodesToAdd[i].Episodes[j]
                    });
                }

                listItemsShowSingleSeason.Episodes = listItemsShowSingleSeasonEpisodes;
                listItemsShowSeasons.Add(listItemsShowSingleSeason);
            }
            var existingShow = _listItemsPost.Shows.Where(s => s.Ids == show.Ids).FirstOrDefault();

            if (existingShow != null)
            {
                existingShow.Seasons = listItemsShowSeasons;
            }
            else
            {
                var listItemsShow = new TraktUserCustomListItemsShow();
                listItemsShow.Ids = show.Ids;

                listItemsShow.Seasons = listItemsShowSeasons;
                (_listItemsPost.Shows as List<TraktUserCustomListItemsShow>).Add(listItemsShow);
            }

            return this;
        }

        public TraktUserCustomListItemsPostBuilder AddPerson(TraktPerson person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            if (person.Ids == null)
                throw new ArgumentNullException(nameof(person.Ids));

            if (!person.Ids.HasAnyId)
                throw new ArgumentException("no person ids set or valid", nameof(person.Ids));

            if (string.IsNullOrEmpty(person.Name))
                throw new ArgumentException("person name not valid", nameof(person.Name));

            if (_listItemsPost.People == null)
                _listItemsPost.People = new List<TraktPerson>();

            var existingPerson = _listItemsPost.People.Where(p => p == person).FirstOrDefault();

            if (existingPerson != null)
                return this;

            (_listItemsPost.People as List<TraktPerson>).Add(person);

            return this;
        }

        public TraktUserCustomListItemsPost Build()
        {
            var movies = _listItemsPost.Movies;
            var shows = _listItemsPost.Shows;
            var people = _listItemsPost.People;

            var bHasNoMovies = movies == null || !movies.Any();
            var bHasNoShows = shows == null || !shows.Any();
            var bHasNoPeople = people == null || !people.Any();

            if (bHasNoMovies && bHasNoShows && bHasNoPeople)
                throw new ArgumentException("no items set");

            return _listItemsPost;
        }
    }
}
