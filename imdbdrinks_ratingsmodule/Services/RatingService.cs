using System.Collections.Generic;
using System.Linq;
using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Repositories;

namespace imdbdrinks_ratingsmodule.Services;

public class RatingService
{
    private readonly IRatingRepository ratingRepository;

    public RatingService(IRatingRepository ratingRepository)
    {
        this.ratingRepository = ratingRepository;
    }

    public Rating GetRatingById(int ratingId) => this.ratingRepository.GetRatingById(ratingId);

    public IEnumerable<Rating> GetRatingsByProduct(int productId) => this.ratingRepository.GetRatingsByProductId(productId);

    public Rating CreateRating(Rating rating)
    {
        if (!rating.IsValid())
        {
            throw new System.ArgumentException("Invalid rating value.");
        }

        rating.RatingDate = System.DateTime.Now;
        rating.IsActive = true;

        return this.ratingRepository.AddRating(rating);
    }

    public Rating UpdateRating(Rating rating)
    {
        if (!rating.IsValid())
        {
            throw new System.ArgumentException("Invalid rating value.");
        }

        return this.ratingRepository.UpdateRating(rating);
    }

    public void DeleteRatingById(int ratingId) => this.ratingRepository.DeleteRatingById(ratingId);

    public double GetAverageRating(int productId)
    {
        var ratings = this.ratingRepository.GetRatingsByProductId(productId).Where(r => r.IsActive);

        if (!ratings.Any())
        {
            return 0;
        }

        return ratings.Average(r => r.RatingValue);
    }
}
