namespace imdbdrinks_ratingsmodule
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Input;
    using imdbdrinks_ratingsmodule.Domain;
    using imdbdrinks_ratingsmodule.ViewHelpers;
    using imdbdrinks_ratingsmodule.ViewModels;

    /// <summary>
    /// Represents the window used to create or edit a rating.
    /// </summary>
    public sealed partial class RatingWindow : Window
    {
        private const int BottleRatingToIndexOffset = 1;
        private readonly RatingViewModel ratingViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The rating view model.</param>
        public RatingWindow(RatingViewModel viewModel)
        {
            this.InitializeComponent();
            this.ratingViewModel = viewModel;
            this.rootGrid.DataContext = viewModel;
        }

        /// <summary>
        /// Handles the event when a bottle is clicked.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The tapped routed event arguments.</param>
        private void Bottle_Click(object sender, TappedRoutedEventArgs e)
        {
            if (sender is not Image clickedImage)
            {
                return;
            }

            if (clickedImage.DataContext is not BottleAsset clickedBottle)
            {
                return;
            }

            int clickedBottleNumber = this.ratingViewModel.Bottles.IndexOf(clickedBottle) + BottleRatingToIndexOffset;
            this.ratingViewModel.UpdateBottleRating(clickedBottleNumber);
        }

        /// <summary>
        /// Handles the event when the rate button is clicked.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The routed event arguments.</param>
        private void RateButton_Click(object sender, RoutedEventArgs e)
        {
            this.ratingViewModel.AddRating();
            this.Close();
        }
    }
}
