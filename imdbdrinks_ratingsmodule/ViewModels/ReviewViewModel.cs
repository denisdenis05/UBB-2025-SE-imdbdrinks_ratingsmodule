namespace imdbdrinks_ratingsmodule.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using imdbdrinks_ratingsmodule.Domain;
    using imdbdrinks_ratingsmodule.Services;

    /// <summary>
    /// ViewModel for managing reviews associated with ratings.
    /// </summary>
    public class ReviewViewModel : ViewModelBase
    {
        private readonly ReviewService reviewService;
        private ObservableCollection<Review> reviews;
        private Review selectedReview;
        private string reviewContent;
        private const int DefaultUserId = 999;

        /// <summary>
        /// Gets or sets the collection of reviews.
        /// </summary>
        public ObservableCollection<Review> Reviews
        {
            get => reviews;
            set => SetProperty(ref reviews, value);
        }

        /// <summary>
        /// Gets or sets the selected review.
        /// </summary>
        public Review SelectedReview
        {
            get => selectedReview;
            set => SetProperty(ref selectedReview, value);
        }

        /// <summary>
        /// Gets or sets the content of the review.
        /// </summary>
        public string ReviewContent
        {
            get => reviewContent;
            set => SetProperty(ref reviewContent, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewViewModel"/> class.
        /// </summary>
        /// <param name="reviewService">The service used to manage reviews.</param>
        public ReviewViewModel(ReviewService reviewService)
        {
            this.reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
            Reviews = new ObservableCollection<Review>();
        }

        /// <summary>
        /// Loads reviews for a specific rating based on its ID.
        /// </summary>
        /// <param name="ratingId">The ID of the rating whose reviews are to be loaded.</param>
        public void LoadReviewsForRating(int ratingId)
        {
            var reviewsList = reviewService.GetReviewsByRating(ratingId);
            Reviews.Clear();
            foreach (var review in reviewsList)
            {
                Reviews.Add(review);
            }
        }

        /// <summary>
        /// Adds a new review for a specified rating.
        /// </summary>
        /// <param name="ratingId">The ID of the rating to which the review is to be added.</param>
        public void AddReview(int ratingId)
        {
            Debug.WriteLine(ReviewContent);

            if (string.IsNullOrWhiteSpace(ReviewContent))
            {
                return;
            }

            var newReview = new Review
            {
                RatingId = ratingId,
                UserId = DefaultUserId,
                Content = ReviewContent,
                IsActive = true
            };

            reviewService.AddReview(newReview);
            LoadReviewsForRating(ratingId);
            ReviewContent = string.Empty;
        }

        /// <summary>
        /// Clears the content of the current review.
        /// </summary>
        public void ClearReviewContent()
        {
            ReviewContent = string.Empty;
        }
    }
}
