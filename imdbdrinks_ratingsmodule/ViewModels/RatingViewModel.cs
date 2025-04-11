using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Services;

namespace imdbdrinks_ratingsmodule.ViewModels
{
    /// <summary>
    /// ViewModel class that manages the rating functionality for drink products.
    /// Handles user interactions with the rating interface and coordinates with the RatingService.
    /// </summary>
    public class RatingViewModel : INotifyPropertyChanged
    {
        // Constants
        private const int MAX_RATING_VALUE = 5;
        private const int MIN_RATING_VALUE = 0;
        private const int DEFAULT_PRODUCT_ID = 100; // TODO: Remove when real product IDs are implemented
        private const string DEFAULT_RATING_PROMPT = "Select a rating from 1 to 5";

        // Resource paths for bottle images
        private const string EmptyBottleImagePath = "ms-appx:///Assets/Bottle.png";
        private const string FilledBottleImagePath = "ms-appx:///Assets/FullBottle.png";

        private readonly RatingService _ratingService;
        private ObservableCollection<Rating> _ratings;
        private ObservableCollection<BottleViewModel> _ratingBottles;
        private int _userRatingScore;
        private string _ratingFeedbackText;

        /// <summary>
        /// Gets or sets the collection of ratings for the current product.
        /// </summary>
        public ObservableCollection<Rating> Ratings
        {
            get => _ratings;
            set
            {
                if (_ratings != value)
                {
                    _ratings = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the collection of bottle view models used for rating visualization.
        /// </summary>
        public ObservableCollection<BottleViewModel> RatingBottles
        {
            get => _ratingBottles;
            set
            {
                if (_ratingBottles != value)
                {
                    _ratingBottles = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the user's current rating score (1-5).
        /// Updates the rating feedback text when changed.
        /// </summary>
        public int UserRatingScore
        {
            get => _userRatingScore;
            set
            {
                if (_userRatingScore != value)
                {
                    _userRatingScore = value;
                    OnPropertyChanged();
                    UpdateRatingFeedback();
                }
            }
        }

        /// <summary>
        /// Gets or sets the feedback text displayed to the user based on their rating selection.
        /// </summary>
        public string RatingFeedbackText
        {
            get => _ratingFeedbackText;
            set
            {
                if (_ratingFeedbackText != value)
                {
                    _ratingFeedbackText = value;
                    OnPropertyChanged();
                }
            }
        }

        private Rating _selectedRating;
        /// <summary>
        /// Gets or sets the currently selected rating in the UI.
        /// </summary>
        public Rating SelectedRating
        {
            get => _selectedRating;
            set
            {
                if (_selectedRating != value)
                {
                    _selectedRating = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _averageRating;
        /// <summary>
        /// Gets or sets the average rating for the current product.
        /// Automatically rounds to 2 decimal places.
        /// </summary>
        public double AverageRating
        {
            get => _averageRating;
            set
            {
                const int DECIMAL_PLACES = 2;
                double roundedValue = Math.Round(value, DECIMAL_PLACES);
                if (_averageRating != roundedValue)
                {
                    _averageRating = roundedValue;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the RatingViewModel class.
        /// </summary>
        /// <param name="ratingService">The rating service used for data operations.</param>
        public RatingViewModel(RatingService ratingService)
        {
            _ratingService = ratingService;
            Ratings = new ObservableCollection<Rating>();
            RatingBottles = new ObservableCollection<BottleViewModel>();
            InitializeBottles();
            RatingFeedbackText = DEFAULT_RATING_PROMPT;
        }

        /// <summary>
        /// Initializes the bottle collection with empty bottle images.
        /// </summary>
        private void InitializeBottles()
        {
            RatingBottles.Clear();
            for (int i = 0; i < MAX_RATING_VALUE; i++)
            {
                RatingBottles.Add(new BottleViewModel { ImageSource = EmptyBottleImagePath });
            }
        }

        /// <summary>
        /// Updates the visual state of the bottles based on the selected rating.
        /// </summary>
        /// <param name="selectedIndex">The index of the selected bottle (0-based).</param>
        public void UpdateBottleRating(int selectedIndex)
        {
            for (int i = 0; i < RatingBottles.Count; i++)
            {
                RatingBottles[i].ImageSource = i <= selectedIndex ? FilledBottleImagePath : EmptyBottleImagePath;
            }
            UserRatingScore = selectedIndex + 1;
        }

        /// <summary>
        /// Updates the feedback text based on the current user rating.
        /// </summary>
        private void UpdateRatingFeedback()
        {
            RatingFeedbackText = UserRatingScore > MIN_RATING_VALUE 
                ? $"You selected {UserRatingScore} out of {MAX_RATING_VALUE} bottles" 
                : DEFAULT_RATING_PROMPT;
        }

        /// <summary>
        /// Loads all ratings for a specific product and updates the average rating.
        /// </summary>
        /// <param name="productId">The ID of the product to load ratings for.</param>
        public void LoadRatingsForProduct(long productId)
        {
            var ratings = _ratingService.GetRatingsByProduct(productId);
            Ratings.Clear();
            // Reverse the order so that the newest rating appears first
            foreach (var rating in ratings.Reverse())
            {
                Ratings.Add(rating);
            }
            AverageRating = _ratingService.GetAverageRating(productId);
        }

        /// <summary>
        /// Adds a new rating and reloads the ratings for the product.
        /// </summary>
        /// <param name="rating">The rating to add.</param>
        public void AddRating(Rating rating)
        {
            _ratingService.CreateRating(rating);
            // Reload the ratings so that the new one appears at the top
            LoadRatingsForProduct(rating.ProductId);
        }

        /// <summary>
        /// Event that is raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// ViewModel class for individual bottle images in the rating interface.
    /// </summary>
    public class BottleViewModel : INotifyPropertyChanged
    {
        private string _imageSource;
        
        /// <summary>
        /// Gets or sets the image source path for the bottle.
        /// </summary>
        public string ImageSource
        {
            get => _imageSource;
            set
            {
                if (_imageSource != value)
                {
                    _imageSource = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageSource)));
                }
            }
        }

        /// <summary>
        /// Event that is raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
