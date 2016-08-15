﻿namespace TraktApiSharp.Example.UWP.ViewModels.Movies
{
    using Models.Movies;
    using Requests.Params;
    using Services.TraktService;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Views;
    using Windows.UI.Xaml.Navigation;

    public class TrendingMoviesPageViewModel : PaginationViewModel
    {
        private TraktMoviesService Movies { get; } = TraktMoviesService.Instance;

        private ObservableCollection<TrendingMovie> _trendingMovies = new ObservableCollection<TrendingMovie>();

        public ObservableCollection<TrendingMovie> TrendingMovies
        {
            get { return _trendingMovies; }

            set
            {
                _trendingMovies = value;
                base.RaisePropertyChanged();
            }
        }

        private int _totalUsers = 0;

        public int TotalUsers
        {
            get { return _totalUsers; }

            set
            {
                _totalUsers = value;
                base.RaisePropertyChanged();
            }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (TrendingMovies != null && TrendingMovies.Count <= 0)
                await LoadPage(1, DEFAULT_LIMIT);
        }

        protected override async Task LoadPage(int? page = null, int? limit = null)
        {
            var extendedOption = new TraktExtendedOption
            {
                Full = true,
                Images = true
            };

            Busy.SetBusy(true, "Loading trending movies...");
            var traktTrendingMovies = await Movies.GetTrendingMoviesAsync(extendedOption, whichPage: page, limitPerPage: limit);

            if (traktTrendingMovies.Items != null)
            {
                TrendingMovies = traktTrendingMovies.Items;
                TotalUsers = traktTrendingMovies.TotalUserCount.GetValueOrDefault();
                CurrentPage = traktTrendingMovies.CurrentPage.GetValueOrDefault();
                ItemsPerPage = traktTrendingMovies.LimitPerPage.GetValueOrDefault();
                TotalItems = traktTrendingMovies.TotalItemCount.GetValueOrDefault();
                TotalPages = traktTrendingMovies.TotalPages.GetValueOrDefault();
                SelectedLimit = ItemsPerPage;
                SelectedPage = CurrentPage;
            }

            Busy.SetBusy(false);
        }
    }
}
