using System.Collections.Generic;
using imdbdrinks_ratingsmodule.Domain;

namespace imdbdrinks_ratingsmodule.Repositories
{
    public interface IRatingRepository
    {
        Rating FindById(int ratingId);

        IEnumerable<Rating> FindAll();

        IEnumerable<Rating> FindByProductId(int productId);

        Rating Save(Rating rating);

        Rating Add(Rating rating);

        Rating Update(Rating rating);

        void Delete(int ratingId);
    }
}
