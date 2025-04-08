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
    /// ViewModel for managing rating operations and data binding
    /// </summary>
    public class RatingViewModel : INotifyPropertyChanged
    {
        private readonly RatingService _ratingService;
        private ObservableCollection<Rating> _ratings;
        private Rating? _selectedRating;
        private double _averageRating;

        /// <summary>
        /// Collection of ratings for the selected product
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
        /// Currently selected rating
        /// </summary>
        public Rating? SelectedRating
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

        /// <summary>
        /// Average rating value for the selected product
        /// </summary>
        public double AverageRating
        {
            get => _averageRating;
            private set
            {
                double roundedValue = Math.Round(value, 2);
                if (_averageRating != roundedValue)
                {
                    _averageRating = roundedValue;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the RatingViewModel class
        /// </summary>
        /// <param name="ratingService">Service for rating data operations</param>
        public RatingViewModel(RatingService ratingService)
        {
            _ratingService = ratingService;
            _ratings = new ObservableCollection<Rating>();
        }

        /// <summary>
        /// Loads ratings for the specified product
        /// </summary>
        /// <param name="productId">ID of the product to load ratings for</param>
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
        /// Adds a new rating and refreshes the rating list
        /// </summary>
        /// <param name="rating">The rating to add</param>
        public void AddRating(Rating rating)
        {
            _ratingService.CreateRating(rating);
            
            // Reload the ratings so that the new one appears at the top
            LoadRatingsForProduct(rating.ProductId);
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
