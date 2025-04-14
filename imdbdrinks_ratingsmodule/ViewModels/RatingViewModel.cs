// <copyright file="RatingViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

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
        private const int BottleRatingToIndexOffset = 1;
        private const int RatingsCountToUserOffset = 1;
        private const int PlaceholderItemId = 100;

        private readonly RatingService ratingService;
        private ObservableCollection<Rating> ratings;
        private Rating? selectedRating;
        private double averageRating;
        private ObservableCollection<BottleAsset> bottles;
        private int ratingScore;

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingViewModel"/> class.
        /// </summary>
        /// <param name="ratingService">The service used to manage ratings.</param>
        public RatingViewModel(RatingService ratingService)
        {
            this.ratingService = ratingService ?? throw new ArgumentNullException(nameof(ratingService));
            this.Ratings = new ObservableCollection<Rating>();
            this.Bottles = new ObservableCollection<BottleAsset>();
            this.InitializeBottles();
        }

        /// <summary>
        /// Gets or sets the collection of ratings.
        /// </summary>
        public ObservableCollection<Rating> Ratings
        {
            get => this.ratings;
            set => this.SetProperty(ref this.ratings, value);
        }

        /// <summary>
        /// Gets or sets the selected rating.
        /// </summary>
        public Rating? SelectedRating
        {
            get => this.selectedRating;
            set => this.SetProperty(ref this.selectedRating, value);
        }

        /// <summary>
        /// Gets or sets the average rating value, rounded to two decimal places.
        /// </summary>
        public double AverageRating
        {
            get => this.averageRating;
            set => this.SetProperty(ref this.averageRating, Math.Round(value, 2));
        }

        /// <summary>
        /// Gets or sets the collection of bottle assets.
        /// </summary>
        public ObservableCollection<BottleAsset> Bottles
        {
            get => this.bottles;
            set => this.SetProperty(ref this.bottles, value);
        }

        /// <summary>
        /// Gets or sets the rating score.
        /// </summary>
        public int RatingScore
        {
            get => this.ratingScore;
            set => this.SetProperty(ref this.ratingScore, value);
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
                this.Bottles[bottleIndex].ImageSource = i <= clickedBottleNumber
                    ? AssetConstants.FilledBottlePath
                    : AssetConstants.EmptyBottlePath;
            }

            this.RatingScore = clickedBottleNumber;
        }

        /// <summary>
        /// Adds a new rating based on the current rating score.
        /// </summary>
        public void AddRating()
        {
            if (this.RatingScore < RatingDomainConstants.MinRatingValue)
            {
                return;
            }

            var rating = new Rating
            {
                ProductId = PlaceholderItemId,
                RatingValue = this.RatingScore,
                UserId = this.GetUserId(),
            };

            this.ratingService.CreateRating(rating);
            this.LoadRatingsForProduct(rating.ProductId);
        }

        /// <summary>
        /// Loads ratings for a specific product identified by its ID.
        /// </summary>
        /// <param name="productId">The ID of the product whose ratings are to be loaded.</param>
        public void LoadRatingsForProduct(int productId)
        {
            var ratingsForProduct = this.ratingService.GetRatingsByProduct(productId);
            var ratingsOrderedByNewest = ratingsForProduct.Reverse();

            this.Ratings.Clear();
            foreach (var rating in ratingsOrderedByNewest)
            {
                this.Ratings.Add(rating);
            }

            this.AverageRating = this.ratingService.GetAverageRating(productId);
        }

        private void InitializeBottles()
        {
            this.Bottles = new ObservableCollection<BottleAsset>();
            for (int i = RatingDomainConstants.MinRatingValue; i <= RatingDomainConstants.MaxRatingValue; i++)
            {
                var bottleToAdd = new BottleAsset { ImageSource = AssetConstants.EmptyBottlePath };
                this.Bottles.Add(bottleToAdd);
            }
        }

        private int GetUserId()
        {
            return this.ratings.Count + RatingsCountToUserOffset;
        }
    }
}
