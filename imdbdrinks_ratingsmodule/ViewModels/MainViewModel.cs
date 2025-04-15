// <copyright file="MainViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace imdbdrinks_ratingsmodule.ViewModels
{
    using imdbdrinks_ratingsmodule.Domain;
    using Microsoft.Extensions.Configuration;
    using Microsoft.UI.Xaml.Controls;

    /// <summary>
    /// Represents the main view model for managing ratings and reviews.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private const int MinimumValidIndex = 0;
        private const int InvalidSelectionIndex = -1;

        private readonly IConfiguration configuration;
        private RatingViewModel ratingViewModel;
        private ReviewViewModel reviewViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="configuration">The configuration settings.</param>
        /// <param name="ratingViewModel">The rating view model.</param>
        /// <param name="reviewViewModel">The review view model.</param>
        public MainViewModel(
            IConfiguration configuration,
            RatingViewModel ratingViewModel,
            ReviewViewModel reviewViewModel)
        {
            this.configuration = configuration;
            this.ratingViewModel = ratingViewModel;
            this.reviewViewModel = reviewViewModel;

            this.InitializeData();
        }

        /// <summary>
        /// Gets the configuration settings.
        /// </summary>
        public IConfiguration Configuration => this.configuration;

        /// <summary>
        /// Gets or sets the rating view model.
        /// </summary>
        public RatingViewModel RatingViewModel
        {
            get => this.ratingViewModel;
            set => this.SetProperty(ref this.ratingViewModel, value);
        }

        /// <summary>
        /// Gets or sets the review view model.
        /// </summary>
        public ReviewViewModel ReviewViewModel
        {
            get => this.reviewViewModel;
            set => this.SetProperty(ref this.reviewViewModel, value);
        }

        /// <summary>
        /// Gets the selected rating.
        /// </summary>
        public Rating? SelectedRating => this.ratingViewModel.SelectedRating;

        /// <summary>
        /// Handles the rating selection change event.
        /// </summary>
        /// <param name="listView">The list view containing the ratings.</param>
        public void HandleRatingSelection(ListView listView)
        {
            this.HandleRatingSelectionInternal(listView?.SelectedIndex ?? InvalidSelectionIndex);
        }

        /// <summary>
        /// Clears the selected rating.
        /// </summary>
        public void ClearSelectedRating()
        {
            this.ratingViewModel.SelectedRating = null!;
        }

        /// <summary>
        /// Handles the rating selection change event internally.
        /// </summary>
        /// <param name="selectedIndex">The selection index.</param>
        internal void HandleRatingSelectionInternal(int selectedIndex)
        {
            if (selectedIndex >= MinimumValidIndex && selectedIndex < this.ratingViewModel.Ratings.Count)
            {
                var selectedRating = this.ratingViewModel.Ratings[selectedIndex];
                this.ratingViewModel.SelectedRating = selectedRating;
                this.reviewViewModel.LoadReviewsForRating(selectedRating.RatingId);
            }
        }

        /// <summary>
        /// Initializes the data by loading ratings for the default product.
        /// </summary>
        private void InitializeData()
        {
            const int defaultProductId = 100;
            this.ratingViewModel.LoadRatingsForProduct(defaultProductId);
        }
    }
}
