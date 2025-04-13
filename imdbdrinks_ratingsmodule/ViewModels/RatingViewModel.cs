namespace imdbdrinks_ratingsmodule.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Services;

public class RatingViewModel : ViewModelBase
{

    private readonly RatingService ratingService;
    private ObservableCollection<Rating> ratings;
    private Rating selectedRating;
    private double averageRating;

    public ObservableCollection<Rating> Ratings
    {
        get => ratings;
        set
        {
            if (ratings != value)
            {
                SetProperty(ref ratings, value);
            }
        }
    }

    public Rating SelectedRating
    {
        get => selectedRating;
        set
        {
            if (selectedRating != value)
            {
                SetProperty(ref selectedRating, value);
            }
        }
    }

    public double AverageRating
    {
        get => averageRating;
        set
        {
            double roundedValue = Math.Round(value, 2);
            if (averageRating != roundedValue)
            {
                SetProperty(ref averageRating, value);
            }
        }
    }

    public RatingViewModel(RatingService ratingService)
    {
        this.ratingService = ratingService;
        Ratings = new ObservableCollection<Rating>();
    }

    public void LoadRatingsForProduct(long productId)
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

    public void AddRating(int productId, int ratingValue)
    {
        Rating rating = new Rating
        {
            ProductId = productId,
            RatingValue = ratingValue,
            UserId = GetUserId()
        };

        ratingService.CreateRating(rating);

        LoadRatingsForProduct(rating.ProductId);
    }

    private int GetUserId()
    {
        const int ratingsNumberToUserOffset = 1;

        return Ratings.Count + ratingsNumberToUserOffset;
    }
}
