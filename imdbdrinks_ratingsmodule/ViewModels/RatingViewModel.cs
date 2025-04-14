namespace imdbdrinks_ratingsmodule.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using imdbdrinks_ratingsmodule.Constants;
using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Services;
using imdbdrinks_ratingsmodule.ViewHelpers;

public class RatingViewModel: ViewModelBase
{
    private readonly RatingService ratingService;
    private ObservableCollection<Rating> ratings;
    private Rating selectedRating;
    private double averageRating;
    private ObservableCollection<BottleAsset> bottles;
    private int ratingScore;

    public virtual ObservableCollection<Rating> Ratings
    {
        get => ratings;
        set => ratings = value;
    }

    public virtual Rating SelectedRating
    {
        get => selectedRating;
        set => selectedRating = value;
    }

    public virtual double AverageRating
    {
        get => averageRating;
        set => averageRating = Math.Round(value, 2);
    }

    public virtual ObservableCollection<BottleAsset> Bottles
    {
        get => bottles;
        set => bottles = value;
    }

    public virtual int RatingScore
    {
        get => ratingScore;
        set => ratingScore = value;
    }

    private const int BottleRatingToIndexOffset = 1;
    private const int RatingsCountToUserOffset = 1;
    private const int PlaceholderItemId = 100;

    public RatingViewModel(RatingService ratingService)
    {
        this.ratingService = ratingService;
        Ratings = new ObservableCollection<Rating>();
        InitializeBottles();
    }

    protected virtual void InitializeBottles()
    {
        Bottles = new ObservableCollection<BottleAsset>();
        foreach (var currentRating in Enumerable.Range(RatingDomainConstants.MinRatingValue, RatingDomainConstants.MaxRatingValue))
        {
            var bottleToAdd = new BottleAsset { ImageSource = AssetConstants.EmptyBottlePath };
            Bottles.Add(bottleToAdd);
        }
    }

    public virtual void UpdateBottleRating(int clickedBottleNumber)
    {
        foreach (var currentRatingBottle in Enumerable.Range(RatingDomainConstants.MinRatingValue, RatingDomainConstants.MaxRatingValue))
        {
            var bottleIndex = currentRatingBottle - BottleRatingToIndexOffset;
            Bottles[bottleIndex].ImageSource = currentRatingBottle <= clickedBottleNumber 
                ? AssetConstants.FilledBottlePath 
                : AssetConstants.EmptyBottlePath;
        }

        RatingScore = clickedBottleNumber;
    }

    public virtual void AddRating()
    {
        if (RatingScore < RatingDomainConstants.MinRatingValue)
            return;

        Rating rating = new Rating
        {
            ProductId = PlaceholderItemId,
            RatingValue = RatingScore,
            UserId = GetUserId()
        };

        ratingService.CreateRating(rating);
        LoadRatingsForProduct(rating.ProductId);
    }

    public virtual void LoadRatingsForProduct(int productId)
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

    protected virtual int GetUserId()
    {
        return Ratings.Count + RatingsCountToUserOffset;
    }
}
