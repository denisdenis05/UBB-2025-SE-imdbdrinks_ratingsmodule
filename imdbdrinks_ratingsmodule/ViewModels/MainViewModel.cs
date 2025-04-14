using imdbdrinks_ratingsmodule.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.UI.Xaml.Controls;

namespace imdbdrinks_ratingsmodule.ViewModels
{
    /// <summary>
    /// Represents the main view model for managing ratings and reviews.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IConfiguration configuration;
        private RatingViewModel ratingViewModel;
        private ReviewViewModel reviewViewModel;

        /// <summary>
        /// Gets the configuration settings.
        /// </summary>
        public IConfiguration Configuration => configuration;

        /// <summary>
        /// Gets or sets the rating view model.
        /// </summary>
        public RatingViewModel RatingViewModel
        {
            get => ratingViewModel;
            set => SetProperty(ref ratingViewModel, value);
        }

        /// <summary>
        /// Gets or sets the review view model.
        /// </summary>
        public ReviewViewModel ReviewViewModel
        {
            get => reviewViewModel;
            set => SetProperty(ref reviewViewModel, value);
        }

        /// <summary>
        /// Gets the selected rating.
        /// </summary>
        public Rating SelectedRating => ratingViewModel.SelectedRating;

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

            InitializeData();
        }

        /// <summary>
        /// Initializes the data by loading ratings for the default product.
        /// </summary>
        private void InitializeData()
        {
            const int defaultProductId = 100;
            ratingViewModel.LoadRatingsForProduct(defaultProductId);
        }

        /// <summary>
        /// Handles the rating selection change event.
        /// </summary>
        /// <param name="listView">The list view containing the ratings.</param>
        public void HandleRatingSelection(ListView listView)
        {
            if (listView?.SelectedIndex >= 0)
            {
                var selectedRating = ratingViewModel.Ratings[listView.SelectedIndex];
                ratingViewModel.SelectedRating = selectedRating;
                reviewViewModel.LoadReviewsForRating(selectedRating.RatingId);
            }
        }

        /// <summary>
        /// Clears the selected rating.
        /// </summary>
        public void ClearSelectedRating()
        {
            ratingViewModel.SelectedRating = null;
        }
    }
}
