namespace imdbdrinks_ratingsmodule
{
    using System;
    using imdbdrinks_ratingsmodule.ViewModels;
    using Microsoft.Extensions.Configuration;
    using Microsoft.UI.Xaml;

    /// <summary>
    /// A window for submitting or generating a review.
    /// </summary>
    public sealed partial class ReviewWindow : Window
    {
        private readonly IConfiguration configuration;
        private readonly RatingViewModel ratingViewModel;
        private readonly ReviewViewModel reviewViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewWindow"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="ratingViewModel">The rating view model.</param>
        /// <param name="reviewViewModel">The review view model.</param>
        public ReviewWindow(IConfiguration configuration, RatingViewModel ratingViewModel, ReviewViewModel reviewViewModel)
        {
            this.configuration = configuration;
            this.ratingViewModel = ratingViewModel;
            this.reviewViewModel = reviewViewModel;

            this.InitializeComponent();
            this.rootGrid.DataContext = reviewViewModel;
        }

        /// <summary>
        /// Handles the Submit Review button click event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        private async void SubmitReview_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.reviewViewModel.ReviewContent))
            {
                await EmptyReviewDialog.ShowAsync();
                return;
            }

            if (this.ratingViewModel.SelectedRating != null)
            {
                this.reviewViewModel.AddReview(this.ratingViewModel.SelectedRating.RatingId);
                this.Close();
            }
        }

        /// <summary>
        /// Handles the Generate AI Review button click event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void GenerateAIReview_Click(object sender, RoutedEventArgs e)
        {
            var aiReviewWindow = new AIReviewWindow(this.configuration, this.OnAIReviewGenerated);
            aiReviewWindow.Activate();
        }

        /// <summary>
        /// Callback when an AI-generated review is created.
        /// </summary>
        /// <param name="aiReview">The generated review text.</param>
        private void OnAIReviewGenerated(string aiReview)
        {
            this.reviewViewModel.ReviewContent = aiReview;
        }
    }
}
