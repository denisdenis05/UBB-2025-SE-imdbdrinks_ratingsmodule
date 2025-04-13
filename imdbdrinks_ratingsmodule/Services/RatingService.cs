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

    public Rating GetRatingById(int ratingId) => this.ratingRepository.FindById(ratingId);

    public IEnumerable<Rating> GetRatingsByProduct(int productId) => this.ratingRepository.FindByProductId(productId);

    public Rating CreateRating(Rating rating)
    {
        if (!rating.IsValid())
        {
            throw new System.ArgumentException("Invalid rating value.");
        }

        rating.RatingDate = System.DateTime.Now;
        rating.IsActive = true;
        return this.ratingRepository.Save(rating);
    }

    public void DeleteRating(int ratingId) => this.ratingRepository.Delete(ratingId);

    public double GetAverageRating(int productId)
    {
        var ratings = this.ratingRepository.FindByProductId(productId).Where(r => r.IsActive);
        if (!ratings.Any())
        {
            return 0;
        }

        return ratings.Average(r => r.RatingValue);
    }
}
