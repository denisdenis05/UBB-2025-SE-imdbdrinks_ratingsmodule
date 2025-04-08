using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Services;

namespace imdbdrinks_ratingsmodule.ViewModels
{
    /// <summary>
    /// ViewModel for managing review operations and data binding
    /// </summary>
    public class ReviewViewModel : INotifyPropertyChanged
    {
        private readonly ReviewService _reviewService;
        private ObservableCollection<Review> _reviews;
        private Review? _selectedReview;

        /// <summary>
        /// Collection of reviews for the selected rating
        /// </summary>
        public ObservableCollection<Review> Reviews
        {
            get => _reviews;
            set 
            { 
                if (_reviews != value)
                {
                    _reviews = value; 
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Currently selected review
        /// </summary>
        public Review? SelectedReview
        {
            get => _selectedReview;
            set 
            { 
                if (_selectedReview != value)
                {
                    _selectedReview = value; 
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the ReviewViewModel class
        /// </summary>
        /// <param name="reviewService">Service for review data operations</param>
        public ReviewViewModel(ReviewService reviewService)
        {
            _reviewService = reviewService;
            _reviews = new ObservableCollection<Review>();
        }

        /// <summary>
        /// Loads reviews associated with the specified rating
        /// </summary>
        /// <param name="ratingId">ID of the rating to load reviews for</param>
        public void LoadReviewsForRating(long ratingId)
        {
            var reviews = _reviewService.GetReviewsByRating(ratingId);
            Reviews.Clear();
            
            foreach (var review in reviews)
            {
                Reviews.Add(review);
            }
        }

        /// <summary>
        /// Adds a new review and refreshes the review list
        /// </summary>
        /// <param name="review">The review to add</param>
        public void AddReview(Review review)
        {
            _reviewService.CreateReview(review);
            LoadReviewsForRating(review.RatingId);
        }

        /// <summary>
        /// Event for property change notifications
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Notifies subscribers that a property value has changed
        /// </summary>
        /// <param name="propertyName">Name of the property that changed</param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
