using Microsoft.UI.Xaml;
using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Services;
using imdbdrinks_ratingsmodule.ViewModels;
using imdbdrinks_ratingsmodule.Domain;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
namespace imdbdrinks_ratingsmodule
{

    public sealed partial class MainWindow : Window
    {

        private readonly IConfiguration _configuration;
        // Public properties for binding.
        public RatingViewModel RatingViewModel { get; }
        public ReviewViewModel ReviewViewModel { get; }

        public MainWindow(IConfiguration configuration, RatingViewModel ratingViewModel, ReviewViewModel reviewViewModel)
        {
            _configuration = configuration;
            RatingViewModel = ratingViewModel;
            ReviewViewModel = reviewViewModel;

            this.InitializeComponent();

            // Unique connection string for MySql database (change accordingly)
            Debug.WriteLine(configuration);



            // Create repository instances.
            //var ratingRepo = new HardCodedRatingRepository();
            //var reviewRepo = new HardCodedReviewRepository();

            // Create service instances.

            // Instantiate ViewModels.

            // Load initial data for product ID 100
            LoadInitialData();
        }

        /// <summary>
        /// Loads initial data for the main window
        /// </summary>
        private void LoadInitialData()
        {
            const int DefaultProductId = 100;
            RatingViewModel.LoadRatingsForProduct(DefaultProductId);
        }

        /// <summary>
        /// Opens the review window when the add review button is clicked
        /// </summary>
        private void AddReview_Click(object sender, RoutedEventArgs e)
        {
            if (RatingViewModel.SelectedRating != null)
            {
                var reviewWindow = new ReviewWindow(_configuration, RatingViewModel, ReviewViewModel);
                reviewWindow.Activate();
            }
            else
            {
                NoRatingSelectedDialog.ShowAsync();
            }
        }

        /// <summary>
        /// Opens the rating window when the add rating button is clicked
        /// </summary>
        private void AddRating_Click(object sender, RoutedEventArgs e)
        {
            var ratingWindow = new RatingWindow(RatingViewModel);
            RatingViewModel.SelectedRating = null;
            ratingWindow.Activate();
        }
     
        /// <summary>
        /// Updates the selected rating and loads associated reviews when a rating is selected
        /// </summary>
        private void RatingSelection_Changed(object sender, RoutedEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedIndex >= 0)
            {
                var selectedRating = RatingViewModel.Ratings[listView.SelectedIndex];
                RatingViewModel.SelectedRating = selectedRating;
                ReviewViewModel.LoadReviewsForRating(selectedRating.RatingId);
            }
        }

    }
}
