using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imdbdrinks_ratingsmodule.Test
{
    public class ReviewServiceGetReviewsByRatingIdTest
    {
        private ReviewService _service;
        private Mock<IReviewRepository> _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IReviewRepository>();
            _service = new ReviewService(_repository.Object);
        }

        [Test]
        public void GetReviewsByRating_WhenCalled_ReturnsExpectedReviews()
        {   
            var ratingId = 1;
            var expectedReviews = new List<Review>
            {
                new Review { RatingId = ratingId, Content = "Nice." },
                new Review { RatingId = ratingId, Content = "Loved it!" }
            };
            var expectedReviewCount = expectedReviews.Count;
            _repository.Setup(r => r.GetReviewsByRatingId(ratingId)).Returns(expectedReviews);

            var result = _service.GetReviewsByRating(ratingId);

            Assert.That(result.Count(), Is.EqualTo(expectedReviewCount));
        }
    }
}
