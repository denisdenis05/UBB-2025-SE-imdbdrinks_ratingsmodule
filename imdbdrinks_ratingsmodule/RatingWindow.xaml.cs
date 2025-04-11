using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;

namespace imdbdrinks_ratingsmodule
{
    /// <summary>
    /// Window that allows users to provide a rating for a drink product.
    /// Displays a set of bottle images that can be clicked to select a rating score.
    /// </summary>
    public sealed partial class RatingWindow : Window
    {
        private readonly RatingViewModel _ratingViewModel;

        /// <summary>
        /// Initializes a new instance of the RatingWindow class.
        /// </summary>
        /// <param name="viewModel">The rating view model to use for data operations.</param>
        public RatingWindow(RatingViewModel viewModel)
        {
            InitializeComponent();
            _ratingViewModel = viewModel;
            rootGrid.DataContext = _ratingViewModel;
        }

        /// <summary>
        /// Handles the click event when a bottle icon is tapped.
        /// Updates the bottle display and saves the rating score.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event arguments containing information about the tap.</param>
        private void Bottle_Click(object sender, TappedRoutedEventArgs e)
        {
            if (sender is Image clickedImage && clickedImage.DataContext is BottleViewModel clickedBottle)
            {
                int selectedIndex = _ratingViewModel.RatingBottles.IndexOf(clickedBottle);
                _ratingViewModel.UpdateBottleRating(selectedIndex);
            }
        }

        /// <summary>
        /// Handles the click event when the Rate button is clicked.
        /// Creates and saves a new rating based on the user's selection.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event arguments for the button click.</param>
        private async void RateButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate that a rating was selected
            if (_ratingViewModel.UserRatingScore == 0)
            {
                // TODO: Message for user to select a rating
                return;
            }

            // Create a new rating object with the user's selection
            Rating newRating = new Rating
            {
                ProductId = 100, // TODO: Replace with actual product ID from context
                RatingValue = _ratingViewModel.UserRatingScore,
                UserId = _ratingViewModel.Ratings.Count + 1 // TODO: Replace with actual user ID
            };

            // Save the rating via the view model
            _ratingViewModel.AddRating(newRating);

            // Close the rating window
            this.Close();
        }
    }
}
