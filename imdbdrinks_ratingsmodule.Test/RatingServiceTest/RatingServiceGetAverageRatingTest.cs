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
    public class RatingServiceGetAverageRatingTest
    {
        private RatingService _service;
        private Mock<IRatingRepository> _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IRatingRepository>();
            _service = new RatingService(_repository.Object);
        }

        [Test]
        public void GetAverageRating_WhenActiveRatingsExist_ReturnsCorrectAverage()
        {
            var ratings = new List<Rating>
            {
                new Rating { RatingValue = 3.0, IsActive = true },
                new Rating { RatingValue = 4.0, IsActive = true },
                new Rating { RatingValue = 5.0, IsActive = false }
            };

            _repository.Setup(r => r.FindByProductId(1)).Returns(ratings);

            var result = _service.GetAverageRating(1);

            Assert.That(result, Is.EqualTo(3.5));
        }

        [Test]
        public void GetAverageRating_WhenNoActiveRatingsExist_ReturnsZero()
        {
            var ratings = new List<Rating>();
            _repository.Setup(r => r.FindByProductId(1)).Returns(ratings);

            var result = _service.GetAverageRating(1);

            Assert.That(result, Is.Zero);
        }
    }
}
