namespace imdbdrinks_ratingsmodule.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using imdbdrinks_ratingsmodule.Constants;
    using imdbdrinks_ratingsmodule.Domain;
    using imdbdrinks_ratingsmodule.Services;
    using imdbdrinks_ratingsmodule.ViewHelpers;

    /// <summary>
    /// ViewModel for managing ratings and associated bottle assets.
    /// </summary>
    public class RatingViewModel : ViewModelBase
    {
        private readonly RatingService ratingService;
        private ObservableCollection<Rating> ratings;
        private Rating selectedRating;
        private double averageRating;
        private ObservableCollection<BottleAsset> bottles;
        private int ratingScore;

        /// <summary>
        /// Gets or sets the collection of ratings.
        /// </summary>
        public ObservableCollection<Rating> Ratings
        {
            get => ratings;
            set => SetProperty(ref ratings, value);
        }

        /// <summary>
        /// Gets or sets the selected rating.
        /// </summary>
        public Rating SelectedRating
        {
            get => selectedRating;
            set => SetProperty(ref selectedRating, value);
        }

        /// <summary>
        /// Gets or sets the average rating value, rounded to two decimal places.
        /// </summary>
        public double AverageRating
        {
            get => averageRating;
            set => SetProperty(ref averageRating, Math.Round(value, 2));
        }

        /// <summary>
        /// Gets or sets the collection of bottle assets.
        /// </summary>
        public ObservableCollection<BottleAsset> Bottles
        {
            get => bottles;
            set => SetProperty(ref bottles, value);
        }

        /// <summary>
        /// Gets or sets the rating score.
        /// </summary>
        public int RatingScore
        {
            get => ratingScore;
            set => SetProperty(ref ratingScore, value);
        }

        private const int BottleRatingToIndexOffset = 1;
        private const int RatingsCountToUserOffset = 1;
        private const int PlaceholderItemId = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingViewModel"/> class.
        /// </summary>
        /// <param name="ratingService">The service used to manage ratings.</param>
        public RatingViewModel(RatingService ratingService)
        {
            this.ratingService = ratingService ?? throw new ArgumentNullException(nameof(ratingService));
            Ratings = new ObservableCollection<Rating>();
            InitializeBottles();
        }

        private void InitializeBottles()
        {
            Bottles = new ObservableCollection<BottleAsset>();
            for (int i = RatingDomainConstants.MinRatingValue; i <= RatingDomainConstants.MaxRatingValue; i++)
            {
                var bottleToAdd = new BottleAsset { ImageSource = AssetConstants.EmptyBottlePath };
                Bottles.Add(bottleToAdd);
            }
        }

        /// <summary>
        /// Updates the bottle ratings based on the clicked bottle number.
        /// </summary>
        /// <param name="clickedBottleNumber">The number of the clicked bottle.</param>
        public void UpdateBottleRating(int clickedBottleNumber)
        {
            for (int i = RatingDomainConstants.MinRatingValue; i <= RatingDomainConstants.MaxRatingValue; i++)
            {
                int bottleIndex = i - BottleRatingToIndexOffset;
                Bottles[bottleIndex].ImageSource = i <= clickedBottleNumber
                    ? AssetConstants.FilledBottlePath
                    : AssetConstants.EmptyBottlePath;
            }

            RatingScore = clickedBottleNumber;
        }

        /// <summary>
        /// Adds a new rating based on the current rating score.
        /// </summary>
        public void AddRating()
        {
            if (RatingScore < RatingDomainConstants.MinRatingValue)
            {
                return;
            }

            var rating = new Rating
            {
                ProductId = PlaceholderItemId,
                RatingValue = RatingScore,
                UserId = GetUserId()
            };

            ratingService.CreateRating(rating);
            LoadRatingsForProduct(rating.ProductId);
        }

        /// <summary>
        /// Loads ratings for a specific product identified by its ID.
        /// </summary>
        /// <param name="productId">The ID of the product whose ratings are to be loaded.</param>
        public void LoadRatingsForProduct(int productId)
        {
            var ratingsForProduct = ratingService.GetRatingsByProduct(productId);
            var ratingsOrderedByNewest = ratingsForProduct.Reverse();

            Ratings.Clear();
            foreach (var rating in ratingsOrderedByNewest)
            {
                Ratings.Add(rating);
            }

            AverageRating = ratingService.GetAverageRating(productId);
        }

        private int GetUserId()
        {
            return Ratings.Count + RatingsCountToUserOffset;
        }
    }
}
